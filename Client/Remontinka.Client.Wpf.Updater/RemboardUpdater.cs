using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Remontinka.Client.Wpf.Updater
{
    public class RemboardUpdater
    {
#if TESTBUILD
        /// <summary>
        /// Содержит URL Для обновления системы направленный на тестовый сервер.
        /// </summary>
        private const string UpdateInfoURL = "http://.ru/updatetest.conf";
#else
        /// <summary>
        /// Содержит URL Для обновления системы направленный на боевой сервер.
        /// </summary>
        private const string UpdateInfoURL = "http://remboard.ru/soft/updaterem.conf";
#endif
        /// <summary>
        /// Содержит название файла с текущими правилами.
        /// </summary>
        private const string CurrentUpdateFile = "updaterem.conf";

        /// <summary>
        /// Содержит имя файла, который был скачен с сервиса обновлений.
        /// </summary>
        private const string DownloadedUpdateFile = "updaterem.confs";

        /// <summary>
        /// Содержит название файла клиента.
        /// </summary>
        private const string RemboardClientFile = "Remboard.exe";

        /// <summary>
        /// Файл конфигурации клиента.
        /// </summary>
        private const string RemboardClientConfigFile = "Remboard.exe.config";

        /// <summary>
        /// Содержит текущую модель.
        /// </summary>
        private MainModel _model;

        /// <summary>
        /// Происходит когда необходимо закрыть приложение.
        /// </summary>
        public event EventHandler NeedExit;

        private void RiseNeedExit()
        {
            EventHandler handler = NeedExit;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Старт обновления системы.
        /// </summary>
        /// <param name="model">Модель для View.</param>
        public void StartUpdate(MainModel model)
        {

            _model = model;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            ThreadPool.QueueUserWorkItem(StartUpdate);
        }

        /// <summary>
        /// Начинает процесс загрузки данных и обновлений.
        /// </summary>
        /// <param name="state">Состояние.</param>
        private void StartUpdate(object state)
        {
            try
            {
                _model.Description = "Старт загрузки файла обновлений";
                var fullPath = GetFullPath(DownloadedUpdateFile);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                } //if

                var client = new WebClient();
                client.DownloadFile(UpdateInfoURL, fullPath);

                var newUpdates = Parse(DownloadedUpdateFile);
                var currentUpdates = Parse(CurrentUpdateFile);

                var filesToDownload = Diff(currentUpdates, newUpdates);

                var index = 1;

                if (!filesToDownload.Any())
                {
                    _model.Description = "Обновлений нет";
                } //if

                foreach (string file in filesToDownload)
                {
                    ProcessFile(file);
                    _model.TotalPercent = (index / (double)filesToDownload.Count) * 100;
                    index++;
                } //foreach

                var oldPath = GetFullPath(CurrentUpdateFile);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                } //if

                File.Copy(fullPath, oldPath);

                StartClient(false);

                RiseNeedExit();
            }
            catch (Exception ex)
            {
                _model.HasError = true;
                _model.Description = ex.Message;

            } //try
        }

        /// <summary>
        /// Стартует клиент на исполнение.
        /// </summary>
        public bool StartClient(bool isOffline)
        {
            var clientPath = GetFullPath(RemboardClientFile);

            var arguments = "updatecompleted";
            if (isOffline)
            {
                arguments += " offlinemode";
            } //if

            if (!File.Exists(clientPath))
            {
                return false;
            }

            Process.Start(clientPath, arguments);

            return true;
        }

        /// <summary>
        /// Содержит текущий загрузчик.
        /// </summary>
        private FileDownloader _currentDownloader;

        private readonly ManualResetEvent _manualReset = new ManualResetEvent(false);

        private const string TempExtention = ".1tmp1";

        /// <summary>
        /// Загрузка файлов.
        /// </summary>
        /// <param name="file">Файл для скачивания.</param>
        private void ProcessFile(string file)
        {
            if (_currentDownloader != null)
            {
                _currentDownloader.DownloadCompleted -= DownloadCompleted;
                _currentDownloader.ProgressChanged -= ProgressChanged;
                _currentDownloader.ResponseInfoReceived -= ResponseInfoReceived;
            } //if

            _manualReset.Reset();

            _currentDownloader = new FileDownloader();
            _model.CurrentFile = Path.GetFileName(file);
            _currentDownloader.DownloadCompleted += DownloadCompleted;
            _currentDownloader.ProgressChanged += ProgressChanged;
            _currentDownloader.ResponseInfoReceived += ResponseInfoReceived;
            var fileToCopy = file;
            if (fileToCopy.ToLower().Contains(RemboardClientConfigFile.ToLower()))
            {
                fileToCopy = RemboardClientConfigFile;
            } //if
            
            var normalPath = GetFullPath(Path.GetFileName(fileToCopy));
            var path = normalPath + TempExtention;
            
            _currentDownloader.Start(file, path);
            _manualReset.WaitOne();

            if (File.Exists(normalPath))
            {
                File.Delete(normalPath);
            } //if

            File.Move(path, normalPath);

        }

        void ResponseInfoReceived(object sender, ResponseInfoEventArgs e)
        {
            if (e.HasError)
            {
                _model.Description = string.Format("Произошла ошибка: {0}", e.StatusDescription);
                _model.HasError = true;
            } //if
            else
            {
                _model.Description = e.StatusDescription;
            } //else

        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _model.CurrentBytesRead = e.TotalBytes;
            _model.CurrentTransaferRate = e.TransferRate;
            _model.CurrentPercent = e.PercentCompleted;
        }

        void DownloadCompleted(object sender, EventArgs e)
        {
            _manualReset.Set();
        }

        /// <summary>
        /// Высчисляет необходимые файлы для загрузки.
        /// </summary>
        /// <param name="currentList">Текущий список.</param>
        /// <param name="newList">Новый список.</param>
        /// <returns>Файлы для загрузки.</returns>
        private IList<string> Diff(IEnumerable<KeyValuePair<string, int>> currentList, IDictionary<string, int> newList)
        {
            var result = newList.Where(i => currentList.All(cl => !StringComparer.OrdinalIgnoreCase.Equals(i.Key, cl.Key)))
                .Union(
                    newList.Where(i => currentList.Any(cl => StringComparer.OrdinalIgnoreCase.Equals(i.Key, cl.Key) && i.Value != cl.Value))
                ).Select(i => i.Key).ToList();


            return result;
        }

        /// <summary>
        /// Распрарсивает файл со значениями обновлений.
        /// </summary>
        /// <param name="file">Файл для анализа</param>
        /// <returns>Содержимое.</returns>
        private IDictionary<string, int> Parse(string file)
        {
            var result = new Dictionary<string, int>();

            var fullPath = GetFullPath(file);

            if (!File.Exists(fullPath))
            {
                return result;
            } //if

            var lines = File.ReadAllLines(fullPath);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { '#' });
                if (parts.Length == 2)
                {
                    int number;
                    if (int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                    {
                        result.Add(parts[0], number);
                    } //if
                } //if
            } //foreach

            return result;
        }

        /// <summary>
        /// Возвращает полный путь к файлу находящемся в текуще директории с программой.
        /// </summary>
        /// <param name="fileName">Номер файла.</param>
        /// <returns>Полный путь.</returns>
        private string GetFullPath(string fileName)
        {
            var directory = Path.GetDirectoryName(typeof(RemboardUpdater).Assembly.Location);

            return Path.Combine(directory, fileName);
        }
    }
}

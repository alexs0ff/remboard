using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Remontinka.Server.Crypto
{
    /// <summary>
    /// Мнеджер управления OpenSSL для генерации RSA ключ
    /// </summary>
    public class OpenSslManager
    {
        /// <summary>
        /// Содержит путь к openssl файлу.
        /// </summary>
        private const string OpenSslFilePath = @"openssl.exe";

        /// <summary>
        /// Формат команды на запуск нового секретного ключа.
        /// </summary>
        private const string CreateNewPrivateFileFormat = "genrsa -des3 -passout pass:{0} -out \"{1}\" 2048";

        /// <summary>
        /// Формат команды на запуск получения публичного ключа из приватного.
        /// </summary>
        private const string CreatePublicFileFormat = "rsa -in \"{0}\" -outform PEM -passin pass:{1} -pubout -out \"{2}\"";

        /// <summary>
        /// Производит генерацию новых ключей RSA.
        /// </summary>
        /// <param name="privateKeyPath">Путь к приватному ключу. </param>
        /// <param name="password">Пароль к приватному ключу SSL.</param>
        /// <param name="openSslPath">Путь к файлу openssl (без имени файла).</param>
        /// <param name="publicKeyPath">Путь к публичному ключу.</param>
        public void GenerateRSAKeys(string openSslPath,string publicKeyPath,string privateKeyPath,string password)
        {
            var privPath = privateKeyPath;
            var publicPath = publicKeyPath;

            //Удаляем файлы, если есть
            if (File.Exists(privPath))
            {
                File.Delete(privPath);
            } //if

            if (File.Exists(publicPath))
            {
                File.Delete(publicPath);
            } //if

            var opensslPath = Path.Combine(openSslPath, OpenSslFilePath);

            if (!File.Exists(opensslPath))
            {
                throw new Exception("OpenSSL Отсутвует");
            } //if

            var privCmd = string.Format(CreateNewPrivateFileFormat, password, privPath);

            Execute(opensslPath, privCmd);

            if (!File.Exists(privPath))
            {
                throw new Exception("Секретный файл не создался");
            } //if

            var pubCmd = string.Format(CreatePublicFileFormat, privPath, password, publicPath);

            Execute(opensslPath, pubCmd);

            if (!File.Exists(publicPath))
            {
                throw new Exception("Публичный файл не создался");
            } //if
        }

        /// <summary>
        /// Запускает на исполнение команду в ОС.
        /// </summary>
        /// <param name="file">Файл для запуска.</param>
        /// <param name="commandText">Сформированный текст комманды.</param>
        private void Execute(string file, string commandText)
        {
            var info = new ProcessStartInfo(file, commandText);
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            var process = Process.Start(info);
            process.WaitForExit();

        }
    }
}

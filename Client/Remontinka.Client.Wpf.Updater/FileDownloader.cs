using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// Downloader файлов. Работает с Ftp и HTTP.
    /// </summary>
    public class FileDownloader
    {
        private const int BUFFER_SIZE = 1448; // TCP packet size if 1448 bytes (seen thru inspection)

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        private WebRequestState _state;

        /// <summary>
        /// Содержит ссылку на файл в который идет запись.
        /// </summary>
        private string _filePath;

        /// <summary>
        /// Вызывается для потверждения логина и пароля для FTP клиента.
        /// </summary>
        public event EventHandler<FtpСredentialsNeedEventArgs> FtpСredentialsNeed;

        private void RiseFtpСredentialsNeed(FtpСredentialsNeedEventArgs e)
        {
            EventHandler<FtpСredentialsNeedEventArgs> handler = FtpСredentialsNeed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Происходит во время получения информации.
        /// </summary>
        public event EventHandler<ResponseInfoEventArgs> ResponseInfoReceived;

        /// <summary>
        /// Происходит когда изменяется прогресс загрузки.
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private void RiseProgressChanged(int totalBytes, double percentCompleted, double transferRate)
        {
            EventHandler<ProgressChangedEventArgs> handler = ProgressChanged;
            if (handler != null)
            {
                handler(this, new ProgressChangedEventArgs(totalBytes, percentCompleted, transferRate));
            }
        }

        /// <summary>
        /// Происходит при завершении процесса загрузки.
        /// </summary>
        public event EventHandler DownloadCompleted;

        private void RiseDownloadCompleted()
        {
            EventHandler handler = DownloadCompleted;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Вызывает события изменения статуса.
        /// </summary>
        /// <param name="status">Текущий статус.</param>
        /// <param name="length">Длина.</param>
        /// <param name="hasError">Наличие ошибки.</param>
        public void RiseResponseInfoReceived(string status, long length, bool hasError)
        {
            EventHandler<ResponseInfoEventArgs> handler = ResponseInfoReceived;
            if (handler != null)
            {
                handler(this, new ResponseInfoEventArgs(status, length, hasError));
            }
        }

        /// <summary>
        /// Начинает закачку данных с опеределеного ресураса.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath">Путь к закачиваемому файлу. </param>
        public void Start(string url, string filePath)
        {
            _filePath = filePath;
            var fileUri = new Uri(url);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            } //if

            if (fileUri.Scheme == Uri.UriSchemeHttp)
            {
                _state = new HttpWebRequestState(BUFFER_SIZE);
                _state.request = WebRequest.Create(fileUri);
            }
            else if (fileUri.Scheme == Uri.UriSchemeFtp)
            {
                var args = new FtpСredentialsNeedEventArgs();

                RiseFtpСredentialsNeed(args);
                _state = new FtpWebRequestState(BUFFER_SIZE);
                _state.request = FtpWebRequest.Create(fileUri);
                _state.request.Credentials = new NetworkCredential(args.Login, args.Password);

                // Set FTP-specific stuff
                ((FtpWebRequest)_state.request).KeepAlive = false;

                //Проверка на существование системного proxy
                var systemProxy = WebRequest.GetSystemWebProxy();

                if (systemProxy.IsBypassed(new Uri(url)))
                {
                    // First thing we do is get file size.  2nd step, done later, 
                    // will be to download actual file.
                    _state.request.Method = WebRequestMethods.Ftp.GetFileSize;
                    _state.FtpMethod = WebRequestMethods.Ftp.GetFileSize;
                }
                else
                {
                    _state.request.Method = WebRequestMethods.Ftp.DownloadFile;
                    _state.FtpMethod = WebRequestMethods.Ftp.DownloadFile;
                } //else
            }
            else
            {
                throw new Exception("Ошибочная схема: " + fileUri.Scheme);
            } //else

            _state.FileUri = fileUri;

            _state.TransferStart = DateTime.Now;

            // Start the asynchronous request.
            _state.request.BeginGetResponse(RespCallback, null);
        }

        /// <summary>
        /// Callback для вызова после первого ответа.
        /// </summary>
        private void RespCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Will be either HttpWebRequestState or FtpWebRequestState
                WebRequestState reqState = _state;
                WebRequest req = reqState.request;
                string statusDescr = "";
                long contentLength = 0;

                // HTTP 
                if (reqState.FileUri.Scheme == Uri.UriSchemeHttp)
                {
                    HttpWebResponse resp = ((HttpWebResponse)(req.EndGetResponse(asyncResult)));
                    reqState.response = resp;
                    statusDescr = resp.StatusDescription;
                    reqState.TotalBytes = reqState.response.ContentLength;
                    contentLength = reqState.response.ContentLength;   // # bytes
                }

                // FTP part 1 - response to GetFileSize command
                else if ((reqState.FileUri.Scheme == Uri.UriSchemeFtp) &&
                         (reqState.FtpMethod == WebRequestMethods.Ftp.GetFileSize))
                {
                    // First FTP command was GetFileSize, so this 1st response is the size of 
                    // the file.
                    FtpWebResponse resp = ((FtpWebResponse)(req.EndGetResponse(asyncResult)));
                    statusDescr = resp.StatusDescription;
                    reqState.TotalBytes = resp.ContentLength;
                    contentLength = resp.ContentLength;   // # bytes
                }

                // FTP part 2 - response to DownloadFile command
                else if ((reqState.FileUri.Scheme == Uri.UriSchemeFtp) &&
                         (reqState.FtpMethod == WebRequestMethods.Ftp.DownloadFile))
                {
                    FtpWebResponse resp = ((FtpWebResponse)(req.EndGetResponse(asyncResult)));
                    reqState.response = resp;
                }

                else
                {
                    throw new ApplicationException("Unexpected URI");
                }


                // Get this info back to the GUI -- max # bytes, so we can do progress bar
                if (!string.IsNullOrWhiteSpace(statusDescr))
                {
                    RiseResponseInfoReceived(statusDescr, contentLength, false);
                } //if

                // FTP part 1 done, need to kick off 2nd FTP request to get the actual file
                if ((reqState.FileUri.Scheme == Uri.UriSchemeFtp) && (reqState.FtpMethod == WebRequestMethods.Ftp.GetFileSize))
                {
                    // Note: Need to create a new FtpWebRequest, because we're not allowed to change .Method after
                    // we've already submitted the earlier request.  I.e. FtpWebRequest not recyclable.
                    // So create a new request, moving everything we need over to it.
                    FtpWebRequest req2 = (FtpWebRequest)FtpWebRequest.Create(reqState.FileUri);
                    req2.Credentials = req.Credentials;
                    req2.UseBinary = true;
                    req2.KeepAlive = true;
                    req2.Method = WebRequestMethods.Ftp.DownloadFile;

                    reqState.request = req2;
                    reqState.FtpMethod = WebRequestMethods.Ftp.DownloadFile;

                    // Start the asynchronous request, which will call back into this same method
                    req2.BeginGetResponse(new AsyncCallback(RespCallback), reqState);
                }
                else	// HTTP or FTP part 2 -- we're ready for the actual file download
                {
                    // Set up a stream, for reading response data into it
                    Stream responseStream = reqState.response.GetResponseStream();
                    reqState.StreamResponse = responseStream;

                    _state.StreamWriter = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                    // Begin reading contents of the response data
                    responseStream.BeginRead(reqState.BufferRead, 0, BUFFER_SIZE, ReadCallback, null);
                }

            }
            catch (Exception ex)
            {
                RiseResponseInfoReceived(string.Format("Ошибка чтения {0} ({1})", ex.Message, ex.GetType()), 0, true);
                if (_state.StreamWriter != null)
                {
                    _state.StreamWriter.Close();
                } //if
            }
        }

        /// <summary>
        /// Callback для вызыова в периоды получения данных..
        /// </summary>
        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                // Will be either HttpWebRequestState or FtpWebRequestState
                WebRequestState reqState = _state;

                Stream responseStream = reqState.StreamResponse;

                // Get results of read operation
                int bytesRead = responseStream.EndRead(ar);

                // Got some data, need to read more
                if (bytesRead > 0)
                {
                    _state.StreamWriter.Write(_state.BufferRead, 0, bytesRead);
                    // Report some progress, including total # bytes read, % complete, and transfer rate
                    reqState.BytesRead += bytesRead;
                    double pctComplete = ((double)reqState.BytesRead / reqState.TotalBytes) * 100.0f;

                    // Note: bytesRead/totalMS is in bytes/ms.  Convert to kb/sec.
                    TimeSpan totalTime = DateTime.Now - reqState.TransferStart;
                    double kbPerSec = (reqState.BytesRead * 1000.0f) / (totalTime.TotalMilliseconds * 1024.0f);

                    RiseProgressChanged(reqState.BytesRead, pctComplete, kbPerSec);

                    // Kick off another read
                    responseStream.BeginRead(reqState.BufferRead, 0, BUFFER_SIZE, ReadCallback, reqState);

                }

                // EndRead returned 0, so no more data to be read
                else
                {
                    responseStream.Close();
                    reqState.response.Close();
                    _state.StreamWriter.Close();
                    RiseDownloadCompleted();
                }
            }
            catch (Exception ex)
            {
                if (_state.StreamWriter != null)
                {
                    _state.StreamWriter.Close();
                } //if
                RiseResponseInfoReceived(string.Format("Ошибка загрузки {0} ({1})", ex.Message, ex.GetType()), 0, true);
            }
        }
    }
}

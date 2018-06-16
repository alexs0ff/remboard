using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// Base class for state object that gets passed around amongst async methods 
    /// when doing async web request/response for data transfer.  We store basic 
    /// things that track current state of a download, including # bytes transfered,
    /// as well as some async callbacks that will get invoked at various points.
    /// </summary>
    public abstract class WebRequestState
    {
        /// <summary>
        /// Задает или получает количество байтов за текущую передачу.
        /// </summary>
        public int BytesRead { get; set; }

        /// <summary>
        /// Задает ли получает общее количество байтов, которое было прочитано от начала.
        /// </summary>
        public long TotalBytes { get; set; }

        /// <summary>
        /// Задает или получает дульту % для каждого чтения из буфера.
        /// </summary>
        public double ProgIncrement { get; set; }

        /// <summary>
        /// Задает или получает поток откуда ведется чтение данных.
        /// </summary>
        public Stream StreamResponse { get; set; }

        /// <summary>
        /// Задает или получает буффер куда ведется запись полученных данных.
        /// </summary>
        public byte[] BufferRead { get; private set; }

        /// <summary>
        /// Задает или получает URL ресурс скачивания файла.
        /// </summary>
        public Uri FileUri { get; set; }

        /// <summary>
        /// Задает или получает метод FTP.
        /// </summary>
        public string FtpMethod { get; set; }

        /// <summary>
        /// Задает или получает вреся начала загрузки.
        /// </summary>
        public DateTime TransferStart { get; set; }

        private WebRequest _request;

        public virtual WebRequest request
        {
            get { return null; }
            set { _request = value; }
        }

        private WebResponse _response;

        public virtual WebResponse response
        {
            get { return null; }
            set { _response = value; }
        }

        public FileStream StreamWriter { get; set; }

        public WebRequestState(int buffSize)
        {
            BytesRead = 0;
            BufferRead = new byte[buffSize];
            StreamResponse = null;
        }
    }
}

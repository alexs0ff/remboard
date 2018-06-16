using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Romontinka.Server.ProtocolServices;
using log4net;

namespace Romontinka.Server.WebSite.Api
{
    /// <summary>
    /// Асинхронная операция протокола взаимодействия.
    /// </summary>
    public class ProtocolRequestOperation : IAsyncResult
    {
         /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProtocolRequestOperation));

        private bool _completed;

        private Object _state;

        private AsyncCallback _callback;

        private HttpContext _context;

        bool IAsyncResult.IsCompleted
        {
            get { return _completed; }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return null; }
        }

        Object IAsyncResult.AsyncState
        {
            get { return _state; }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return false; }
        }

        public ProtocolRequestOperation(AsyncCallback callback, HttpContext context, Object state)
        {
            _callback = callback;
            _context = context;
            _state = state;
            _completed = false;
        }

        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(StartAsyncTask, null);
        }

        /// <summary>
        ///   Главный метод обработки запросов к веб серверу.
        /// </summary>
        /// <param name="workItemState"> </param>
        private void StartAsyncTask(Object workItemState)
        {
            var result = GetType().FullName;
            try
            {
                using (
                    var reader = new StreamReader(_context.Request.InputStream,
                                                  Encoding.UTF8))
                {
                    var request = reader.ReadToEnd();
                    
                    if (!string.IsNullOrEmpty(request))
                    {
                        result = ProtocolServer.Instance.ProcessRequest(request,_context.Request.UserHostAddress);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unhandled exception", ex);
                result = "critical";
            } //try
            using (
                var writer = new StreamWriter(_context.Response.OutputStream,
                                              Encoding.UTF8))
            {
                writer.Write(result);
            }
            _completed = true;
            _callback(this);
        }
    }
}
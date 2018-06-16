using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Romontinka.Server.WebSite.Api
{
    /// <summary>
    /// Хендлер протокольных запросов.
    /// </summary>
    public class ProtocolRequestHandler : IHttpAsyncHandler, IReadOnlySessionState
    {
        /// <summary>
        /// Запускает асинхронный вызов обработчика НТТР.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.IAsyncResult"/>, содержащий сведения о состоянии процесса.
        /// </returns>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/>, предоставляющий ссылки на внутренние серверные объекты (например, Request, Response, Session и Server), которые используются для обслуживания HTTP-запросов. </param><param name="cb">Метод <see cref="T:System.AsyncCallback"/>, который вызывается после завершения асинхронного вызова метода.Если значение параметра <paramref name="cb"/> равно null, делегат не вызывается.</param><param name="extraData">Любые дополнительные данные, необходимые для обработки запроса. </param>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var asynch = new ProtocolRequestOperation(cb, context, extraData);
            asynch.StartAsyncWork();
            return asynch;
        }

        /// <summary>
        /// Предоставляет метод End асинхронного процесса после завершения процесса.
        /// </summary>
        /// <param name="result">Объект <see cref="T:System.IAsyncResult"/>, содержащий сведения о состоянии процесса. </param>
        public void EndProcessRequest(IAsyncResult result)
        {
        }

        /// <summary>
        /// Возвращает значение, позволяющее определить, может ли другой запрос использовать экземпляр класса <see cref="T:System.Web.IHttpHandler"/>.
        /// </summary>
        /// <returns>
        /// Значение true, если экземпляр <see cref="T:System.Web.IHttpHandler"/> доступен для повторного использования; в противном случае — значение false.
        /// </returns>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Разрешает обработку веб-запросов НТТР для пользовательского элемента HttpHandler, который реализует интерфейс <see cref="T:System.Web.IHttpHandler"/>.
        /// </summary>
        /// <param name="context">Объект <see cref="T:System.Web.HttpContext"/>, предоставляющий ссылки на внутренние серверные объекты (например, Request, Response, Session и Server), используемые для обслуживания HTTP-запросов. </param>
        public void ProcessRequest(HttpContext context)
        {
        }
    }
}
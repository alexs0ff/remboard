using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Базовый класс всех подписанных запросов.
    /// TODO: сделать поддержку сессий для запросов.
    /// Потому сейчас можно перехватывать сообщения и отправлять еще раз.
    /// </summary>
    public abstract class SignedRequestBase : MessageBase
    {
        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Заносит поля в сообщении для подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя.</param>
        protected abstract void ProcessFields(MessageSignVisitorBase signVisitor);

        /// <summary>
        /// Производит процес подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя.</param>
        public void Sign(MessageSignVisitorBase signVisitor)
        {
            ProcessFields(signVisitor);
            SignData = signVisitor.CreateSign();
        }

        /// <summary>
        /// Возвращает сырые данные для проверки подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя</param>
        /// <returns>Сырые данные.</returns>
        public string GetDataForSign(MessageSignVisitorBase signVisitor)
        {
            ProcessFields(signVisitor);
            return signVisitor.GetRawDataToSign();
        }

        /// <summary>
        /// Задает или получает подпись сообщения.
        /// </summary>
        public string SignData { get; internal set; }
    }
}

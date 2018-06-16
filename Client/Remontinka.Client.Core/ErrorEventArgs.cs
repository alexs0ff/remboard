using System;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// ��������� ������� � ��������.
    /// </summary>
    public class ErrorEventArgs:EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public ErrorEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public ErrorEventArgs(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
            Message = exception.Message;
        }

        /// <summary>
        /// �������� ������ ��������� �� ������.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// �������� ���������� �� ������.
        /// </summary>
        public Exception Exception { get; private set; }
    }
}

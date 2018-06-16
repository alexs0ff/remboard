using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Remontinka.Client.Wpf.Controllers
{
    /// <summary>
    /// Базовый контроллер для контроллеров отображения.
    /// </summary>
    public abstract class BaseController
    {
        /// <summary>
        /// Инициализация данных контроллера.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Завершает действие контроллера, освобождая его ресурсы.
        /// </summary>
        public abstract void Terminate();

        /// <summary>
        /// Получает View для отображения на форме.
        /// </summary>
        /// <returns>View.</returns>
        public abstract UserControl GetView();

        /// <summary>
        /// Вызывается для определения возможности завершения работы.
        /// Вызывается системой для проверки можно ли закрыть текущее представление и отобразить другое.
        /// </summary>
        /// <returns>Признак завершения.</returns>
        public virtual bool CanTerminate()
        {
            return true;
        }

        /// <summary>
        /// Вызывает метод в контексте синхронизации представления.
        /// </summary>
        /// <param name="method">Метод для вызова.</param>
        protected void SaveInvoke(Action method)
        {
            var view = GetView();
            if (method!=null && view!=null)
            {
                if (!view.Dispatcher.CheckAccess())
                {
                    view.Dispatcher.Invoke(method);
                } //if
                else
                {
                    method();
                } //else
            } //if
        }

        /// <summary>
        /// Вызывает метод в контексте синхронизации представления без блокировки потока.
        /// </summary>
        /// <param name="method">Метод для вызова.</param>
        protected void StartSaveInvoke(Action method)
        {
            var view = GetView();
            if (method != null && view != null)
            {
                if (!view.Dispatcher.CheckAccess())
                {
                    view.Dispatcher.BeginInvoke(method);
                } //if
                else
                {
                    method();
                } //else
            } //if
        }

        /// <summary>
        /// Стартует задачу с выполнением результатов в контексте синхронизации.
        /// </summary>
        /// <typeparam name="T">Тип результата выполнения.</typeparam>
        /// <param name="functToExec">Функция для выполнения.</param>
        /// <param name="successFunct">Функция для обработки результатов.</param>
        /// <param name="errorFunct">Вызываемая функция при ошибке.</param>
        /// <returns>Токен отмены.</returns>
        protected CancellationTokenSource SaveStartTask<T>(Func<CancellationTokenSource, T> functToExec, Action<T> successFunct, Action<Exception, bool,bool> errorFunct)
        {
            var result = new CancellationTokenSource();

            var task =
                new Task<T>(() => functToExec(result), result.Token);
				
            task.ContinueWith(t => successFunct(t.Result), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
            if (errorFunct!=null)
            {
                task.ContinueWith(t => errorFunct(t.Exception, false, t.IsFaulted), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                task.ContinueWith(t => errorFunct(t.Exception, t.IsCanceled, false), CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, TaskScheduler.FromCurrentSynchronizationContext());    
            }
            task.Start();
            return result;
        }
    }
}

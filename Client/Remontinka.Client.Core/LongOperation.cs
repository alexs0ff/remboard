using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Модель длительной операции.
    /// Value измеряется от 0 до 100 %
    /// </summary>
    public class LongOperation : BindableModelObject
    {
        /// <summary>
        /// Объект синхронизации.
        /// </summary>
        private readonly object _syncRoot = new object();

        /// <summary>
        /// Задает или получает текущее значение
        /// </summary>
        [NotifyPropertyChanged]
        public virtual int Value { get; set; }

        /// <summary>
        /// Задает или получает название операции.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Title { get; set; }

        /// <summary>
        /// Получает значение свободности данной операции.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsReleased { get; set; }

        /// <summary>
        /// Освобождает данную операцию.
        /// </summary>
        public void Release()
        {
            lock (_syncRoot)
            {
                IsReleased = true;
            } //lock
        }

        private int _maxValue;

        /// <summary>
        /// Пробует захватить данную операцию.
        /// </summary>
        /// <param name="maxValue">Максимальное значение.</param>
        /// <param name="currentValue">текущее значение.</param>
        /// <returns>True - если захват положительный.</returns>
        public bool Set(int maxValue, int currentValue)
        {
            lock (_syncRoot)
            {
                if (!IsReleased)
                {
                    return false;
                } //if
                IsReleased = false;

                _maxValue = maxValue;
                SetCurrentValue(currentValue);
                return true;
            } //lock
        }

        public void SetCurrentValue(int value)
        {
            lock (_syncRoot)
            {
                if (_maxValue == 0)
                {
                    return;
                } //if
                Value = (value * 100) / _maxValue;
            } //lock
        }
    }
}

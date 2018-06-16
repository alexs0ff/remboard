using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Remontinka.Client.Core.Context
{
    /// <summary>
    /// Создает строку изходя из данных контекста. 
    /// <remarks>В качестве примера: есть строка "Платежи за период {BeginDate}-{EndDate} количество {Count}, наименование поставщика {ProviderTitle}"
    /// мы создаем ContextStringBuilder и добавляем с помощью переопределенных классов наследованных от ContextItemBase все необходимые значения.
    ///  </remarks>
    /// </summary>
    public class ContextStringBuilder
    {
        /// <summary>
        /// Здесь содержатся значения для замены.
        /// </summary>
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Здесь содержатся значения которые не обnullяются после вызова метода SetEmptyValues
        /// </summary>
        private readonly HashSet<string> _forcedValues = new HashSet<string>();

        /// <summary>
        /// Добавление всех значений существующих в переопределенных классах.
        /// </summary>
        /// <param name="contextItem">Пункт контекста.</param>
        public void Add(ContextItemBase contextItem)
        {
            var ht = contextItem.GetValues();
            foreach (DictionaryEntry entry in ht)
            {
                if (!_values.ContainsKey((string)entry.Key))
                {
                    _values.Add((string)entry.Key, entry.Value);
                } //if
                else
                {
                    _values[(string) entry.Key] = entry.Value;
                } //else
            } //foreach
        }

        /// <summary>
        /// Добавление всех значений существующих в переопределенных классах и обозначение их как "нестираемых"
        /// </summary>
        /// <param name="contextItem"></param>
        public void AddReadonly(ContextItemBase contextItem)
        {
            Add(contextItem);

            foreach (DictionaryEntry entry in contextItem.GetValues())
            {
                if (!_forcedValues.Contains((string)entry.Key))
                {
                    _forcedValues.Add((string)entry.Key);
                } //if
            }
        }

        /// <summary>
        /// Выставление всех значений в null, кроме тех кто обозначен как "нестираемый"
        /// </summary>
        public void SetEmptyValues()
        {
            var copy = _values.ToList();
            foreach (var value in copy)
            {
                if (!_forcedValues.Contains(value.Key))
                {
                    _values[value.Key] = null;
                } //if
            } //foreach
        }

        /// <summary>
        /// Заменяет значения заданные в форматированой строке значениями содержащимися в контексте выполнения.
        /// </summary>
        /// <param name="source">Форматированная строка.</param>
        /// <returns>Результат замены значениями.</returns>
        public string Create(string source)
        {
            return source.Inject( (IDictionary)_values );
        }

        /// <summary>
        /// Заменяет значения заданные в форматированой строке значениями содержащимися в контексте выполнения или возвращает конкретное значение без кастинга в строку.
        /// {ValueName} - эта строка будет заменена конкретным значением, без перевода в строку.
        /// </summary>
        /// <param name="source">Форматированная строка.</param>
        /// <returns>Результат замены значениями или значение.</returns>
        public object CreateValue(string source)
        {
            return source.InjectValue((IDictionary) _values);
        }
    }
}
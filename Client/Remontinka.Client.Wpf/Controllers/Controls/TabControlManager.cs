using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Model;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Менеджер управления перемещением между контролами.
    /// </summary>
    public class TabControlManager
    {
        /// <summary>
        /// Пункт контроля фокусом.
        /// </summary>
        private class FocusItem
        {
            /// <summary>
            /// Задает или получает контрол для фокуса.
            /// </summary>
            public IFocusable Focusable { get; set; }

            /// <summary>
            /// Задает или получает шаг для контрола.
            /// </summary>
            public int TabStep { get; set; }
        }

        /// <summary>
        /// Коллекция пунктов.
        /// </summary>
        private readonly Collection<FocusItem> _items = new Collection<FocusItem>();

        /// <summary>
        /// Текущий индекс для перемещения по контролам.
        /// </summary>
        private int _currentTabIndex = -1;

        /// <summary>
        /// Добавление контрола для перемещения.
        /// </summary>
        /// <param name="focusable">Контрол для перемещения.</param>
        /// <param name="tabStep">Порядковый номер перемещения. </param>
        public void Add(IFocusable focusable, int tabStep)
        {
            if (tabStep != ModelConstants.NotTabNumber)
            {
                _items.Add(new FocusItem { Focusable = focusable, TabStep = tabStep });
            } //if
        }

        /// <summary>
        /// Удаление контрола для перемещения.
        /// </summary>
        /// <param name="focusable">Контрол для удаления.</param>
        public void Remove(IFocusable focusable)
        {
            var item =
                _items.FirstOrDefault(i => ReferenceEquals(i.Focusable, focusable));

            if (item != null)
            {
                _items.Remove(item);
            } //if
        }

        /// <summary>
        /// Проводит фокус на первый пункт.
        /// </summary>
        public void SelectFirts()
        {
            _currentTabIndex = -1;
            var item = _items.OrderBy(i => i.TabStep).FirstOrDefault();

            if (item != null)
            {
                _currentTabIndex = item.TabStep;
                item.Focusable.SetFocus();
            } //if
        }

        /// <summary>
        /// Проводит фокус на последний пункт.
        /// </summary>
        public void SelectLast()
        {
            var item = _items.OrderByDescending(i => i.TabStep).FirstOrDefault();

            if (item != null)
            {
                _currentTabIndex = item.TabStep;
                item.Focusable.SetFocus();
            } //if
            else
            {
                _currentTabIndex = -1;
            } //else
        }

        /// <summary>
        /// Обновляет индекс текущего контрола.
        /// </summary>
        private void UpdateCurrentTabStepIndex()
        {
            var focusable = _items.FirstOrDefault(i => i.Focusable.HasFocus());

            if (focusable != null)
            {
                _currentTabIndex = focusable.TabStep;
            } //if
        }

        /// <summary>
        /// Перемещает фокус далее.
        /// </summary>
        public void Next()
        {
            UpdateCurrentTabStepIndex();
            var item = _items.Where(i => i.TabStep > _currentTabIndex).OrderBy(i => i.TabStep).FirstOrDefault();
            if (item == null)
            {
                SelectFirts();
            } //if
            else
            {
                _currentTabIndex = item.TabStep;
                item.Focusable.SetFocus();
            } //else
        }

        /// <summary>
        /// Перемещает фокус назад.
        /// </summary>
        public void Back()
        {
            UpdateCurrentTabStepIndex();
            var item = _items.Where(i => i.TabStep < _currentTabIndex).OrderByDescending(i => i.TabStep).FirstOrDefault();
            if (item == null)
            {
                SelectLast();
            } //if
            else
            {
                _currentTabIndex = item.TabStep;
                item.Focusable.SetFocus();
            } //else
        }
    }
}

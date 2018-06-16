using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Контроллер управления и создания пагинатора.
    /// </summary>
    public class PaginatorController
    {
        /// <summary>
        /// Содержит представление.
        /// </summary>
        private StackPanel _view;

        /// <summary>
        /// Содержит максимальное количество страниц для навигации.
        /// </summary>
        private int _maxPages;

        /// <summary>
        /// Содержит максимальное количество элементов на странице
        /// </summary>
        private int _maxItemsPerPage;

        /// <summary>
        /// Содержит количество страниц для навигации по умолчанию
        /// </summary>
        private const int MaxPagesDefault = 10;

        /// <summary>
        /// Содержит максимальное количество элементов на странице по умолчанию
        /// </summary>
        private const int MaxItemsPerPage = 20;

        /// <summary>
        /// Установка представления контролу.
        /// </summary>
        /// <param name="view">Представление куда будет добавляться номера страниц для навигации.</param>
        public void SetView(StackPanel view)
        {
            SetView(view, MaxPagesDefault, MaxItemsPerPage);
        }

        /// <summary>
        /// Установка представления контролу.
        /// </summary>
        /// <param name="view">Представление куда будет добавляться номера страниц для навигации.</param>
        /// <param name="maxPages">Максимальное количество страниц.</param>
        /// <param name="maxItemsPerPage">Максимальное количество элементов на странице.</param>
        public void SetView(StackPanel view, int maxPages,int maxItemsPerPage)
        {
            if (_view!=null)
            {
                throw new Exception("Представление уже привязано");
            }

            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            _view = view;

            _view.Orientation = Orientation.Horizontal;
            _maxPages = maxPages;
            _maxItemsPerPage = maxItemsPerPage;
        }

        /// <summary>
        /// Происходит во время того, как пользователь хочет сменить страницу.
        /// </summary>
        public event EventHandler<PageChangedEventArgs> PageChanged;

        /// <summary>
        /// Вызывает событие смены страницы.
        /// </summary>
        /// <param name="page">Страница на которую необходимо перейти.</param>
        private void RisePageChanged(int page)
        {
            EventHandler<PageChangedEventArgs> handler = PageChanged;
            if (handler != null)
            {
                handler(this, new PageChangedEventArgs(page));
            }
        }

        /// <summary>
        /// Установка текущей страницы.
        /// </summary>
        /// <param name="total">Итого элементов.</param>
        /// <param name="current">Текущая страница.</param>
        public void SetPages(int total, int current)
        {
            _view.Children.Clear();

            int countPages = total / _maxItemsPerPage;

            if (total % _maxItemsPerPage > 0)
            {
                countPages++;
            }
            
            int page;

            if (countPages > _maxPages)
            {
                var middle = _maxPages / 2;

                var startPage = current - middle;

                if (startPage < 1)
                {
                    startPage = 1;
                }

                var endPage = current + middle;

                if ((endPage - startPage) < _maxPages)
                {
                    endPage += _maxPages - endPage + startPage;
                }

                if (endPage > countPages)
                {
                    endPage = countPages;
                }

                if ((endPage - startPage) < _maxPages)
                {
                    startPage -= _maxPages - endPage + startPage;
                }


                if (startPage > 1)
                {
                    _view.Children.Add(CreatePageLinkText(1, current));
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    _view.Children.Add(CreatePageLinkText(i, current));
                }

                if (endPage < countPages)
                {
                    _view.Children.Add(CreatePageLinkText(countPages, current));
                }

            }
            else
            {
                for (int i = 0; i < countPages; i++)
                {
                    page = i + 1;
                    _view.Children.Add(CreatePageLinkText(page, current));
                }
            }
        }

        /*private const string ButtonXaml = "<Button Margin=\"5\" Content=\"Test\" Cursor=\"Hand\">"+
    "<Button.Template>"+
        "<ControlTemplate TargetType=\"Button\">"+
            "<TextBlock TextDecorations=\"Underline\">"+
                "<ContentPresenter />"+
            "</TextBlock>"+
        "</ControlTemplate>"+
    "</Button.Template>"+
    "<Button.Style>"+
        "<Style TargetType=\"Button\">"+
            "<Setter Property=\"Foreground\" Value=\"Blue\" />"+
            "<Style.Triggers>"
                "<Trigger Property=\"IsMouseOver\" Value=\"true\">"
                    "<Setter Property="Foreground" Value="Red" />"
                </Trigger>
            </Style.Triggers>
        </Style>
    </Button.Style>
</Button>";
        */
        private Control CreatePageLinkText(int page, int currentPage)
        {
            if (page == currentPage)
            {
                return new Label {Content = WpfUtils.IntToString(page)};
            }

            
            var button = new Button();
            button.Cursor= Cursors.Hand;
            button.Margin = new Thickness(5);
            ControlTemplate template = new ControlTemplate(typeof(Button));
            var textBlock = new FrameworkElementFactory(typeof(TextBlock ));

            textBlock.SetValue(TextBlock.TextDecorationsProperty,TextDecorations.Underline);
            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            textBlock.AppendChild(contentPresenter);

            template.VisualTree = textBlock;


            var style = new Style(typeof (Button));
            
            Setter setter = new Setter();
            setter.Property = Control.ForegroundProperty;
            setter.Value = Brushes.Blue;
            style.Setters.Add(setter);


            var trigger = new Trigger();
            trigger.Property = UIElement.IsMouseOverProperty;
            trigger.Value = true;

            setter = new Setter();
            setter.Property = Control.ForegroundProperty;
            setter.Value = Brushes.Red;
            trigger.Setters.Add(setter);

            style.Triggers.Add(trigger);

            button.Style = style;
            button.Content = WpfUtils.IntToString(page);
            button.Tag = page;
            button.Click+=OnPageButtonClick;
            return button;
        }

        /// <summary>
        /// Вызывается когда пользователь нажал на кнопку смены страницы.
        /// </summary>
        private void OnPageButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var button = sender as Button;
            if (button!=null)
            {
                RisePageChanged((int)button.Tag);
            }
        }
    }
}

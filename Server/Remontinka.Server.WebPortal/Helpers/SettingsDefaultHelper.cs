using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для базовых настроек элементов управления.
    /// </summary>
    public static class SettingsDefaultHelper
    {
        /// <summary>
        /// Устанавливает настройки по умолчанию для лайаута.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings<T>(this FormLayoutSettings<T> settings, SecurityToken token)
        {
            settings.SettingsAdaptivity.AdaptivityMode =
                                    FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 800;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора текст бокса.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this TextBoxSettings settings, SecurityToken token)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора Memo.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this MemoSettings settings, SecurityToken token)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора токен бокса.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this TokenBoxSettings settings, SecurityToken token)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора комбабокса.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this ComboBoxSettings settings, SecurityToken token)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
            settings.Properties.ClearButton.DisplayMode = ClearButtonDisplayMode.OnHover;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора даты.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        /// <param name="useTime">Параметр использования времени.</param>
        public static void SetDefaultSettings(this DateEditSettings settings, SecurityToken token,bool useTime=false)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;

            SetDefaultProperties(settings.Properties, token, useTime);
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора даты.
        /// </summary>
        /// <param name="properties">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        /// <param name="useTime">Параметр использования времени.</param>
        public static void SetDefaultProperties(this DateEditProperties properties, SecurityToken token, bool useTime = false)
        {
            properties.UseMaskBehavior = true;
            properties.EditFormat = EditFormat.Custom;
            if (!useTime)
            {
                properties.EditFormatString = "dd.MM.yyyy";
            }
            else
            {
                properties.EditFormatString = "dd.MM.yyyy HH:mm tt";
                properties.TimeSectionProperties.Visible = true;
                properties.TimeSectionProperties.TimeEditProperties.EditFormat = EditFormat.Custom;
                properties.TimeSectionProperties.TimeEditProperties.EditFormatString = "HH:mm tt";
            }
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора чисел.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        /// <param name="isDecimal">Признак числа с плавающей точкой.</param>
        public static void SetDefaultSettings(this SpinEditSettings settings, SecurityToken token, bool isDecimal = false)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;

            settings.Properties.SpinButtons.ShowLargeIncrementButtons = false;
            settings.Properties.SpinButtons.ShowIncrementButtons = false;
            if (isDecimal)
            {
                settings.Properties.NumberType = SpinEditNumberType.Float;
                settings.Properties.DisplayFormatString = "f2";
            }
            
            settings.Properties.ClearButton.DisplayMode = ClearButtonDisplayMode.OnHover;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора чек бокса.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this CheckBoxSettings settings, SecurityToken token)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для редактора текст бокса.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this CheckBoxListSettings settings, SecurityToken token)
        {
            settings.Width = Unit.Percentage(100);
            settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            settings.ShowModelErrors = true;
            settings.Properties.RepeatLayout = RepeatLayout.Table;
            settings.Properties.RepeatLayout = RepeatLayout.Table;
            settings.Properties.RepeatDirection = RepeatDirection.Horizontal;
        }

        /// <summary>
        /// Устанавливает настройки по-умолчанию для popup контролов.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetDefaultSettings(this PopupControlSettings settings, SecurityToken token)
        {
            settings.ShowOnPageLoad = false;
            settings.AllowDragging = true;
            settings.CloseAction = CloseAction.CloseButton;
            settings.CloseOnEscape = true;
            settings.PopupAnimationType = AnimationType.None;
        }

        
    }
}
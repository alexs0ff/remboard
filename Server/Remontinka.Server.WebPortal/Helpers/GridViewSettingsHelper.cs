using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.ASPxThemes;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using DevExpress.XtraPrinting.Native;
using Remontinka.Server.WebPortal.Controllers;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Models.RepairOrderGridForm;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для настроект грида по модели.
    /// </summary>
    public static class GridViewSettingsHelper
    {
        /// <summary>
        /// Устанавливает настройки по-умолчанию для гридов.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безопасности.</param>
        public static void SetDefaultSettings(this GridViewSettings settings, SecurityToken token)
        {
            settings.Settings.ShowFilterRow = true;
            settings.Settings.ShowGroupPanel = true;
            settings.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            settings.SettingsSearchPanel.Visible = true;

            settings.Width = Unit.Percentage(100);
        }

        /// <summary>
        /// Устанавливает настройки согласно модели данных.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="html">Текущий html хелпер.</param>
        /// <param name="model">Модель данных.</param>
        public static void Apply<TGridModel>(this GridViewSettings settings, GridModelBase model, HtmlHelper<TGridModel> html)
            where TGridModel: GridModelBase
        {
            //!Филитры под датам отсюда https://demos.devexpress.com/MVCxGridViewDemos/Filtering/DateRangeHeaderFilter
            //здесь связь с другими контролами https://demos.devexpress.com/MVCxNavigationAndLayoutDemos/Panel/ResponsiveLayout
            html.EnableClientValidation();
            settings.Name = model.GridName;
            settings.KeyFieldName = model.KeyFieldName;
            settings.CallbackRouteValues = new { Controller = model.ControllerName, Action = "GetGrid",parentID = model.ParentId };
            //settings.ClientLayout += (sender, args) =>{};//TODO save layout

            settings.SettingsEditing.AddNewRowRouteValues = new { Controller = model.ControllerName, Action = "EditFormTemplateAddNewPartial", parentID = model.ParentId };
            settings.SettingsEditing.UpdateRowRouteValues = new { Controller = model.ControllerName, Action = "EditFormTemplateUpdatePartial", parentID = model.ParentId };
            settings.SettingsEditing.DeleteRowRouteValues =
                new {Controller = model.ControllerName, Action = "EditFormTemplateDeletePartial", entityId = "value", parentID = model.ParentId };


            settings.CommandColumn.ButtonRenderMode = GridCommandButtonRenderMode.Link;//Пока оставить ссылкой, т.к. не все кнопки имеют картинку
            settings.SettingsCommandButton.EditButton.Image.IconID = IconID.EditEdit16x16;
            settings.SettingsCommandButton.NewButton.Image.IconID = IconID.ActionsNew16x16;
            settings.SettingsCommandButton.DeleteButton.Image.IconID = IconID.ActionsDelete16x16devav;

            settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;
            settings.SettingsBehavior.ConfirmDelete = true;
            settings.Width = Unit.Percentage(100);
            settings.EnableRowsCache = false;

            settings.SettingsAdaptivity.AdaptivityMode = GridViewAdaptivityMode.HideDataCells;
            settings.SettingsAdaptivity.AllowOnlyOneAdaptiveDetailExpanded = true;
            settings.EditFormLayoutProperties.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.EditFormLayoutProperties.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            settings.CommandColumn.Visible = true;
            settings.CommandColumn.ShowNewButtonInHeader = true;
            settings.CommandColumn.ShowDeleteButton = true;
            settings.CommandColumn.ShowEditButton = true;

            settings.SettingsPager.Position = PagerPosition.TopAndBottom;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.ClientLayout = (sender, args) =>
            {
                if (args.LayoutMode == ClientLayoutMode.Saving)
                {
                    RemontinkaServer.Instance.EntitiesFacade.SaveUserGridState(model.Token,model.GridName,args.LayoutData);
                }
                else if (args.LayoutMode == ClientLayoutMode.Loading)
                {
                    args.LayoutData = RemontinkaServer.Instance.EntitiesFacade.GetGridUserState(model.Token, model.GridName);
                }
            };

            settings.SetEditFormTemplateContent(container =>
                {
                    var formId = "editForm" + model.ControllerName;

                    var controller = html.ViewContext.Controller as ICrudController;
                    object objectToBind = container.DataItem;
                    object gridEditModel = null;
                    if (container.Grid.IsNewRowEditing)
                    {
                         objectToBind= controller.CreateNewModel(html.ViewData[model.GridObjectName], model);
                    }
                    else if (container.Grid.IsEditing)
                    {
                        gridEditModel= controller.PrepareUpdateEditModel(
                                                html.ViewData[model.GridObjectName] ?? container.DataItem, model);
                        objectToBind = gridEditModel;
                    }

                    using (html.BeginForm("", "",FormMethod.Get, new { id = formId }))
                    {
                        html.DevExpress().FormLayout(formLayoutSettings =>
                            {
                                formLayoutSettings.Name = "FormLayout" + model.ControllerName;
                                
                                formLayoutSettings.SetDefaultSettings(model.Token);

                                    if (container.Grid.IsNewRowEditing)
                                    {
                                        model.IsEditing = false;
                                        html.ViewContext.Writer.Write(html.Partial(controller.GetCreateViewName(),
                                            controller.CreateNewEditSettingsModel(formLayoutSettings,
                                                objectToBind, model,html)));
                                    }
                                    else if (container.Grid.IsEditing)
                                    {
                                        model.IsEditing = true;
                                        
                                        formLayoutSettings.Items.Add(item =>
                                        {
                                            item.Name = model.KeyFieldName;
                                            item.ClientVisible = false;
                                            item.SetNestedContent(()=> html.ViewContext.Writer.Write(html.Hidden(model.KeyFieldName).ToHtmlString()));
                                        });

                                        html.ViewContext.Writer.Write(html.Partial(controller.GetUpdateViewName(),
                                            controller.CreateUpdateEditSettingsModel(formLayoutSettings, gridEditModel, model, html)));

                                        html.ViewData[model.GridObjectName] = gridEditModel;
                                    }
                               

                                formLayoutSettings.Items.AddEmptyItem();
                                formLayoutSettings.Items.Add(i =>
                                {
                                    i.ShowCaption = DefaultBoolean.False;
                                }).SetNestedContent(() =>
                                {
                                    html.ViewContext.Writer.Write("<div style='float:right'>");
                                    html.DevExpress().Button(
                                        btnSettings =>
                                        {
                                            btnSettings.Name = "btnUpdate" + model.ControllerName;
                                            btnSettings.Text = "Сохранить";
                                            btnSettings.ControlStyle.CssClass = "button";
                                            btnSettings.ClientSideEvents.Click = string.Format("function(s, e){{ {0}.UpdateEdit(); }}",  model.GridName);
                                        }).Render();
                                    html.DevExpress().Button(
                                        btnSettings =>
                                        {
                                            btnSettings.Name = "btnCancel" + model.ControllerName;
                                            btnSettings.Text = "Отменить";
                                            btnSettings.ControlStyle.CssClass = "button";
                                            btnSettings.Style[HtmlTextWriterStyle.MarginLeft] = "5px";
                                            btnSettings.ClientSideEvents.Click =
                                                string.Format("function(s, e){{ {0}.CancelEdit(); }}", model.GridName);
                                        }).Render();
                                    html.ViewContext.Writer.Write("</div>");
                                });
                            })
                            .Bind(objectToBind)
                            .Render();
                    }
                });
            

        }

        /// <summary>
        /// Создает кнопку сохранения текущего фильтра для грида.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="html">Текущий html хелпер.</param>
        /// <param name="model">Модель данных.</param>
        public static void CreateSaveCurrentFilterButton<TGridModel>(this GridViewSettings settings, GridModelBase model,
            HtmlHelper<TGridModel> html)
            where TGridModel : GridModelBase
        {
            var button = new GridViewCommandColumnCustomButton();

            button.ID = model.GridName + "ShowSaveCurrentFilter";
            button.Text = "Сохранить фильтр";
            button.Visibility = GridViewCustomButtonVisibility.FilterRow;
            button.Image.IconID = IconID.SaveSave16x16;
            settings.CommandColumn.CustomButtons.Add(button);
            
            var popupId = model.GridName + "FilterNamePopup";
            settings.ClientSideEvents.CustomButtonClick = string.Format("function(s, e){{ {0}.Show(); }}", popupId);

            settings.CustomJSProperties += (s, e) =>
            {
                e.Properties["cpFilterExpression"] = ((ASPxGridView)s).FilterExpression;
            };
        }

        /// <summary>
        /// Привязывает грид к модели данных и создает html.
        /// </summary>
        /// <param name="extension">Грид.</param>
        /// <param name="model">Созданная модель данных</param>
        public static MvcHtmlString BindToModel<TGridModel>(this GridViewExtension extension, GridModelBase model, HtmlHelper<TGridModel> html)
            where TGridModel : GridModelBase
        {
            if (html.ViewData["EditError"] != null)
            {
                extension.SetEditErrorText((string) html.ViewData["EditError"]);
            }
            return extension.BindToLINQ(string.Empty, string.Empty, (s, e) => {
                e.QueryableSource = model.Model;
                e.KeyExpression = model.KeyFieldName;
            }).GetHtml();
        }
    }
}
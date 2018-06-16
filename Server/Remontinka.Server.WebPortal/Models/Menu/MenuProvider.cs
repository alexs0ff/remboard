using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.Menu
{
    /// <summary>
    /// Управления пунктами главного меню.
    /// </summary>
    public static class MenuProvider
    {
        private static List<MainMenuItem> _menuItems = new List<MainMenuItem>
        {
            new MainMenuItem
            {
                Title = "Заказы",
                Controller = "RepairOrder",
                Action = "Index",
                Roles = UserRole.GetRolesArray(UserRole.Admin,UserRole.Engineer,UserRole.Manager)
            },
            new MainMenuItem
            {
                Title = "Управление",
                Roles = UserRole.GetRolesArray(UserRole.Admin,UserRole.Engineer,UserRole.Manager),
                SubItems = new List<MainMenuSubItem>
                {
                    new MainMenuSubItem
                    {
                        Title = "Редактор документов",
                        Controller = "CustomizeReport",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Пользователи",
                        Controller = "User",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Филиалы",
                        Controller = "Branch",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Статусы",
                        Controller = "OrderStatus",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Типы заказов",
                        Controller = "OrderKind",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                     new MainMenuSubItem
                    {
                         BeginGroup = true,
                        Title = "Автодополнение",
                        Controller = "AutocompleteItem",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Система",
                        Controller = "System",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Интерфейс",
                        Controller = "System",
                        Action = "InterfaceSettings",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin,UserRole.Engineer,UserRole.Manager),
                    }
                    ,
                    new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Сменить свой пароль",
                        Controller = "Account",
                        Action = "ChangePassword",
                        Roles =  UserRole.GetRolesArray(UserRole.Admin,UserRole.Engineer,UserRole.Manager),
                    },
                }
            },
          
             new MainMenuItem
            {
                Title = "Финансы",
                Roles = UserRole.GetRolesArray(UserRole.Admin),
                SubItems = new List<MainMenuSubItem>
                {
                     new MainMenuSubItem
                    {
                        Title = "Приход/расход",
                        Controller = "FinancialItemValue",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Финансовые группы",
                        Controller = "FinancialGroupItem",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Статьи бюджета",
                        Controller = "FinancialItem",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                   
                    new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Вознаграждение",
                        Controller = "UserInterest",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                }
            },
            new MainMenuItem
            {
                Title = "Складской учет",
                Roles = UserRole.GetRolesArray(UserRole.Admin),
                SubItems = new List<MainMenuSubItem>
                {
                     new MainMenuSubItem
                    {
                        Title = "Складские остатки",
                        Controller = "WarehouseItem",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },

                    new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Склады",
                        Controller = "Warehouse",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Категории товаров",
                        Controller = "ItemCategory",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                     new MainMenuSubItem
                    {
                        Title = "Номенклатура товара",
                        Controller = "GoodsItem",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                      new MainMenuSubItem
                    {
                        Title = "Контрагенты",
                        Controller = "Contractor",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Приходные накладные",
                        Controller = "IncomingDoc",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Списания",
                        Controller = "CancellationDoc",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                     new MainMenuSubItem
                    {
                        Title = "Перемещения",
                        Controller = "TransferDoc",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                }
            },
            new MainMenuItem
            {
                Title = "Общие отчеты",
                Roles = UserRole.GetRolesArray(UserRole.Admin),
                SubItems = new List<MainMenuSubItem>
                {
                     new MainMenuSubItem
                    {
                        Title = "Работа исполнителей",
                        Controller = "EngineerWorkReport",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                     new MainMenuSubItem
                    {
                        Title = "Использованные запчасти",
                        Controller = "UsedDeviceItemsReport",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                      new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Доходы и расходы",
                        Controller = "RevenueAndExpenditureReport",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                     new MainMenuSubItem
                    {
                        Title = "Вознаграждение пользователей",
                        Controller = "UserInterestReport",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                      new MainMenuSubItem
                    {
                        BeginGroup = true,
                        Title = "Приход и расход на складе",
                        Controller = "WarehouseFlowReport",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                }
            },

            new MainMenuItem
            {
                Title = "Offline клиент",
                Roles = UserRole.GetRolesArray(UserRole.Admin),
                SubItems = new List<MainMenuSubItem>
                {
                    new MainMenuSubItem
                    {
                        Title = "Запросы регистрации",
                        Controller = "UserPublicKeyRequest",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                    new MainMenuSubItem
                    {
                        Title = "Ключи клиентов",
                        Controller = "UserPublicKey",
                        Roles = UserRole.GetRolesArray(UserRole.Admin),
                    },
                }
            },
        };

        /// <summary>
        /// Получает инициализорованный список меню в соответствии с правами текущего пользователя.
        /// </summary>
        /// <param name="token">Токен пользователя.</param>
        /// <returns>Пункты меню.</returns>
        public static List<MainMenuItem> GetMenu(SecurityToken token)
        {
            var result = new List<MainMenuItem>();
            var roles = RemontinkaServer.Instance.SecurityService.GetRolesForUser(token.LoginName);

            foreach (var mainMenuItem in _menuItems)
            {
                if (mainMenuItem.Roles.Any(r => roles.Contains(r, StringComparer.OrdinalIgnoreCase)))
                {
                    var copiedItem = mainMenuItem.CreateCopy();
                    result.Add(copiedItem);
                    if (mainMenuItem.SubItems != null)
                    {
                        foreach (var mainMenuSubItem in mainMenuItem.SubItems)
                        {
                            if (mainMenuSubItem.Roles.Any(r => roles.Contains(r, StringComparer.OrdinalIgnoreCase)))
                            {
                                if (copiedItem.SubItems == null)
                                {
                                    copiedItem.SubItems = new List<MainMenuSubItem>();
                                }
                                copiedItem.SubItems.Add(mainMenuSubItem.CreateCopy());
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
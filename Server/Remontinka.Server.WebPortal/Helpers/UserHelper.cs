using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Утилитные методы работы с пользователями.
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// Возваращает список пользователей для определенной роли.
        /// </summary>
        /// <param name="securityToken">Токен пользователя.</param>
        /// <param name="projectRole">Роль.</param>
        /// <returns></returns>
        public static List<SelectListItem<Guid>> GetUserList(SecurityToken securityToken, byte? projectRole)
        {
            List<SelectListItem<Guid>> list = new List<SelectListItem<Guid>>();
            PopulateUserList(list, null, securityToken, projectRole);
            return list;
        }
        /// <summary>
        /// Заполняет список пользователей для комбобокса.
        /// </summary>
        public static void PopulateUserList(List<SelectListItem<Guid>> list, Guid? selectedId, SecurityToken securityToken, byte? projectRole)
        {
            if (securityToken.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                var userList = RemontinkaServer.Instance.EntitiesFacade.GetUsers(securityToken, projectRole).ToList();

                //у администратора, если нет пользователей определенного вида и в фирме нет пользователей, добавляем их в выборку

                if (!userList.Any())
                {
                    userList =
                        RemontinkaServer.Instance.EntitiesFacade.GetUsers(securityToken, ProjectRoleSet.Admin.ProjectRoleID).ToList();
                } //if

                foreach (var user in userList)
                {
                    list.Add(new SelectListItem<Guid>
                    {
                        Selected = user.UserID == selectedId,
                        Text =
                            string.Format("{0} {1} {2}", user.LastName, user.FirstName,
                                          user.MiddleName),
                        Value = user.UserID
                    });
                } //foreach
            }
            else
            {
                foreach (var userBranchMapItemDto in RemontinkaServer.Instance.DataStore.GetUserBranchMapByItemsByUser(securityToken.User.UserID))
                {
                    foreach (var entity in RemontinkaServer.Instance.DataStore.GetUserBranchMapByItemsByBranch(userBranchMapItemDto.BranchID, projectRole))
                    {
                        list.Add(new SelectListItem<Guid>
                        {
                            Selected = entity.UserID == selectedId,
                            Text = FormatUserTitle(entity.FirstName, entity.LastName, entity.MiddleName),
                            Value = entity.UserID
                        });
                    }
                }

            }

            if (list.Count == 0)
            {
                var userList = RemontinkaServer.Instance.EntitiesFacade.GetUsers(securityToken, ProjectRoleSet.Admin.ProjectRoleID).ToList();
                foreach (var selectedUser in userList)
                {
                    list.Add(new SelectListItem<Guid>
                    {

                        Text = FormatUserTitle(selectedUser.LastName, selectedUser.FirstName, selectedUser.MiddleName),
                        Value = selectedUser.UserID
                    });
                }
            }

            ////TODO позволяем выбрать админа, если он раньше был.
            //if (selectedId != null && list.All(i => !i.Selected))
            //{
            //    var selectedUser = RemontinkaServer.Instance.DataStore.GetUser(selectedId,
            //                                                                   securityToken.User.UserDomainID);
            //    if (selectedUser != null && selectedUser.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            //    {
            //        list.Add(new SelectListItem<Guid>
            //        {
            //            Selected = true,
            //            Text = FormatUserTitle(selectedUser.FirstName, selectedUser.LastName, selectedUser.MiddleName),
            //            Value = selectedUser.UserID
            //        });
            //    } //if
            //} //if
        }

        /// <summary>
        /// Форматирование имени пользователя.
        /// </summary>
        /// <returns>Сформированная строка.</returns>
        private static string FormatUserTitle(string firstName, string lastName, string middleName)
        {
            return string.Format("{0} {1} {2}",lastName,firstName, middleName);
        }
    }
}
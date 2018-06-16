using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Утилитные методы работы с пользователями.
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// Заполняет список пользователей для комбобокса.
        /// </summary>
        public static void PopulateUserList(List<JSelectListItem<Guid>> list, Guid? selectedId, SecurityToken securityToken, byte? projectRole)
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
                    list.Add(new JSelectListItem<Guid>
                    {
                        Selected = user.UserID == selectedId,
                        Text =
                            string.Format("{0} {1} {2}", user.FirstName, user.LastName,
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
                        list.Add(new JSelectListItem<Guid>
                        {
                            Selected = entity.UserID == selectedId,
                            Text = FormatUserTitle(entity.FirstName,entity.LastName,entity.MiddleName),
                            Value = entity.UserID
                        });
                    }
                }
                
            }

            //TODO позволяем выбрать админа, если он раньше был.
            if (selectedId!=null && list.All(i=>!i.Selected))
            {
                var selectedUser = RemontinkaServer.Instance.DataStore.GetUser(selectedId,
                                                                               securityToken.User.UserDomainID);
                if (selectedUser!=null && selectedUser.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
                {
                    list.Add(new JSelectListItem<Guid>
                             {
                                 Selected = true,
                                 Text = FormatUserTitle(selectedUser.FirstName,selectedUser.LastName,selectedUser.MiddleName),
                                 Value = selectedUser.UserID
                             });
                } //if
            } //if
        }

        /// <summary>
        /// Форматирование имени пользователя.
        /// </summary>
        /// <returns>Сформированная строка.</returns>
        private static string FormatUserTitle(string firstName,string lastName,string middleName)
        {
            return string.Format("{0} {1} {2}", firstName, lastName, middleName);
        }
    }
}
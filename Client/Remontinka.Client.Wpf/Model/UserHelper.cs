using System;
using System.Collections.Generic;
using System.Linq;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Controllers.Items;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Утилитные методы работы с пользователями.
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// Заполняет список пользователей для комбобокса.
        /// </summary>
        public static void PopulateUserList(List<SelectListItem<Guid>> list, Guid? selectedId, SecurityToken securityToken, long? projectRole)
        {
            if (securityToken.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                var userList = ClientCore.Instance.DataStore.GetUsers(projectRole).ToList();

                //у администратора, если нет пользователей определенного вида и в фирме нет пользователей, добавляем их в выборку

                if (!userList.Any())
                {
                    userList =
                        ClientCore.Instance.DataStore.GetUsers((byte?) ProjectRoleSet.Admin.ProjectRoleID).ToList();
                } //if

                foreach (var user in userList)
                {
                    list.Add(new SelectListItem<Guid>
                    {
                        Selected = user.UserIDGuid == selectedId,
                        Text =
                            string.Format("{0} {1} {2}", user.FirstName, user.LastName,
                                          user.MiddleName),
                        Value = user.UserIDGuid
                    });
                } //foreach
            }
            else
            {
                foreach (var userBranchMapItemDto in ClientCore.Instance.DataStore.GetUserBranchMapByItemsByUser(securityToken.UserID))
                {
                    foreach (var entity in ClientCore.Instance.DataStore.GetUserBranchMapByItemsByBranch(userBranchMapItemDto.BranchIDGuid, projectRole))
                    {
                        list.Add(new SelectListItem<Guid>
                        {
                            Selected = entity.UserIDGuid == selectedId,
                            Text = FormatUserTitle(entity.FirstName,entity.LastName,entity.MiddleName),
                            Value = entity.UserIDGuid
                        });
                    }
                }
                
            }

            //TODO позволяем выбрать админа, если список пустой.
            if (selectedId!=null && list.All(i=>!i.Selected))
            {
                var selectedUser = ClientCore.Instance.DataStore.GetUser(selectedId);

                if (selectedUser!=null && selectedUser.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
                {
                    list.Add(new SelectListItem<Guid>
                             {
                                 Selected = true,
                                 Text = FormatUserTitle(selectedUser.FirstName,selectedUser.LastName,selectedUser.MiddleName),
                                 Value = selectedUser.UserIDGuid
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
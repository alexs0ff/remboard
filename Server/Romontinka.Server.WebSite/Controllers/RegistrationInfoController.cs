using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.SystemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления информацией регистрации.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class RegistrationInfoController : JCrudControllerBase<Guid, RegistrationInfoEditModel, RegistrationInfoEditModel, RegistrationInfoItem, RegistrationInfoItem>
    {
        /// <summary>
        /// Содержит наимнование контроллера.
        /// </summary>
        public const string ControllerName = "RegistrationInfo";

        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="id">Код сущности для удаления.</param>
        protected override void DeleteEntity(SecurityToken token, Guid? id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализирует модель для создания формы новой сущности.
        /// </summary>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Инициализированная модель.</returns>
        protected override RegistrationInfoEditModel InitCreateModel(SecurityToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализирует модель для редактирования формы существующей сущности.
        /// </summary>
        ///<param name="token">Токен безопасности.</param>
        ///<param name="id">Код сущности.</param>
        ///<returns>Инициализированная модель редактирования.</returns>
        protected override RegistrationInfoEditModel InitEditModel(SecurityToken token, Guid? id)
        {
            var domain = RemontinkaServer.Instance.EntitiesFacade.GetUserDomain(token);

            return new RegistrationInfoEditModel
                   {
                       Address = domain.Address,
                       Email = domain.RegistredEmail,
                       LegalName = domain.LegalName,
                       Trademark = domain.Trademark
                   };
        }

        /// <summary>
        /// Сохраняет модель при сохранении новой сущности.
        /// </summary>
        ///<param name="model">Модель для сохранения сущности.</param>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Сохраненная сущность.</returns>
        protected override RegistrationInfoItem SaveCreateModel(RegistrationInfoEditModel model, SecurityToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет модель при редактировании существующей сущности.
        /// </summary>
        ///<param name="model">Модель для сохранения редактируемой сущности.</param>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Сохраненная сущность.</returns>
        protected override RegistrationInfoItem SaveEditModel(RegistrationInfoEditModel model, SecurityToken token)
        {
            var userDomain = new UserDomain
                             {
                                 Address = model.Address,
                                 LegalName = model.LegalName,
                                 Trademark = model.Trademark,
                                 RegistredEmail = model.Email
                             };

            RemontinkaServer.Instance.EntitiesFacade.UpdateUserDomain(token, userDomain);

            return new RegistrationInfoItem
                   {
                       Address = model.Address,
                       Email = model.Email,
                       LegalName = model.LegalName,
                       Trademark = model.Trademark
                   };
        }
    }
}
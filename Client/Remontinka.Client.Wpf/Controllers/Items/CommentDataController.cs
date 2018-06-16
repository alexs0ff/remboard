using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Controllers.Forms;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.Model.Items;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер добавления комментариев.
    /// </summary>
    public class CommentDataController : ModelEditControllerBase<CommentCreateView, CommentCreateView, CommentCreateModel, CommentCreateModel, Guid, Guid?>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parameters">Модель параметров.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override CommentCreateModel CreateEditedModel(SecurityToken token, Guid entityId, Guid? parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="parameters">Модель параметров.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override CommentCreateModel CreateNewModel(SecurityToken token, Guid? parameters)
        {
            return new CommentCreateModel
                   {
                       RepairOrderID = parameters,
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, CommentCreateModel model, SaveModelResult result)
        {
            ClientCore.Instance.OrderTimelineManager.AddNewComment(token, model.RepairOrderID, model.CommentTitle);

        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, CommentCreateModel model, SaveModelResult result)
        {
            throw new NotImplementedException();
        }
    }
}

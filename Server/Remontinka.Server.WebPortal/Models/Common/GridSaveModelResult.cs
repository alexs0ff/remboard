using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Модель результата сохранения сущности грида.
    /// </summary>
    public class GridSaveModelResult
    {
        /// <summary>
        /// Задает или получает признак наличия ошибок в модели.
        /// </summary>
        public bool HasModelError
        {
            get { return _modelErrors != null && _modelErrors.Any(); }
        }

        /// <summary>
        /// Содержит ошибки модели.
        /// </summary>
        private List<PairItem<string, string>> _modelErrors;

        /// <summary>
        /// Получает ошибки модели.
        /// </summary>
        public List<PairItem<string, string>> ModelErrors
        {
            get
            {
                if (_modelErrors == null)
                {
                    _modelErrors = new List<PairItem<string, string>>();
                } //if
                return _modelErrors;
            }
        }
    }
}
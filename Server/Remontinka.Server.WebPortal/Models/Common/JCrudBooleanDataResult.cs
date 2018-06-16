namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Результат обработки документа.
    /// </summary>
    public class JCrudBooleanDataResult : JCrudResult
    {
        /// <summary>
        /// Задавет или получает булевское значение результата.
        /// </summary>
        public bool Data { get; set; }
    }
}
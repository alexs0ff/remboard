using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Core.Context
{
    /// <summary>
    /// Контекст для филиалов.
    /// </summary>
    public class BranchContextItem : ContextItemBase
    {
        public BranchContextItem(Branch branch)
        {
            _values[ContextConstants.BranchTitle] = branch.Title;
            _values[ContextConstants.BranchLegalName] = branch.LegalName;
            _values[ContextConstants.BranchAddress] = branch.Address;
        }
    }
}

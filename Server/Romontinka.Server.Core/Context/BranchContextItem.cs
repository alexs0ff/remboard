using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.Core.Context
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

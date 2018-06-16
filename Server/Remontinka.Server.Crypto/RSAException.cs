using System;
using System.Collections.Generic;
using System.Text;

namespace Remontinka.Server.Crypto
{
    public class RSAException : Exception
    {
        #region Private Fields

        private int error;
        
        #endregion

        #region Constructors

        public RSAException()
        {

        }
        public RSAException(string message)
            : base(message)
        {
            error = 0;
        }

        
        public RSAException(string message, int error)
            : base(message)
        {
            this.error = error;
        }

        #endregion

        #region Public Propeties

        public int Error
        {
            get { return error; }
        }

        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// State object for FTP transfers
    /// </summary>
    public class FtpWebRequestState : WebRequestState
    {
        private FtpWebRequest _request;

        public override WebRequest request
        {
            get { return _request; }
            set { _request = (FtpWebRequest)value; }
        }

        private FtpWebResponse _response;

        public override WebResponse response
        {
            get { return _response; }
            set { _response = (FtpWebResponse)value; }
        }

        public FtpWebRequestState(int buffSize)
            : base(buffSize)
        {
        }
    }
}

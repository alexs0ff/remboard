using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// State object for HTTP transfers
    /// </summary>
    public class HttpWebRequestState : WebRequestState
    {
        private HttpWebRequest _request;

        public override WebRequest request
        {
            get { return _request; }
            set { _request = (HttpWebRequest)value; }
        }

        private HttpWebResponse _response;

        public override WebResponse response
        {
            get { return _response; }
            set { _response = (HttpWebResponse)value; }
        }

        public HttpWebRequestState(int buffSize)
            : base(buffSize)
        {
        }
    }
}

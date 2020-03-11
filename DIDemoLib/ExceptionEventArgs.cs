using System;
using System.Collections.Generic;
using System.Text;

namespace DIDemoLib
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}

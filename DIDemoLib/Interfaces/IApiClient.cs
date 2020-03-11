using System;
using System.Collections.Generic;
using System.Text;

namespace DIDemoLib.Interfaces
{
    public interface IApiClient
    {
        public event EventHandler<ExceptionEventArgs> OnException;

        public void InvokExceptionHandler();
        public void FooMethod();
    }
}

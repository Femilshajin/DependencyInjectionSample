using DIDemoLib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DIDemoLib
{
    public class ApiClient: IApiClient
    {
        public event EventHandler<ExceptionEventArgs> OnException;
        private ILogger _logger;
        public ApiClient(ILogger<ApiClient> logger)
        {
            _logger = logger;
        }

        public void FooMethod()
        {
            _logger.LogInformation("Bad Request");
        }
        public void InvokExceptionHandler()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                HandleException(ex, new Dictionary<string, string>
                {
                    { "DateTime", DateTime.Now.ToString() }
                });
            }
        }

        protected virtual void HandleException(Exception e, Dictionary<string, string> properties)
        {
            if (e != null)
            {
                OnException?.Invoke(this, new ExceptionEventArgs
                {
                    Exception = e,
                    Properties = properties
                });
            }
        }
    }
}

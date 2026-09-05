using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model
{
    public class Result
    {
        #region Proprates 
        public bool Success { get; set; }
        public string Message { get; set; }
        #endregion

        #region Methods
        public  void SuccessResult(string message)
        {
            Success = true;
            Message = message;
        }

        public  void FailureResult(string message)
        {
            Success = false;
            Message = message;
        }
        #endregion
    }
}

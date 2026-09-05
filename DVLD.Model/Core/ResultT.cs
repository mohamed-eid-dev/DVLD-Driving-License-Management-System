using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Model
{
    public class Result<T> : Result
    {
        public T Data { get; set; }

        #region Methods
        // Remove 'override' since base method is not virtual/abstract/override
        public  void SuccessResult(T data,string message)
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public  void FailureResult(T data ,string message)
        {
            Success = false;
            Message = message;
            Data = data;
        }
        #endregion
    }
}

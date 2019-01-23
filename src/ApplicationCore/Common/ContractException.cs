using System;

namespace ApplicationCore.Common
{
    public class ContractException : Exception
    {
        public ContractException()
        {
        }
        public ContractException(string message)
            : base(message)
        {
        }

        public ContractException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

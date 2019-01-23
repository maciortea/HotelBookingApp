namespace ApplicationCore.Common
{
    public static class Contract
    {
        public static void Require(bool precondition, string message = "")
        {
            if (!precondition)
            {
                throw new ContractException(message);
            }
        }
    }
}

using System;

namespace LegacyApp
{
    public class UserCreditService : IDisposable
    {
        public void Dispose()
        {
            //...
        }

        internal int GetCreditLimit(string firstName, string lastName, DateTime dateOfBirth)
        {
            //Fetching the data...
            return 10000;
        }
    }
}
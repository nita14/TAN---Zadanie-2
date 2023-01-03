namespace LegacyApp
{
    public class ClientRepository
    {
        public ClientRepository()
        {
        }

        internal Client GetById(int clientId)
        {
            //Fetching the data...
            return new Client
            {
                ClientId = clientId
            };
        }
    }
}
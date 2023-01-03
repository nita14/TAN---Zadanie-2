using System;

namespace LegacyApp
{
    public class UserService
    {
        //AddUser_ShouldAddUserCorrectly
        //AddUser_ShouldFail_IncorrectEmail

        //SOLID
        //Metoda do dodawania użytkownika. Powinna mieć @doc opis parametrów oraz do zwraca. Jak ponizej:
        /// <summary>
        /// /
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            //sprawdzenie czy imie lub nazwisko jest null. Powinna być wykorzystywania dedykowana klasa do walidacji
            //ze statycznymi metodami.
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                //tu powinno byc logowanie błędu
                return false;
            }

            //sprawdzeine czy parametr email zawiera @ lub . 
            //niestety nie ma sprawdzania czy wartość jest null

            if (!email.Contains("@") && !email.Contains("."))
            {
                //ponownie logowanie
                return false;
            }

            //pozyskanie czasu - tu powinna byc UTC (w innym przypadku moze  byc to data na serwerze) albo inna strefa czasowa
            var now = DateTime.Now;
            //wyliczanie wieku nowego uzytkownika.
            //brak walidacji czy data jest null
            int age = now.Year - dateOfBirth.Year;
            //sprawdzanie wieku z uwzglednienienie miesiaca urodzenia. Jesli tak, zmniejszenie o 1.
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            //chyba niepełnoletnich (wg. USA) nie mozna tworzyć
            if (age < 21)
            {   
                //znów logowanie błędu
                return false;
            }

            //tworzneie repozytorium klientów oraz klienta
            var clientRepository = new ClientRepository();
            //niestety znów brak sprawdzania czy clientid jest null
            var client = clientRepository.GetById(clientId);

            //tworzenie nowego użytkownika
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            //sprawdzenie czy klient jest bardzo ważny, jeśli tak to usuwa limit kredytowe.
            if (client.Name == "VeryImportantClient")
            {
                //Skip credit limit
                user.HasCreditLimit = false;
            }

            //tworzenie kredytu dla waznych klientów
            else if (client.Name == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    //pobranie aktualnego kredytu i zwiększenie go dwukrotnie
                    //brak walidacji danych
                    int creditLimit = userCreditService.GetCreditLimit(user.FirstName, user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            // w przeciwnym wypadku...
            else
            {
                //powtorzenie kodu jak powyzej - powinna byc wyeksapulowana do osobnej metody
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.FirstName, user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            //jesli user ma kredyt ponizej 500 ,nie towrzy nowego usera
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            //dodanie uzytkownia
            UserDataAccess.AddUser(user);
            //zwracanie wyniku dzialania - poprawne dodanie usera
            return true;

            ///Generalnie metoda AddUser powinna być rozbita na kilka mniejszych pomocniczych oraz implementacę interfejsu
        }
    }
}

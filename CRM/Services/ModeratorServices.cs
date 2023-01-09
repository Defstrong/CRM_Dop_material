using CRM.Models;
using CRM.Enums;

namespace CRM.Services
{
    public sealed class ModeratorServices
    {
        private static List<Person> Persons;
        static List<Loan> RequestsLoanUser;
        static List<Massage> Massages;
        public ModeratorServices(List<Person> persons, List<Loan> requestsLoanUser, List<Massage> massages)
        {
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
            Massages = massages;
        }
        public void ChoiceRequest(int idx, string choice)
        {
            if (choice == "Accepted")
                Persons[idx].Status = StatusUser.Accepted;
            else
                Persons[idx].Status = StatusUser.Refuse;
        }
    }
}

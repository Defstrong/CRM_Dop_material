using CRM.DTO;
using CRM.Enums;
using CRM.Models;

namespace CRM.Services
{
    public sealed class AdminServices
    {
        private static List<Person> Persons;
        static List<Loan> RequestsLoanUser;
        static List<Massage> Massages;
        public AdminServices(List<Person> persons, List<Loan> requestsLoanUser, List<Massage> massages)
        {
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
            Massages = massages;
        }
        public void EditPerson(InputUserDto editUser, Guid idPerson)
        {
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(idPerson));
            if (!string.IsNullOrEmpty(editUser.FirstName))
                Persons[idxPerson].FirstName = editUser.FirstName;
            if (!string.IsNullOrEmpty(editUser.LastName))
                Persons[idxPerson].LastName = editUser.LastName;
            if (!string.IsNullOrEmpty(editUser.Patronymic))
                Persons[idxPerson].Patronymic = editUser.Patronymic;
            if (!string.IsNullOrEmpty(editUser.Age.ToString()))
                Persons[idxPerson].Age = editUser.Age;
            if (!string.IsNullOrEmpty(editUser.Login))
                Persons[idxPerson].Login = editUser.Login;
            if (!string.IsNullOrEmpty(editUser.Password))
                Persons[idxPerson].Password = editUser.Password;
        }

        public void DeletePerson(Guid idPerson)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(idPerson));
            Persons.Remove(Persons[idx]);
        }

        public void BlockPerson(Guid idPerson)
        {
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(idPerson));
            Persons[idxPerson].Status = StatusUser.Refuse;
        }

        public void LoanForUser(InputUserDto dtoLoan, int idxUser)
        {
            RequestsLoanUser.Add(new Loan
            {
                Id = Guid.NewGuid(),
                IdSender = dtoLoan.Id,
                Payday = dtoLoan.Payday,
                Name = Persons[idxUser].FirstName,
                Age = Persons[idxUser].Age,
                CountMoney = dtoLoan.AmountMoney,
                StatusDuty = StatusUser.Accepted
            });
        }

        public void PayOffUserDebts(int idxUserTransaction)
        {
            RequestsLoanUser.Remove(RequestsLoanUser[idxUserTransaction]);
        }
    }
}

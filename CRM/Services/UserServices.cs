using CRM.DTO;
using CRM.Enums;
using CRM.Models;

namespace CRM.Services
{
    public sealed class UserServices
    {
        private static List<Person> Persons;
        static List<Loan> RequestsLoanUser;
        public UserServices(List<Person> persons, List<Loan> requestsLoanUser)
        {
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
        }

        public void DeleteProfile(Guid id)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(id));
            Persons.Remove(Persons[idx]);
        }

        public void EditProfile(InputUserDto editUser, Guid id)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(id));
            if (!string.IsNullOrEmpty(editUser.FirstName))
                Persons[idx].FirstName = editUser.FirstName;
            if (!string.IsNullOrEmpty(editUser.LastName))
                Persons[idx].LastName = editUser.LastName;
            if (!string.IsNullOrEmpty(editUser.Patronymic))
                Persons[idx].Patronymic = editUser.Patronymic;
            if (!string.IsNullOrEmpty(editUser.Age.ToString()))
                Persons[idx].Age = editUser.Age;
            if (!string.IsNullOrEmpty(editUser.Login))
                Persons[idx].Login = editUser.Login;
            if (!string.IsNullOrEmpty(editUser.Password))
                Persons[idx].Password = editUser.Password;
        }

        public void Loan(InputUserDto dtoLoan, int index)
        {
            RequestsLoanUser.Add(new Loan
            {
                Id = Guid.NewGuid(),
                IdSender = dtoLoan.Id,
                Payday = dtoLoan.Payday,
                Name = Persons[index].FirstName,
                Age = Persons[index].Age,
                CountMoney = dtoLoan.AmountMoney,
                StatusDuty = StatusUser.Pending
            }) ;
        }

        public void PayTheDebtOff(Guid id)
        {
            int idx = RequestsLoanUser.FindIndex(x => x.Id.Equals(id));
            if (idx != -1)
                RequestsLoanUser.Remove(RequestsLoanUser[idx]);
            else
                throw new Exception("Request is not found");
        }
    }
}

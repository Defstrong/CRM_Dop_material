using DTO;
using Enums;
using Models;
using AbstractClasses;

namespace Services
{
    class UserLoanServices : LoanServices
    {
        public UserLoanServices(List<Person> persons, List<Loan> requestsLoanUser) : base(persons, requestsLoanUser) {}

        public override void Loan(InputUserDto dtoLoan, int index)
        {
            RequestsLoanUser.Add(new Loan
            {
                Id = Guid.NewGuid(),
                IdSender = dtoLoan.Id,
                Payday = dtoLoan.Payday,
                Name = Persons[index].FirstName,
                Age = Persons[index].Age,
                CountMoney = dtoLoan.AmountMoney,
                StatusDuty = StatusLoan.Pending
            });
        } 

        public override void PayTheDebtOff(Guid id)
        {
            int idxTransaction = RequestsLoanUser.FindIndex(x => x.Id.Equals(id));
            if (idxTransaction != -1)
                RequestsLoanUser[idxTransaction].StatusDuty = StatusLoan.Paid;
            else
                throw new Exception("Request is not found");
        }
    }

    class AdminLoanServices : LoanServices
    {
        public AdminLoanServices(List<Person> persons, List<Loan> requestsLoanUser) : base(persons, requestsLoanUser) { }

        public override void Loan(InputUserDto dtoLoan, int idxUser)
        {
            RequestsLoanUser.Add(new Loan
            {
                Id = Guid.NewGuid(),
                IdSender = dtoLoan.Id,
                Payday = dtoLoan.Payday,
                Name = Persons[idxUser].FirstName,
                Age = Persons[idxUser].Age,
                CountMoney = dtoLoan.AmountMoney,
                StatusDuty = StatusLoan.Accepted
            });
        }

        public override void PayTheDebtOff(Guid id)
        {
            int idxUserTransaction = RequestsLoanUser.FindIndex(x => x.Id.Equals(id));
            RequestsLoanUser[idxUserTransaction].StatusDuty = StatusLoan.Paid;
        }
    }
}
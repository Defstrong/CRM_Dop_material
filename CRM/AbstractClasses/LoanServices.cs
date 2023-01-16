using DTO;
using Models;

namespace AbstractClasses
{
    abstract class LoanServices
    {
        public readonly List<Person> Persons;
        public readonly List<Loan> RequestsLoanUser;
        public LoanServices(List<Person> persons, List<Loan> requestsLoanUser)
        {
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
        }

        public abstract void Loan(InputUserDto dtoLoan, int index);
        public abstract void PayTheDebtOff(Guid id);
    }

}

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

        public abstract Result<bool> Loan(InputUserDto dtoLoan, Guid idPerson);
        public abstract Result<bool> PayTheDebtOff(Guid id);
    }

}

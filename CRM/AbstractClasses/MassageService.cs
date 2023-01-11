using DTO;
using Models;

namespace AbstractClasses
{
    abstract class MassageService
    {
        public List<Massage> Massages { get; set; }
        public List<Person> Persons;
        public List<Loan> RequestsLoanUser;

        public MassageService(List<Massage> massages, List<Person> persons, List<Loan> requestsLoanUser)
        {
            Massages = massages;
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
        }

        abstract public void SendMassage(InputUserDto massages);

        public virtual void OverdueRequest(ref bool sendMassageForOverdueRequest) { }
    }
}

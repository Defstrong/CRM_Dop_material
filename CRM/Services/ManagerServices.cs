using DTO;
using Enums;
using Models;

namespace Services
{
    public sealed class ManagerServices
    {
        static List<Loan> RequestsLoanUser;
        public ManagerServices(List<Loan> requestsLoanUser)
        {
            RequestsLoanUser = requestsLoanUser;
        }

        public void ChoiceLoanRequest(int idxUserLoan, string choice)
        {
            if (choice == "Accepted")
                RequestsLoanUser[idxUserLoan].StatusDuty = StatusLoan.Accepted;
            else if(choice == "Refuse")
                RequestsLoanUser[idxUserLoan].StatusDuty = StatusLoan.Refuse;
        }
    }
}

using CRM.DTO;
using CRM.Enums;
using CRM.Models;

namespace CRM.Services
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
                RequestsLoanUser[idxUserLoan].StatusDuty = StatusUser.Accepted;
            else if(choice == "Refuse")
                RequestsLoanUser[idxUserLoan].StatusDuty = StatusUser.Refuse;
        }
    }
}

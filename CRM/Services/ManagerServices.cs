using Enums;
using Models;

namespace Services
{
    public sealed class ManagerServices
    {
        private readonly List<Loan> RequestsLoanUser;
        public ManagerServices(List<Loan> requestsLoanUser)
        {
            RequestsLoanUser = requestsLoanUser;
        }

        public void ChoiceLoanRequest(Guid idTransaction, StatusLoan choice)
        {
            int idxUserLoan = RequestsLoanUser.FindIndex(x => x.Id.Equals(idTransaction));
            if (choice == StatusLoan.Accepted)
                RequestsLoanUser[idxUserLoan].StatusDuty = StatusLoan.Accepted;
            else if(choice == StatusLoan.Refuse)
                RequestsLoanUser[idxUserLoan].StatusDuty = StatusLoan.Refuse;
        }
    }
}

using DTO;
using Enums;
using Models;
using AbstractClasses;

namespace Services
{
    class UserLoanServices : LoanServices
    {
        public UserLoanServices(List<Person> persons, List<Loan> requestsLoanUser) : base(persons, requestsLoanUser) {}

        public override Result<bool> Loan(InputUserDto dtoLoan, Guid idUser)
        {
            var resultLoanUser = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxUser = Persons.FindIndex(x => x.Id.Equals(idUser));
            if (idxUser != -1)
            {
                if (!string.IsNullOrEmpty(dtoLoan.AmountMoney.ToString()) &&
                    !string.IsNullOrEmpty(dtoLoan.Payday.ToString()))
                {
                    resultLoanUser.TextError += "Create loan for user completed successfully";
                    resultLoanUser.Error = ErrorStatus.Success;
                    resultLoanUser.IsSuccessfully = true;
                    resultLoanUser.Payload = true;
                    RequestsLoanUser.Add(new Loan
                    {
                        Id = Guid.NewGuid(),
                        IdSender = dtoLoan.Id,
                        Payday = dtoLoan.Payday,
                        Name = Persons[idxUser].FirstName,
                        Age = Persons[idxUser].Age,
                        CountMoney = dtoLoan.AmountMoney,
                        StatusDuty = StatusLoan.Pending
                    });
                    return resultLoanUser;
                }
                else if (string.IsNullOrEmpty(dtoLoan.AmountMoney.ToString()))
                    resultLoanUser.TextError += "Text box AmoutMoney is empty. Please fill in field AmoutMoney";
                else if (string.IsNullOrEmpty(dtoLoan.Payday.ToString()))
                    resultLoanUser.TextError += "Text box Payday is empty. Please fill in field Payday";
                resultLoanUser.Error = ErrorStatus.ArgumentNull;
            }
            else
            {
                resultLoanUser.TextError += "User is not found";
                resultLoanUser.Error = ErrorStatus.NotFound;
            }
            return resultLoanUser;
        } 

        public override Result<bool> PayTheDebtOff(Guid idUser)
        {
            var resultPayTheDebtOff = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxUserTransaction = RequestsLoanUser.FindIndex(x => x.Id.Equals(idUser));
            if (idxUserTransaction != -1)
            {
                resultPayTheDebtOff.TextError += "Pay the debt off completed successfully";
                resultPayTheDebtOff.Error = ErrorStatus.Success;
                resultPayTheDebtOff.IsSuccessfully = true;
                resultPayTheDebtOff.Payload = true;
                RequestsLoanUser[idxUserTransaction].StatusDuty = StatusLoan.Paid;
                return resultPayTheDebtOff;
            }
            resultPayTheDebtOff.TextError += "Loan is not found";
            resultPayTheDebtOff.Error = ErrorStatus.NotFound;
            return resultPayTheDebtOff;
        }

        public string StatusDuty(Guid idUser)
        {
            string showStatusDuty = string.Empty;
            bool thePresenceOfDebt = false;

            foreach (var ii in RequestsLoanUser)
            {
                if (ii.IdSender == idUser)
                {
                    showStatusDuty += $"{ii.StatusDuty}\n";
                    showStatusDuty += $"{ii.ToString()}\n";
                    thePresenceOfDebt = true;
                }
            }
            if (!thePresenceOfDebt)
                showStatusDuty = "\nYou don't have loan\n";

            return showStatusDuty;
        }
    }

    class AdminLoanServices : LoanServices
    {
        public AdminLoanServices(List<Person> persons, List<Loan> requestsLoanUser) 
            : base(persons, requestsLoanUser) { }

        public override Result<bool> Loan(InputUserDto dtoLoan, Guid idUser)
        {
            var resultLoanAdmin = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxUser = Persons.FindIndex(x => x.Id.Equals(idUser));
            if (idxUser != -1)
            {
                if (!string.IsNullOrEmpty(dtoLoan.AmountMoney.ToString()) &&
                    !string.IsNullOrEmpty(dtoLoan.Payday.ToString()))
                {
                    resultLoanAdmin.TextError += "Create loan for user completed successfully";
                    resultLoanAdmin.Error = ErrorStatus.Success;
                    resultLoanAdmin.IsSuccessfully = true;
                    resultLoanAdmin.Payload = true;
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
                    return resultLoanAdmin;
                }
                else if (string.IsNullOrEmpty(dtoLoan.AmountMoney.ToString()))
                    resultLoanAdmin.TextError += "Text box AmoutMoney is empty. Please fill in field AmoutMoney";
                else if (string.IsNullOrEmpty(dtoLoan.Payday.ToString()))
                    resultLoanAdmin.TextError += "Text box Payday is empty. Please fill in field Payday";
                resultLoanAdmin.Error = ErrorStatus.ArgumentNull;
            }
            else
            {
                resultLoanAdmin.TextError += "User is not found";
                resultLoanAdmin.Error = ErrorStatus.NotFound;
            }
            return resultLoanAdmin;
        }

        public override Result<bool> PayTheDebtOff(Guid idTransaction)
        {
            var resultPayTheDebtOff = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxUserTransaction = RequestsLoanUser.FindIndex(x => x.Id.Equals(idTransaction));
            if(idxUserTransaction != -1)
            {
                resultPayTheDebtOff.TextError += "Pay the debt off completed successfully";
                resultPayTheDebtOff.Error = ErrorStatus.Success;
                resultPayTheDebtOff.IsSuccessfully = true;
                resultPayTheDebtOff.Payload = true;
                RequestsLoanUser[idxUserTransaction].StatusDuty = StatusLoan.Paid;
                return resultPayTheDebtOff;
            }
            resultPayTheDebtOff.TextError += "Loan is not found";
            resultPayTheDebtOff.Error = ErrorStatus.NotFound;
            return resultPayTheDebtOff;
        }
    }
}
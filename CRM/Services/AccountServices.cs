using Models;
using Interfaces;
using DTO;
using Enums;
namespace Services
{
    class AccountServices : IPayMoney, IFindingAnAvailableAccount
    {
        private readonly List<Person> Persons;
        public long CompanyAccount { get; set; } = 100000000;
        public AccountServices(List<Person> persons)
        {
            Persons = persons;
        }
        public AccountServices() { }

        public void PayMoney(Guid idEmployee)
        {
            var employee = Persons.FirstOrDefault(x => x.Id.Equals(idEmployee));
            if (employee is null)
            {
                employee.BankAccount[0] += employee.DataPassportEmployeeAndSalary.Salary;
                CompanyAccount -= employee.DataPassportEmployeeAndSalary.Salary;
                employee.DataPassportEmployeeAndSalary.SalaryReceived = true;
            }
        }

        public int FindingAnAvailableAccount(Person person, long countMoney)
        {
            for (int i = 0; i < 20; i++)
                if (person.BankAccount[i] >= countMoney)
                    return i;

            return -1;
        }

        public Result<bool> MoneyTransfer(MoneyTransfer moneyTransfer)
        {
            bool haveErrorOrNo = false;
            var resultMoneyTransfer = new Result<bool> { IsSuccessfully = false, Payload = false };
            if(moneyTransfer.IdReceiver == Guid.Empty)
            {
                resultMoneyTransfer.TextError += "Text box IdReceiver is empty. Please fill in fealt IdReceiver\n";
                haveErrorOrNo = true;
            }
            if(moneyTransfer.CountMoney <= 0)
            {
                resultMoneyTransfer.TextError += "Text box CoutnMoney is empty. Please fill in fealt CoutnMoney\n";
                haveErrorOrNo = true;
            }
            if(moneyTransfer.AccountNumber < 0)
            {
                resultMoneyTransfer.TextError += "Text box AccountNumber is empty. Please fill in fealt AccountNumber\n";
                haveErrorOrNo = true;
            }
            var sender = Persons.FirstOrDefault(x => x.Id.Equals(moneyTransfer.IdSender));
            int avaliableAccount = FindingAnAvailableAccount(sender, moneyTransfer.CountMoney);
            var receiver = Persons.FirstOrDefault(x => x.Id.Equals(moneyTransfer.IdReceiver));
            if(!haveErrorOrNo && avaliableAccount != -1)
            {
                resultMoneyTransfer.Error = ErrorStatus.Success;
                resultMoneyTransfer.Payload = true;
                resultMoneyTransfer.IsSuccessfully = true;
                resultMoneyTransfer.TextError = "Money transfer completed successfully\n";
                sender.BankAccount[avaliableAccount] -= moneyTransfer.CountMoney;
                receiver.BankAccount[moneyTransfer.AccountNumber] += moneyTransfer.CountMoney;
                return resultMoneyTransfer;
            }
            resultMoneyTransfer.Error = ErrorStatus.ArgumentNull;
            return resultMoneyTransfer;
        }
        public void WithdrawFromTheEmployeeAccount(Guid idEmployee, long countMoney)
        {
            var employee = Persons.FirstOrDefault(x => x.Id.Equals(idEmployee));

            int avaliableAccount = FindingAnAvailableAccount(employee, countMoney);

            if(avaliableAccount != -1)
            {
                employee.BankAccount[avaliableAccount] -= countMoney;
                CompanyAccount += countMoney;
            }
        }
    }
}

using AbstractClasses;
using DTO;
using Enums;
using Models;

namespace Services
{
    class UserMassage : MassageService
    {
        public UserMassage(
            List<Massage> massages,
            List<Person> persons,
            List<Loan> loans) :
            base(massages, persons, loans)
        { }

        public override void SendMassage(InputUserDto massageInformation)
        {
            Massages.Add(new Massage
            {
                Id = Guid.NewGuid(),
                Name = massageInformation.FirstName,
                Role = massageInformation.Role,
                IdSender = massageInformation.Id,
                Text = massageInformation.Text,
                Theme = massageInformation.Theme
            });
        }
    }

    class AdminMassage : MassageService
    {
        public AdminMassage(
            List<Massage> massages,
            List<Person> persons,
            List<Loan> loans) :
            base(massages, persons, loans)
        { }
        public override void SendMassage(InputUserDto massageInformation)
        {
            Massages.Add(new Massage
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                IdRecipient = massageInformation.Id,
                Role = Roles.User,
                Text = massageInformation.Text,
                Theme = massageInformation.Theme
            });
        }
    }

    class ManagerMassage : MassageService
    {
        public ManagerMassage(
            List<Massage> massages, 
            List<Person> persons, 
            List<Loan> loans) : 
            base(massages, persons, loans) { }

        public override void SendMassage(InputUserDto massageInformation)
        {
            Massages.Add(new Massage
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                IdRecipient = massageInformation.Id,
                Role = Roles.User,
                Text = massageInformation.Text,
                Theme = massageInformation.Theme
            });
        }
        public override void OverdueRequest(ref bool sendMassageForOverdueRequest)
        {
            for (int i = 0; i < RequestsLoanUser.Count; i++)
            {
                if (RequestsLoanUser[i].Payday >= DateTime.Now && RequestsLoanUser[i].StatusDuty == StatusLoan.Accepted)
                {
                    int idx = Persons.FindIndex(x => x.Id == RequestsLoanUser[i].IdSender);
                    sendMassageForOverdueRequest = true;
                    Massages.Add(new Massage
                    {
                        Id = Guid.NewGuid(),
                        Name = "Manager",
                        IdRecipient = RequestsLoanUser[i].IdSender,
                        Role = Roles.User,
                        Text = $"Dear client {Persons[idx].FirstName} {Persons[idx].LastName} " +
                        $"{Persons[idx].Patronymic} you have not repaid your debt, please within the next three days",
                        Theme = "Overdue"
                    });
                }
            }

        }
    }

}

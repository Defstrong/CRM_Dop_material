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

        public override Result<bool> SendMassage(DtoSendMassage massageInformation)
        {
            Result<bool> result = new Result<bool>();
            bool correctOrNot = true;
            if(massageInformation.AdminOrManager != "Admin" && massageInformation.AdminOrManager != "Manager")
            {
                correctOrNot = false;

                if (string.IsNullOrEmpty(massageInformation.AdminOrManager))
                    result.TextError += "Text box AdminOrManager is empty. Please fill in the field AdminOrManager\n";
                else
                    result.TextError += "Invalid command in text box AdminOrManager\n";

            }
            else
            {
                if (massageInformation.AdminOrManager == "Admin")
                    massageInformation.Role = Roles.Admin;
                else
                    massageInformation.Role = Roles.Manager;

            }
            if(string.IsNullOrEmpty(massageInformation.FirstName))
            {
                correctOrNot = false;
                result.TextError += "Text box FirstName is empty. Please fill in the field FirstName\n";
            }
            if(string.IsNullOrEmpty(massageInformation.Text))
            {
                correctOrNot = false;
                result.TextError += "Text box TextMassage is empty. Please fill in the field TextMassage\n";
            }
            if(string.IsNullOrEmpty(massageInformation.Theme))
            {
                correctOrNot = false;
                result.TextError += "Text box ThemeMassage is empty. Please fill in the field ThemeMassage\n";
            }

            if(!correctOrNot)
            {
                result.Error = ErrorStatus.ArgumentNull;
                result.IsSuccessfully = false;
                result.Payload = false;
            }
            else
            {
                result.Error = ErrorStatus.Success;
                result.Payload = true;
                result.IsSuccessfully = true;
                result.TextError = "Message sent successfully\n";

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
            return result;

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
        public override Result<bool> SendMassage(DtoSendMassage massageInformation)
        {
            Result<bool> result = new Result<bool>();
            Roles adminOrManager;
            bool correctOrNot = true;
            if (string.IsNullOrEmpty(massageInformation.Text))
            {
                correctOrNot = false;
                result.TextError += "Text box TextMassage is empty. Please fill in the field TextMassage\n";
            }
            if (string.IsNullOrEmpty(massageInformation.Theme))
            {
                correctOrNot = false;
                result.TextError += "Text box ThemeMassage is empty. Please fill in the field ThemeMassage\n";
            }
            if(massageInformation.Id == Guid.Empty)
            {
                correctOrNot = false;
                result.TextError += "Text box IdUser is empty. Please fill in the field IdUser\n";
            }

            if (!correctOrNot)
            {
                result.Error = ErrorStatus.ArgumentNull;
                result.IsSuccessfully = false;
                result.Payload = false;
            }
            else
            {
                result.Error = ErrorStatus.Success;
                result.Payload = true;
                result.IsSuccessfully = true;
                result.TextError = "Message sent successfully\n";

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
            return result;

        }
    }
    class ManagerMassage : MassageService
    {
        public ManagerMassage(
            List<Massage> massages, 
            List<Person> persons, 
            List<Loan> loans) : 
            base(massages, persons, loans) { }

        public override Result<bool> SendMassage(DtoSendMassage massageInformation)
        {
            Result<bool> result = new Result<bool>();
            bool correctOrNot = true;
            if (string.IsNullOrEmpty(massageInformation.Text))
            {
                correctOrNot = false;
                result.TextError += "Text box TextMassage is empty. Please fill in the field TextMassage\n";
            }
            if (string.IsNullOrEmpty(massageInformation.Theme))
            {
                correctOrNot = false;
                result.TextError += "Text box ThemeMassage is empty. Please fill in the field ThemeMassage\n";
            }
            if (massageInformation.Id == Guid.Empty)
            {
                correctOrNot = false;
                result.TextError += "Text box IdUser is empty. Please fill in the field IdUser\n";
            }


            if (!correctOrNot)
            {
                result.Error = ErrorStatus.ArgumentNull;
                result.IsSuccessfully = false;
                result.Payload = false;
            }
            else
            {
                result.Error = ErrorStatus.Success;
                result.Payload = true;
                result.IsSuccessfully = true;
                result.TextError = "Message sent successfully\n";

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
            return result;

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

using CRM.DTO;
using Enums;
using Models;

namespace AbstractClasses
{
    abstract class MassageService
    {
        public List<Massage> Massages;
        public List<Person> Persons;
        public List<Loan> RequestsLoanUser;

        public MassageService(List<Massage> massages, List<Person> persons, List<Loan> requestsLoanUser)
        {
            Massages = massages;
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
        }

        public abstract Result<bool> SendMassage(DtoSendMassage massages);

        public virtual void OverdueRequest(ref bool sendMassageForOverdueRequest) { }

        public Result<Massage> MyMassages(Guid idMassage)
        {
            Result<Massage> result = new Result<Massage>();

            string guidToString = idMassage.ToString();
            int idx = Massages.FindIndex(x => x.Id.Equals(idMassage));

            if (idMassage == Guid.Empty || guidToString.Length != 36 || idx == -1)
            {
                if (idMassage == Guid.Empty)
                {
                    result.TextError += "Text box IdMassage is empty. Please fill in field IdMassage";
                    result.Error = ErrorStatus.ArgumentNull;
                }
                else if (guidToString.Count() != 36)
                {
                    result.Error = ErrorStatus.InvalidCommand;
                    result.TextError += "Invalid command in IdAdmin. Please try again";
                }
                else if (idx == -1)
                {
                    result.Error = ErrorStatus.NotFound;
                    result.TextError += "Massage is not found";
                }
                result.IsSuccessfully = false;
            }
            else
            {
                result.IsSuccessfully = true;
                result.TextError += "Operation is successfully\n";
                result.Error = ErrorStatus.Success;
                result.Payload = Massages[idx];
            }
            return result;

        }
    }
}

using Enums;
using Models;
using System.Xml.XPath;

namespace Services
{
    public sealed class AccountantServices
    {
        private readonly List<Person> Persons;
        private readonly List<PrepaidExpense> RequestsEmployeeForAdvance;
        public AccountantServices(List<Person> persons, List<PrepaidExpense> requestsEmployeeForAdvance)
        {
            Persons = persons;
            RequestsEmployeeForAdvance = requestsEmployeeForAdvance;
        }

        public Result<bool> EmployeeSalary(Guid idEmployee, int salaryEmployee)
        {
            Result<bool> result = new Result<bool>();

            string examinationId = idEmployee.ToString();
            string examinationSalary = salaryEmployee.ToString();
            if (string.IsNullOrEmpty(examinationId))
            {
                result.TextError += "Text box IdEmployee is empty. Please fill in field IdEmployee";
                result.Error = ErrorStatus.ArgumentNull;
            }
            else if (string.IsNullOrEmpty(examinationSalary)) 
            {
                result.TextError += "Text box Salary is empty. Please fill in field Salary";
                result.Error = ErrorStatus.ArgumentNull;
            }
            else
            {
                result.TextError += "Action Employee Salary is completed successfully";
                result.Error = ErrorStatus.Success;
                result.IsSuccessfully = true;
                result.Payload = true;
                int idxEmployee = Persons.FindIndex(x => x.Id.Equals(idEmployee));
                Persons[idxEmployee].DataPassportEmployeeAndSalary.Salary = salaryEmployee;
                return result;
            }
            result.IsSuccessfully = false;
            result.Payload = false;
            return result;
        }
        public Result<bool> RequestForAdvance(Guid idRequestForAdvance, StatusRequestForAdvance choiceAccountant)
        {
            var result = new Result<bool>();
            int idxRequestEmployee = RequestsEmployeeForAdvance.FindIndex(x => x.Id.Equals(idRequestForAdvance));
            if(idxRequestEmployee == -1)
            {
                result.TextError += "RequestsEmployee is not found";
                result.Error = ErrorStatus.NotFound;
            }
            int idxEmployee = Persons.FindIndex(x => x.Id.Equals(RequestsEmployeeForAdvance[idxRequestEmployee].IdEmloyee));
            if(idxEmployee == -1)
            {
                result.TextError += "Employee is not found";
                result.Error = ErrorStatus.NotFound;
            }
            string examinationChoiceAccountant = choiceAccountant.ToString();
            if (string.IsNullOrEmpty(examinationChoiceAccountant))
            {
                result.TextError += "Text box ChoiceAccountant is empty. Please fill in field ChoiceAccountant";
                result.Error = ErrorStatus.ArgumentNull;
            }
            else
            {
                if (choiceAccountant == StatusRequestForAdvance.Accepted)
                {
                    Persons[idxEmployee].DataPassportEmployeeAndSalary.SalaryReceived = true;
                    RequestsEmployeeForAdvance[idxRequestEmployee].Status = StatusRequestForAdvance.Accepted;
                    Persons[idxEmployee].DataPassportEmployeeAndSalary.SalaryPaymentDate =
                        Persons[idxEmployee].DataPassportEmployeeAndSalary.SalaryPaymentDate.AddMonths(
                        RequestsEmployeeForAdvance[idxRequestEmployee].CountMonths);

                }
                else if (choiceAccountant == StatusRequestForAdvance.Refuse)
                {
                    RequestsEmployeeForAdvance[idxRequestEmployee].Status = StatusRequestForAdvance.Refuse;
                }
                result.TextError += "Action is successfully";
                result.Error = ErrorStatus.Success;
                result.IsSuccessfully = true;
                result.Payload = true;
                return result;
            }
            result.IsSuccessfully = false;
            result.Payload = false;
            return result;
        }
    }
}

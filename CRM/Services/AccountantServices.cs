using Enums;
using Models;

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

        public void EmployeeSalary(Guid idEmployee, int salaryEmployee)
        {
            int idxEmployee = Persons.FindIndex(x => x.Id.Equals(idEmployee));
            Persons[idxEmployee].DataPassportEmployeeAndSalary.Salary = salaryEmployee;
        }
        public void RequestForAdvance(Guid idRequestForAdvance, string choiceAccountant)
        {
            int idxRequestEmployee = RequestsEmployeeForAdvance.FindIndex(x => x.Id.Equals(idRequestForAdvance));
            int idxEmployee = Persons.FindIndex(x => x.Id.Equals(RequestsEmployeeForAdvance[idxRequestEmployee].IdEmloyee));
            if(choiceAccountant == "Accepted")
            {
                Persons[idxEmployee].DataPassportEmployeeAndSalary.SalaryReceived = true;
                RequestsEmployeeForAdvance[idxRequestEmployee].Status = StatusRequestForAdvance.Accepted;
                Persons[idxEmployee].DataPassportEmployeeAndSalary.SalaryPaymentDate = 
                    Persons[idxEmployee].DataPassportEmployeeAndSalary.SalaryPaymentDate.AddMonths( 
                    RequestsEmployeeForAdvance[idxRequestEmployee].CountMonths );

            }
            else if(choiceAccountant == "Refuse")
            {
                RequestsEmployeeForAdvance[idxRequestEmployee].Status = StatusRequestForAdvance.Refuse;
            }
        }
    }
}

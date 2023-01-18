using DTO;
using Enums;
using Models;

namespace Services
{
    public sealed class AdminServices
    {
        private readonly List<Person> Persons;
        private readonly List<Loan> RequestsLoanUser;

        public AdminServices(List<Person> persons, List<Loan> requestsLoanUser)
        {
            Persons = persons;
            RequestsLoanUser = requestsLoanUser;
        }

        public void EditPerson(DtoEditUser dtoEditUser)
        {
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(dtoEditUser.Id));
            if (!string.IsNullOrEmpty(dtoEditUser.FirstName))
                Persons[idxPerson].FirstName = dtoEditUser.FirstName;
            if (!string.IsNullOrEmpty(dtoEditUser.LastName))
                Persons[idxPerson].LastName = dtoEditUser.LastName;
            if (!string.IsNullOrEmpty(dtoEditUser.Patronymic))
                Persons[idxPerson].Patronymic = dtoEditUser.Patronymic;
            if (!string.IsNullOrEmpty(dtoEditUser.Age.ToString()))
                Persons[idxPerson].Age = dtoEditUser.Age;
            if (!string.IsNullOrEmpty(dtoEditUser.Login))
                Persons[idxPerson].Login = dtoEditUser.Login;
            if (!string.IsNullOrEmpty(dtoEditUser.Password))
                Persons[idxPerson].Password = dtoEditUser.Password;
        }

        public void DeletePerson(Guid idPerson)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(idPerson));
            Persons.Remove(Persons[idx]);
        }

        public void BlockPerson(Guid idPerson)
        {
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(idPerson));
            Persons[idxPerson].Status = StatusUser.Block;
        }

        public void UserToEmployee(DtoDataPassportAndSallary dataPassportEmployee, Guid idUser, string employee)
        {
            int idxUser = Persons.FindIndex(x => x.Id.Equals(idUser));
            Persons[idxUser].Role = Roles.Employee;
            if (employee == "Accountant")
            {
                Persons[idxUser].Responsibility = ResponsibilityPerson.Accountant;
                Persons[idxUser].DataPassportEmployeeAndSalary = dataPassportEmployee;
                Persons[idxUser].DataPassportEmployeeAndSalary.SalaryPaymentDate = DateTime.Now;
                Persons[idxUser].DataPassportEmployeeAndSalary.SalaryPaymentDate.AddMonths(1);
                Persons[idxUser].DataPassportEmployeeAndSalary.SalaryReceived = false;
            }
        }

        public void DuplicatePerson(Guid idPerson, string newLogin, string newPassword)
        {
            int idxPerson = Persons.FindIndex(x => x.Id== idPerson);
            Person newDuplicatePerson = Persons[idxPerson];
            newDuplicatePerson.Login = newLogin;
            newDuplicatePerson.Password = newPassword;
            newDuplicatePerson.Id = Guid.NewGuid();
            Persons.Add(newDuplicatePerson);
        }

        public void DuplicateLoan(Guid idLoan)
        {
            int idxLoan = RequestsLoanUser.FindIndex(x => x.Id.Equals(idLoan));
            var newLoan = RequestsLoanUser[idxLoan];
            newLoan.Id = Guid.NewGuid();
            RequestsLoanUser.Add(newLoan);
        }
    }
}

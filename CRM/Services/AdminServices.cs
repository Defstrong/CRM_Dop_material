using DTO;
using Enums;
using Models;
using System.Xml.XPath;

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

        public Result<bool> EditPerson(DtoEditUser dtoEditUser)
        {
            var resultEditPerson = new Result<bool>();
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(dtoEditUser.Id));
            if(idxPerson != -1)
            {
                resultEditPerson.TextError = "Edit person completed successfuly";
                resultEditPerson.IsSuccessfully = true;
                resultEditPerson.Error = ErrorStatus.Success;
                resultEditPerson.Payload = true;

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
                return resultEditPerson;
            }
            resultEditPerson.TextError = "User is not found";
            resultEditPerson.IsSuccessfully = false;
            resultEditPerson.Error = ErrorStatus.NotFound;
            resultEditPerson.Payload = false;
            return resultEditPerson;
        }

        public Result<bool> DeletePerson(Guid idPerson)
        {
            var resultDeletePerson = new Result<bool>();
            int idx = Persons.FindIndex(x => x.Id.Equals(idPerson));
            if(idx != -1)
            {
                resultDeletePerson.TextError = "Delete person completed successfully";
                resultDeletePerson.Error = ErrorStatus.Success;
                resultDeletePerson.IsSuccessfully = true;
                resultDeletePerson.Payload = true;
                Persons.Remove(Persons[idx]);
                return resultDeletePerson;
            }
            resultDeletePerson.TextError = "User is not found";
            resultDeletePerson.Error = ErrorStatus.NotFound;
            resultDeletePerson.IsSuccessfully = false;
            resultDeletePerson.Payload = false;
            return resultDeletePerson;
        }

        public Result<bool> BlockPerson(Guid idPerson)
        {
            var resultBlockPerson = new Result<bool>();

            int idxPerson = Persons.FindIndex(x => x.Id.Equals(idPerson));
            if (idxPerson != -1)
            {
                resultBlockPerson.TextError = "Block person completed successfully";
                resultBlockPerson.Error = ErrorStatus.Success;
                resultBlockPerson.IsSuccessfully = true;
                resultBlockPerson.Payload = true;
                Persons[idxPerson].Status = StatusUser.Block;
                return resultBlockPerson;
            }
            resultBlockPerson.TextError = "User is not found";
            resultBlockPerson.Error = ErrorStatus.NotFound;
            resultBlockPerson.IsSuccessfully = false;
            resultBlockPerson.Payload = false;
            return resultBlockPerson;
        }

        public Result<bool> UserToEmployee(
            DtoDataPassportAndSallary dataPassportEmployee, 
            Guid idUser, ResponsibilityPerson employee)
        {
            var resultUserToEmployee = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxUser = Persons.FindIndex(x => x.Id.Equals(idUser));
            if(idxUser != -1)
            {
                bool errorOrNo = false;
                if(string.IsNullOrEmpty(dataPassportEmployee.FirstName))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box FirstName is empty. Please fill in field FirstName";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.LastName))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box LastName is empty. Please fill in field LastName";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.Patronymic))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box Patronymic is empty. Please fill in field Patronymic";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.DateOfBirth.ToString()))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box DateOfBirth is empty. Please fill in field DateOfBirth";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.PlaceOfResidence))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box PlaceOfResidence is empty. Please fill in field PlaceOfResidence";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.MaritalStatus))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box MaritalStatus is empty. Please fill in field MaritalStatus";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.PhoneNumber))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box PhoneNumber is empty. Please fill in field PhoneNumber";
                }
                if (string.IsNullOrEmpty(dataPassportEmployee.Salary.ToString()))
                {
                    errorOrNo = true;
                    resultUserToEmployee.TextError += "Text box Salary is empty. Please fill in field Salary";
                }
                resultUserToEmployee.Error = ErrorStatus.ArgumentNull;

                if(!errorOrNo)
                {
                    resultUserToEmployee.TextError += "User to employee completed is successfully";
                    resultUserToEmployee.Error = ErrorStatus.Success;
                    resultUserToEmployee.IsSuccessfully = true;
                    resultUserToEmployee.Payload = true;
                    Persons[idxUser].Role = Roles.Employee;
                    if (employee == ResponsibilityPerson.Accountant)
                    {
                        Persons[idxUser].Responsibility = ResponsibilityPerson.Accountant;
                        Persons[idxUser].DataPassportEmployeeAndSalary = dataPassportEmployee;
                        Persons[idxUser].DataPassportEmployeeAndSalary.SalaryPaymentDate = DateTime.Now;
                        Persons[idxUser].DataPassportEmployeeAndSalary.SalaryPaymentDate.AddMonths(1);
                        Persons[idxUser].DataPassportEmployeeAndSalary.SalaryReceived = false;
                    }
                    return resultUserToEmployee;
                }
            }
            resultUserToEmployee.TextError += "User is not found";
            resultUserToEmployee.Error = ErrorStatus.NotFound;
            return resultUserToEmployee;
        }


        public Result<bool> DuplicatePerson(Guid idPerson, string newLogin, string newPassword)
        {
            var resultDuplicatePerson = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(idPerson));
            bool errorOrNo = false;
            if(idxPerson != -1)
            {
                if(string.IsNullOrEmpty(newLogin))
                {
                    errorOrNo = true;
                    resultDuplicatePerson.TextError += "Text box NewLogin is empty. Please fill in field NewLogin";
                }
                if (string.IsNullOrEmpty(newPassword))
                {
                    errorOrNo = true;
                    resultDuplicatePerson.TextError += "Text box NewPassword is empty. Please fill in field NewPassword";
                }

                resultDuplicatePerson.Error = ErrorStatus.ArgumentNull;
                if (!errorOrNo)
                {
                    resultDuplicatePerson.TextError = "Duplicate Person completed successfully";
                    resultDuplicatePerson.Error = ErrorStatus.Success;
                    resultDuplicatePerson.IsSuccessfully = true;
                    resultDuplicatePerson.Payload = true;
                    Person newDuplicatePerson = Persons[idxPerson];
                    newDuplicatePerson.Login = newLogin;
                    newDuplicatePerson.Password = newPassword;
                    newDuplicatePerson.Id = Guid.NewGuid();
                    Persons.Add(newDuplicatePerson);
                }
                return resultDuplicatePerson;
            }
            resultDuplicatePerson.TextError += "Person is not found";
            resultDuplicatePerson.Error = ErrorStatus.NotFound;
            return resultDuplicatePerson;
        }

        public Result<bool> DuplicateLoan(Guid idLoan)
        {
            var resultDuplicateLoan = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idxLoan = RequestsLoanUser.FindIndex(x => x.Id.Equals(idLoan));
            if(idxLoan != -1)
            {
                resultDuplicateLoan.TextError = "Duplicate Loan completed successfully";
                resultDuplicateLoan.Error = ErrorStatus.Success;
                resultDuplicateLoan.IsSuccessfully = true;
                resultDuplicateLoan.Payload = true;
                var newLoan = RequestsLoanUser[idxLoan];
                newLoan.Id = Guid.NewGuid();
                RequestsLoanUser.Add(newLoan);
                return resultDuplicateLoan;
            }
            resultDuplicateLoan.TextError = "Loan is not found";
            resultDuplicateLoan.Error = ErrorStatus.NotFound;
            return resultDuplicateLoan;
        }
    }
}

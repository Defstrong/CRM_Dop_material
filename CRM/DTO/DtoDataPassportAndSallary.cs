using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DtoDataPassportAndSallary
    {
        public int Salary { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfResidence { get; set; }
        public string MaritalStatus { get; set; }
        public string PhoneNumber { get; set; }
        public bool SalaryReceived { get; set; }
        public DateTime SalaryPaymentDate { get; set; }
        public override string ToString()
        {
            return $"First name: {FirstName}\nLast name: {LastName}\nPatronymic: {Patronymic}\n" +
                $"Date of birth: {DateOfBirth.ToString("dd.MM.yyyy")}\nPlace of residnece: {PlaceOfResidence}\n" +
                $"Material status: {MaritalStatus}\nPhone number: {PhoneNumber}\nSalary: {Salary}";
        }
    }
}

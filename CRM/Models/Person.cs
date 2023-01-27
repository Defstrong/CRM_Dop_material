using DTO;  
using Enums;
using System.Runtime;

namespace Models
{
    public sealed class Person
    {
        public long[] BankAccount = new long[20];
        public Person(InputUserDto classInputUser, Roles role)
        {
            FirstName = classInputUser.FirstName;
            LastName = classInputUser.LastName;
            Patronymic = classInputUser.Patronymic;
            DateOfBirth = classInputUser.DateOfBirth;
            Age = age(DateOfBirth);
            Login = classInputUser.Login;
            Password = classInputUser.Password;
            Role = role;
            BankAccount[0] = 1000;
        }
        public Person() {
            BankAccount[0] = 1000;
        }
        public DtoDataPassportAndSallary DataPassportEmployeeAndSalary { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public Guid Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Roles Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CauseRefuseRegistration { get; set; }
        public StatusUser Status { get; set; }
        public ResponsibilityPerson Responsibility { get; set; }
        public static int age(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            int Age = today.Year - birthDate.Year;
            if (birthDate.AddYears(Age) > today)
                Age--;
            return Age;
        }
        public override string ToString()
        {
            return $"First Name: {FirstName}\nLast Name: {LastName}\nPatronymic:{Patronymic}\n" +
                    $"Age: {Age} \nDate of Birth: {DateOfBirth.ToString("dd.MM.yyyy")}\n";
        }
    }
}
﻿using DTO;  
using Enums;

namespace Models
{
    public sealed class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public string CauseRefuseRegistration { get; set; }
        public StatusUser Status { get; set; }
        public Person(InputUserDto classInputUser, Roles role)
        {
            string date;
            FirstName = classInputUser.FirstName;
            LastName = classInputUser.LastName;
            Patronymic = classInputUser.Patronymic;
            DateOfBirth = classInputUser.DateOfBirth;
            Age = age(DateOfBirth);
            Login = classInputUser.Login;
            Password = classInputUser.Password;
            Role = role;
        }
        public Person() {}
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
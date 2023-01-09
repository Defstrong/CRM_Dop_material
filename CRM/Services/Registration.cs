using CRM.DTO;
using CRM.Models;
using CRM.Enums;

namespace CRM.Services
{
    public abstract class Registration
    {
        public static List<Person> PersonRegistration;
        public Registration(List<Person> persons) =>
            PersonRegistration = persons;
        public virtual void RegistrationPerson(InputUserDto data) { }
    }

    public class AdminRegistration : Registration
    {
        public static List<Person> Persons;
        public AdminRegistration(List<Person> person):base(Persons) 
        {
            Persons = person;
        }

        public override void RegistrationPerson(InputUserDto data)
            => Persons.Add(new Person(data, Roles.Admin) 
                { Status = StatusUser.Accepted, Id = Guid.NewGuid() });
    }


    public class UserRegistration : Registration
    {
        public static List<Person> Persons;
        public UserRegistration(List<Person> persons) : base(PersonRegistration) =>
            Persons = persons;

        public override void RegistrationPerson(InputUserDto data)
            => Persons.Add(new Person(data, Roles.User) 
                { Status = StatusUser.Pending, Id = Guid.NewGuid() });
    }


    public class ManagerRegistration : Registration
    {
        public static List<Person> Persons;
        public ManagerRegistration(List<Person> persons) : base(PersonRegistration)
        {
            Persons = persons;
        }
        public override void RegistrationPerson(InputUserDto data)
            => Persons.Add(new Person(data, Roles.Manager) 
                { Status = StatusUser.Accepted, Id = Guid.NewGuid() });
    }


    public class ModeratorRegistration : Registration
    {
        public static List<Person> Persons;
        public ModeratorRegistration(List<Person> persons) : base(PersonRegistration) =>
            Persons = persons;
        public override void RegistrationPerson(InputUserDto data)
            => Persons.Add(new Person(data, Roles.Moderator) 
                { Status = StatusUser.Accepted, Id = Guid.NewGuid() });
    }
}

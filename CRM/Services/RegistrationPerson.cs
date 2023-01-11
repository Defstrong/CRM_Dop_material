using DTO;
using Models;
using Enums;
using AbstractClasses;

namespace Services
{
    public class AdminRegistration : Registration
    {
        public AdminRegistration(List<Person> person) : base(PersonRegistration) { }

        public override void RegistrationPerson(InputUserDto data)
            => PersonRegistration.Add(new Person(data, Roles.Admin) 
                { Status = StatusUser.Accepted, Id = Guid.NewGuid() });
    }


    public class UserRegistration : Registration
    {
        public UserRegistration(List<Person> persons) : base(persons) { }

        public override void RegistrationPerson(InputUserDto data)
            => PersonRegistration.Add(new Person(data, Roles.User) 
                { Status = StatusUser.Pending, Id = Guid.NewGuid() });
    }


    public class ManagerRegistration : Registration
    {
        public ManagerRegistration(List<Person> persons) : base(persons) { }
        public override void RegistrationPerson(InputUserDto data)
            => PersonRegistration.Add(new Person(data, Roles.Manager) 
                { Status = StatusUser.Accepted, Id = Guid.NewGuid() });
    }


    public class ModeratorRegistration : Registration
    {
        public ModeratorRegistration(List<Person> persons) : base(persons) { }
        public override void RegistrationPerson(InputUserDto data)
            => PersonRegistration.Add(new Person(data, Roles.Moderator) 
                { Status = StatusUser.Accepted, Id = Guid.NewGuid() });
    }
}

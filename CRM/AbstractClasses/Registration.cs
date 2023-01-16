using DTO;
using Models;

namespace AbstractClasses
{
    public abstract class Registration
    {
        public static List<Person> PersonRegistration;
        public Registration(List<Person> persons) =>
            PersonRegistration = persons;
        public abstract Result<bool> RegistrationPerson(InputUserDto data);
    }
}

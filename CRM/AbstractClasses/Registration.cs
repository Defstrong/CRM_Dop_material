using DTO;
using Models;

namespace AbstractClasses
{
    public abstract class Registration
    {
        public static List<Person> PersonRegistration;
        public Registration(List<Person> persons) =>
            PersonRegistration = persons;
        public virtual void RegistrationPerson(InputUserDto data) { }
    }
}

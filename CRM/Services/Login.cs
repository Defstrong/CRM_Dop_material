using CRM.DTO;
using CRM.Enums;
using CRM.Models;

namespace CRM.Services
{
    public class Login
    {
        public List<Person> Persons;
        public Login(List<Person> persons) {
            Persons = persons;
        }
        public Guid LoginPerson(InputUserDto dtoLogin)
        {
            int idx = Persons.FindIndex(x => x.Login.Equals(dtoLogin.Login) && x.Password.Equals(dtoLogin.Password));
            if (Persons[idx].Status == StatusUser.Accepted && idx != -1)
            {
                dtoLogin.Role = Persons[idx].Role;
            }
            else if (Persons[idx].Status == StatusUser.Refuse)
                throw new Exception(Persons[idx].CauseRefuseRegistration);

            if (idx != -1)
                return Persons[idx].Id;
            else 
                throw new Exception("Eror 404\nUser is not found");
        }
    }
}

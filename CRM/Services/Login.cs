using DTO;
using Enums;
using Models;

namespace Services
{
    public class Login
    {
        private readonly List<Person> Persons;
        public Login(List<Person> persons) {
            Persons = persons;
        }
        public Result<Person> LoginPerson(InputUserDto dtoLogin)
        {
            int idx = -1;
            Result<Person> result = new Result<Person>();
            if(string.IsNullOrEmpty(dtoLogin.Login))
                result.TextError += "Text box Login is empty. Please fill in field Login\n";
            if (string.IsNullOrEmpty(dtoLogin.Password))
                result.TextError += "Text box Password is empty. Please fill in field Password\n";
            else
                idx = Persons.FindIndex(x => x.Login.Equals(dtoLogin.Login) && x.Password.Equals(dtoLogin.Password));


            if(idx != -1)
            {
                if (Persons[idx].Status == StatusUser.Accepted)
                {
                    dtoLogin.Role = Persons[idx].Role;
                    result.Error = ErrorStatus.Success;
                    result.TextError += "Login is successfully\n";
                    result.Payload = Persons[idx];
                    result.IsSuccessfully = true;
                    return result;
                }
                else if(Persons[idx].Status == StatusUser.Block)
                {
                    result.Error = ErrorStatus.ServiceNotAvailable;
                    result.TextError = "Sorry but Admin blocked you\n";
                }
                else if (Persons[idx].Status == StatusUser.Refuse)
                {
                    result.TextError += Persons[idx].CauseRefuseRegistration;
                }
                else
                {
                    result.Error = ErrorStatus.NotFound;
                    result.TextError += "Person not found\n";
                }
            }

            result.IsSuccessfully = false;
            return result;
        }
    }
}

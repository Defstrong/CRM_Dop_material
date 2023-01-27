using Models;
using Enums;

namespace Services
{
    public sealed class ModeratorServices
    {
        private readonly List<Person> Persons;
        public ModeratorServices(List<Person> persons)
        {
            Persons = persons;
        }
        public Result<bool> ChoiceRequest(Guid idUser, StatusUser choice)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(idUser));
            Result<bool> result;
            if(choice != StatusUser.Accepted && choice != StatusUser.Refuse)
            {
                result = new Result<bool> { IsSuccessfully = false, Payload = false };
                if (idUser == Guid.Empty)
                    result.TextError += "\nText box IdUser is empty. Please fill in the field IdUesr";
                else if(idx == -1 && string.IsNullOrEmpty(idUser.ToString()) != false)
                    result.TextError += "\nUser is not found. Please try again";

                if (string.IsNullOrEmpty(choice.ToString()))
                {
                    result.Error = ErrorStatus.ArgumentNull;
                    result.TextError += "\nText box Choice is empty. Please fill in the field Choice";
                }
                else
                {
                    result.Error = ErrorStatus.InvalidCommand;
                    result.TextError += "\nInvalid command in text box Choice\n";
                }
                result.TextError += "\nPlease try again\n";
            }
            else
            {
                result = new Result<bool> { Error = ErrorStatus.Success, IsSuccessfully = true, Payload = true };
                if (choice == StatusUser.Accepted)
                {
                    Persons[idx].Status = choice;
                    result.TextError = "\nRequest successfully approved\n";
                }
                else
                {
                    Persons[idx].Status = choice;
                    result.TextError = "\nRequest successfully denied\n";
                }
            }

            return result;
        }
    }
}

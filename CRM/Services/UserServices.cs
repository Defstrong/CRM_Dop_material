using DTO;
using Models;
using Enums;

namespace Services
{
    public sealed class UserServices
    {
        private readonly List<Person> Persons;
        public UserServices(List<Person> persons) =>
            Persons = persons;

        public Result<bool> DeleteProfile(Guid id)
        {
            var resultDeleteProfile = new Result<bool>() { IsSuccessfully = false, Payload = false };
            int idx = Persons.FindIndex(x => x.Id.Equals(id));
            if(idx != -1)
            {
                resultDeleteProfile.Payload = true;
                resultDeleteProfile.IsSuccessfully = true;
                resultDeleteProfile.Error = ErrorStatus.Success;
                resultDeleteProfile.TextError = "Delete profile completed successfully";
                Persons.Remove(Persons[idx]);
                return resultDeleteProfile;
            }
            resultDeleteProfile.Error = ErrorStatus.NotFound;
            resultDeleteProfile.TextError = "Profile is not found";
            return resultDeleteProfile;
            
        }

        public Result<bool> EditProfile(DtoEditUser dtoEditUser)
        {
            var resultEditPerson = new Result<bool>();
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(dtoEditUser.Id));
            if (idxPerson != -1)
            {
                resultEditPerson.TextError = "Edit person completed successfuly";
                resultEditPerson.IsSuccessfully = true;
                resultEditPerson.Error = ErrorStatus.Success;
                resultEditPerson.Payload = true;

                if (!string.IsNullOrEmpty(dtoEditUser.FirstName))
                    Persons[idxPerson].FirstName = dtoEditUser.FirstName;
                if (!string.IsNullOrEmpty(dtoEditUser.LastName))
                    Persons[idxPerson].LastName = dtoEditUser.LastName;
                if (!string.IsNullOrEmpty(dtoEditUser.Patronymic))
                    Persons[idxPerson].Patronymic = dtoEditUser.Patronymic;
                if (!string.IsNullOrEmpty(dtoEditUser.Age.ToString()))
                    Persons[idxPerson].Age = dtoEditUser.Age;
                if (!string.IsNullOrEmpty(dtoEditUser.Login))
                    Persons[idxPerson].Login = dtoEditUser.Login;
                if (!string.IsNullOrEmpty(dtoEditUser.Password))
                    Persons[idxPerson].Password = dtoEditUser.Password;
                return resultEditPerson;
            }
            resultEditPerson.TextError = "User is not found";
            resultEditPerson.IsSuccessfully = false;
            resultEditPerson.Error = ErrorStatus.NotFound;
            resultEditPerson.Payload = false;
            return resultEditPerson;
        }

        public Person PersonalArea(Guid idUser)
        {
            int idxUser = Persons.FindIndex(x => x.Id.Equals(idUser));
            if(idxUser != -1)
                return Persons[idxUser];
            return null;
        }
    }
}

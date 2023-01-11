using DTO;
using Enums;
using Models;

namespace Services
{
    public sealed class UserServices
    {
        private static List<Person> Persons;
        public UserServices(List<Person> persons) =>
            Persons = persons;

        public void DeleteProfile(Guid id)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(id));
            Persons.Remove(Persons[idx]);
        }

        public void EditProfile(DtoEditUser editUser)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(editUser.Id));
            if (!string.IsNullOrEmpty(editUser.FirstName))
                Persons[idx].FirstName = editUser.FirstName;
            if (!string.IsNullOrEmpty(editUser.LastName))
                Persons[idx].LastName = editUser.LastName;
            if (!string.IsNullOrEmpty(editUser.Patronymic))
                Persons[idx].Patronymic = editUser.Patronymic;
            if (!string.IsNullOrEmpty(editUser.Age.ToString()))
                Persons[idx].Age = editUser.Age;
            if (!string.IsNullOrEmpty(editUser.Login))
                Persons[idx].Login = editUser.Login;
            if (!string.IsNullOrEmpty(editUser.Password))
                Persons[idx].Password = editUser.Password;
        }
    }
}

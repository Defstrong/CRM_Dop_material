using DTO;
using Enums;
using Models;

namespace Services
{
    public sealed class AdminServices
    {
        private static List<Person> Persons;
        public AdminServices(List<Person> persons, List<Loan> requestsLoanUser, List<Massage> massages)
        {
            Persons = persons;
        }

        public void EditPerson(DtoEditUser dtoEditUser)
        {
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(dtoEditUser.Id));
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
        }

        public void DeletePerson(Guid idPerson)
        {
            int idx = Persons.FindIndex(x => x.Id.Equals(idPerson));
            Persons.Remove(Persons[idx]);
        }

        public void BlockPerson(Guid idPerson)
        {
            int idxPerson = Persons.FindIndex(x => x.Id.Equals(idPerson));
            Persons[idxPerson].Status = StatusUser.Block;
        }
    }
}

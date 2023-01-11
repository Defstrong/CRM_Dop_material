using Models;
using Enums;

namespace Services
{
    public sealed class ModeratorServices
    {
        private static List<Person> Persons;
        public ModeratorServices(List<Person> persons)
        {
            Persons = persons;
        }
        public void ChoiceRequest(int idx, string choice)
        {
            if (choice == "Accepted")
                Persons[idx].Status = StatusUser.Accepted;
            else
                Persons[idx].Status = StatusUser.Refuse;
        }
    }
}

using Enums;
using Models;

namespace DTO
{
    public sealed class Write
    {
        public void WriteDutys(string duty) =>
            Console.WriteLine(duty);
        public void WriteUserData(Person user) =>
            Console.WriteLine(user.ToString());
        public void WriteUserRequest(Loan userRequest) =>
            Console.WriteLine(userRequest.ToString());
        public void ChoiceAtribut(ref string atribut, ref string change)
        {
            Console.Write("Enter atribut: ");
            atribut = Console.ReadLine();
            Console.Write("Enter replacement: ");
            change = Console.ReadLine();
        }
        public void Choice(ref Guid Id, ref string Choice)
        {
            Console.Write("Enter your choice: ");
            Choice = Console.ReadLine();
            Console.Write("Enter user id: ");
            string str = Console.ReadLine();
            if(!string.IsNullOrEmpty(str))
                Id = Guid.Parse(str);
        }
        public void ChoiceCreator(ref string Name)
        {
            Console.Write("Enter user name: ");
            Name = Console.ReadLine();
        }
        public void Status(StatusLoan status) =>
            Console.WriteLine("\n\t"+status);
        public void ChoiceCreator(ref Guid Id)
        {
            Console.Write("Enter user id: ");
            Id = Guid.Parse(Console.ReadLine());
        }
    }
}

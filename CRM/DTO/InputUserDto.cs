using Enums;

namespace DTO
{
    public sealed class InputUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int AmountMoney { get; set; }
        public DateTime Payday { get; set; }
        public Roles Role { get; set; }
    }
}

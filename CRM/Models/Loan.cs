using Enums;

namespace Models
{
    public sealed class Loan
    {
        public Guid Id { get; set; }
        public Guid IdSender { get; set; }
        public int CountMoney { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public StatusLoan StatusDuty { get; set; } = StatusLoan.Pending;
        public DateTime Payday { get; set; }
        public override string ToString()
        {
            return $"Name: {Name}\nAge: {Age}\nPayday: {Payday.ToString("dd.MM.yyyy")}\nAmount Money: {CountMoney}$";
        }
    }
}

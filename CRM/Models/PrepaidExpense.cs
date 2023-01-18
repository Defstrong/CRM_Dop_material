using Enums;

namespace Models
{
    public class PrepaidExpense
    {
        public Guid Id { get; set; }
        public Guid IdEmloyee { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string ReasonForTheAdvance { get; set; }
        public int CountMonths { get; set; }
        public StatusRequestForAdvance Status { get; set; } = StatusRequestForAdvance.Pending;
    }
}

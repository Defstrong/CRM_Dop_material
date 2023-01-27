using Models;
namespace DTO
{
    public class MoneyTransfer
    {
        public Guid IdReceiver { get; set; }
        public Guid IdSender { get; set; }
        public int AccountNumber { get; set; }
        public long CountMoney { get; set; }
    }
}

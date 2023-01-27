using Models;

namespace Interfaces
{
    public interface IFindingAnAvailableAccount
    {
        public int FindingAnAvailableAccount(Person person, long countMoney);
    }
}

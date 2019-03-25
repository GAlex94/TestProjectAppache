
namespace TestProjectAppache
{
    public interface IMoneyListener
    {
        void OnMoneyChange(int newMoney, int oldMoney);
    }
}
using OneBeyondApi.Model;
using OneBeyondApi.ViewModel;
namespace OneBeyondApi.DataAccess
{
    public interface IOnLoanRepository
    {
        public IReadOnlyList<OnLoanViewModel> GetOnLoans();
    }
}

using OneBeyondApi.Model;
using OneBeyondApi.Request;
using OneBeyondApi.ViewModel;
namespace OneBeyondApi.DataAccess
{
    public interface IOnLoanRepository
    {
        public IReadOnlyList<OnLoanViewModel> GetOnLoans();
        public string OnLoanBookReturning(OnLoanBookReturningRequest request);
    }
}

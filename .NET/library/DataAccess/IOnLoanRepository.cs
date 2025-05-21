using OneBeyondApi.Model;
using OneBeyondApi.Request;
using OneBeyondApi.ViewModel;
namespace OneBeyondApi.DataAccess
{
    public interface IOnLoanRepository
    {
        public IReadOnlyList<OnLoanViewModel> GetOnLoans();
        public Task<string> OnLoanBookReturning(OnLoanBookReturningRequest request);
    }
}

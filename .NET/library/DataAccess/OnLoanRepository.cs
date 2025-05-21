using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;
using OneBeyondApi.ViewModel;

namespace OneBeyondApi.DataAccess
{
    public class OnLoanRepository : IOnLoanRepository
    {
        public OnLoanRepository()
        {
        }
        public IReadOnlyList<OnLoanViewModel> GetOnLoans()
        {
            using (var context = new LibraryContext())
            {


                IReadOnlyList<OnLoanViewModel> borrowersWithBooksOnLoan = context.Catalogue
                    .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OnLoanTo)
                    .AsEnumerable()
                    .Where(w => w.OnLoanTo != null)
                    .GroupBy(x => x.OnLoanTo)
                    .Select(s => new OnLoanViewModel(s.Key, s.Select(booksOnLoan => booksOnLoan.Book.Name))).ToList(); // this query can be potecially slow if data is not stored in memory it is stored on sql server.



                return borrowersWithBooksOnLoan;
            }
        }

    }
}

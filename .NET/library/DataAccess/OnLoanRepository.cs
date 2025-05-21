using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;
using OneBeyondApi.Request;
using OneBeyondApi.ViewModel;

namespace OneBeyondApi.DataAccess
{
    public class OnLoanRepository : IOnLoanRepository
    {
        public OnLoanRepository()
        {
        }
        // 1. Add an "On Loan" end point with functionality to get/query the details of all borrowers with active loans and the titles of books they have on loan.
        public IReadOnlyList<OnLoanViewModel> GetOnLoans()
        {
            using (var context = new LibraryContext())
            {


                IReadOnlyList<OnLoanViewModel> borrowersWithBooksOnLoan = context.Catalogue
                    .Include(x => x.Book)
                    .Include(x => x.OnLoanTo)
                    .AsEnumerable()
                    .Where(w => w.OnLoanTo != null)
                    .GroupBy(x => x.OnLoanTo)
                    .Select(s => new OnLoanViewModel(s.Key, s.Select(booksOnLoan => booksOnLoan.Book.Name))).ToList(); // this query can be potecially slow if data is not stored in memory it is stored on sql server. (includes works like this)



                return borrowersWithBooksOnLoan;
            }
        }
        // 2. Extend the "On Loan" end point to allow books on loan to be returned.
        // This is a new endpoint but as I think it is not possible to implement this in the previous get request. 
        public string OnLoanBookReturning(OnLoanBookReturningRequest request)
        {
            try
            {
                new List<string?> { request.BookName, request.BorrowerName }.ForEach(param =>
                {
                    if (string.IsNullOrWhiteSpace(param))
                        throw new ArgumentException("The following parameter is invalid: ", nameof(param));
                });

                using (var context = new LibraryContext())
                {
                    BookStock? bookReturned = context.Catalogue
                        .Include(x => x.Book)
                        .ThenInclude(x => x.Author)
                        .Include(x => x.OnLoanTo)
                        .AsEnumerable()
                        .FirstOrDefault(w => w.Book.Name == request.BookName && w.OnLoanTo != null && request.BorrowerName == w.OnLoanTo.Name);


                    if (bookReturned == null)
                        throw new Exception("The book is not found in stock or it is not on loan at this barrower.");

                    bookReturned.OnLoanTo = null;
                    bookReturned.LoanEndDate = null;

                    context.Catalogue.Update(bookReturned);
                    context.SaveChanges();
                }

                return request.BookName + " - book returned successfully by " + request.BorrowerName;
            }
            catch (Exception ex)
            {
                //There is always a problem with ex.
                return "There is an issue at returning: " + ex; 
            }    
            
        }

    }
}

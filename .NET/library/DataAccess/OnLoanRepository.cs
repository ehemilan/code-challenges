using System.Diagnostics.Eventing.Reader;
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
        public async Task<string> OnLoanBookReturning(OnLoanBookReturningRequest request)
        {
            try
            {
                new List<string?> { request.BookName, request.BorrowerName }.ForEach(param =>
                {
                    if (string.IsNullOrWhiteSpace(param))
                        throw new ArgumentException("The following parameter is invalid: ", nameof(param));
                });
                Guid? fineId;
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

                    //3. If books are returned after their loan end date then a fine should be raised against the borrower (data model for fines and relationships with borrowers are left to the candidate to define)
                    fineId = await IsBookReturnedInTime(bookReturned); //In the past to make very fast a looped deletion, or a check function, I not waited this tread. At some cases this is a good and fully work trick in practice.
                                                                                        //If would be an another way to check the fines, in this case I return a message so endpoint needs to wait it. 

                    bookReturned.OnLoanTo = null;
                    bookReturned.LoanEndDate = null;

                    context.Catalogue.Update(bookReturned);
                    context.SaveChanges();
                }
               
                return fineId == null ? 
                    request.BookName + " - book returned successfully by " + request.BorrowerName : 
                    request.BorrowerName + " must pay a fine because not returned " + request.BookName + " in time. Fine created with id: " + fineId;
            }
            catch (Exception ex)
            {
                //There is always a problem with ex.
                return "There is an issue at returning: " + ex; 
            }    
            
        }

        // in this case this function is not long but as I experienced better to start a new function for logically separated funcionalities as a tree. 
        private async Task<Guid?> IsBookReturnedInTime(BookStock bookReturned)
        {
            //In an international project or in a cloud project need to define well what timezone are in use. In this case I used local time as in SeedData.cs
            //It needs to be specified that the day when it is ended is in range or not. Curently afternoon is late to move back.
            DateTime currentDate = DateTime.Now;
            
            if (bookReturned.LoanEndDate == null)
                throw new Exception("The book loan end date is null.");

            //if someone delete  w.OnLoanTo != null from the base query
            if (bookReturned.OnLoanTo == null)
                throw new Exception("OnLoanTo param is null.");
            

            // Add fine to borrower
            if (bookReturned.LoanEndDate <= currentDate)
            {
                Fine newFine = new Fine
                {
                    Book = bookReturned.Book,
                    Borrower = bookReturned.OnLoanTo,
                    ReturnedOn = currentDate,
                    LoanEndDate = bookReturned.LoanEndDate.Value

                };

                using (var context = new LibraryContext())
                {
                    context.Fines.Add(newFine);
                    return newFine.Id;
                }

            }

            return null;
            
        }

    }
}

using OneBeyondApi.Model;
namespace OneBeyondApi.ViewModel
{
    public class OnLoanViewModel
    {
        public BorrowerViewModel Borrower { get; set; }
        public IEnumerable<string> BookTitlesOnLoans { get; set; }

        public OnLoanViewModel(Borrower borrower, IEnumerable<string> bookTitlesOnLoans) { 
            Borrower = new BorrowerViewModel (borrower);
            BookTitlesOnLoans = bookTitlesOnLoans;
        }
    }
    public class BorrowerViewModel
    {

        public string Name { get; set; }
        public string EmailAddress { get; set; }
        internal BorrowerViewModel (Borrower borrower)
        {
            Name = borrower.Name;
            EmailAddress = borrower.EmailAddress;
        }
    }
}

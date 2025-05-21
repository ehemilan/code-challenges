using OneBeyondApi.Model;

namespace OneBeyondApi.ViewModel
{
    public class OnLoanQueueViewModel
    {
        public Borrower Borrower { get; set; }
        public Book Book { get; set; }
        public DateTime EstimatedEndDate { get; set; }

        public OnLoanQueueViewModel(Borrower borrower, Book book, DateTime est)
        {
            Borrower = borrower;
            Book = book;
            EstimatedEndDate = est;
        }
    }
}

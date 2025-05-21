namespace OneBeyondApi.Model
{
    public class OnLoanQueue
    {
        public long Id { get; set; }
        public Borrower Borrower { get; set; }

        public Book Book { get; set; }
    }
}

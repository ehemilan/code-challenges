namespace OneBeyondApi.Model
{
    public class Fine
    {
            public Guid Id { get; set; }
            public Borrower Borrower { get; set; }
            
            public Book Book { get; set; }

            public DateTime ReturnedOn { get; set; }

            public DateTime LoanEndDate { get; set; }

    }
}

namespace OneBeyondApi.Request
{
    public class JoinOnLoanQueueById
    {
        public Guid? BookId { get; set; }
            
        public Guid? BorrowerId { get; set; }
    }
}

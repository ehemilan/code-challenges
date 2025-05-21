using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using OneBeyondApi.Request;
using OneBeyondApi.ViewModel;
using System.Collections;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnLoanController : ControllerBase
    {
        private readonly ILogger<OnLoanController> _logger;
        private readonly IOnLoanRepository _onLoanRepository;

        public OnLoanController(ILogger<OnLoanController> logger, IOnLoanRepository onLoanRepository)
        {
            _logger = logger;
            _onLoanRepository = onLoanRepository;
        }


        [HttpGet]
        [Route("GetOnLoan")]
        public IReadOnlyList<OnLoanViewModel> GetOnLoans()
        {
            return _onLoanRepository.GetOnLoans();
        }

        // All books need different name to work my solution. There are a different way in my head. We can use book Id but it is ugly and unreal solution. 
        [HttpPost]
        [Route("OnLoanBookReturning")]
        public Task<string> OnLoanBookReturning([FromBody] OnLoanBookReturningRequest request)
        {
            return _onLoanRepository.OnLoanBookReturning(request);
        }

        // All books need different name to work my solution. There are a different way in my head. We can use book Id but it is ugly and unreal solution. 
        [HttpPost]
        [Route("JoinOnLoanQueue")]
        public string JoinOnLoanQueue([FromBody] JoinOnLoanRequest request)
        {
            return _onLoanRepository.JoinOnLoanQueue(request);
        }

        [HttpGet]
        [Route("GetLoanQueue")]

        public IReadOnlyList<OnLoanQueueViewModel> GetLoanQueue(string? bookName)
        {
            return _onLoanRepository.GetLoanQueue(bookName);
        }


        [HttpPost]
        [Route("OnLoanBookReturningById")]
        public Task<string> OnLoanBookReturningById([FromBody] Guid? bookId)
        {
            return _onLoanRepository.OnLoanBookReturningById(bookId);
        }

        [HttpPost]
        [Route("JoinOnLoanQueueById")]
        public string JoinOnLoanQueueById([FromBody] JoinOnLoanQueueById request)
        {
            return _onLoanRepository.JoinOnLoanQueueById( request.BookId, request.BorrowerId);
        }

        [HttpGet]
        [Route("GetLoanQueueById")]

        public IReadOnlyList<OnLoanQueueViewModel> GetLoanQueueById(Guid? bookId)
        {

            return _onLoanRepository.GetLoanQueueById(bookId);

        }
    }
}
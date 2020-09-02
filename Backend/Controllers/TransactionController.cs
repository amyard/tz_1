using AutoMapper;
using Backend.Dtos;
using Backend.Errors;
using Backend.Helpers;
using Backend.Interfaces;
using Backend.Models;
using Backend.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<TransactionMDDto>>> GetAllTransactions([FromQuery] TransactionFilterModel transFilter)
        {
            return Ok(await _transactionService.GetTransactionsWithFiltersAsync(transFilter));
        }

        [HttpGet("{transactionId}")]
        public async Task<ActionResult<TransactionMDDto>> GetTransaction(int transactionId)
        {
            var trans = await _transactionService.GetTransactionByIdAsync(transactionId);

            if(trans == null) return NotFound(new ApiResponse(404));
        
            return _mapper.Map<TransactionMD, TransactionMDDto>(trans);
        }

        [HttpDelete("{transactionId}")]
        public async Task<ActionResult> DeleteTransactionAsync(int transactionId)
        {
            TransactionMD trans = await _transactionService.GetTransactionByIdAsync(transactionId);

            if(trans == null) return NotFound(new ApiResponse(404));

            await _transactionService.DeleteTransactionAsync(trans);

            return Ok(new ApiResponse(200, "Resource deleted successfully"));
        }

        [HttpPut("{transactionId}")]
        public async Task<ActionResult> UpdateTransactionAsync([FromForm] TransactionMDDto transDto)
        {
            int transId = int.Parse(HttpContext.Request.RouteValues["transactionId"].ToString());
            TransactionMD trans = await _transactionService.GetTransactionByIdAsync(transId);

            if (trans == null) return NotFound(new ApiResponse(404));

            await _transactionService.UpdateTransactionAsync(trans, transDto);

            return Ok(new ApiResponse(200, "Resource updated successfully"));
        }
    }
}

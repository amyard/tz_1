using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Backend.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly StoreContext _context;

        public TransactionController(StoreContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<TransactionMD>>> GetAllData()
        {
            var data = await _context.TransactionMDs.ToListAsync();

            return Ok(data);
        }
    }
}

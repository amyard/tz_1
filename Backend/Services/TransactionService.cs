using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly StoreContext _context;

        public TransactionService(StoreContext context)
        {
            _context = context;
        }

        public Task<bool> DeleteTransactionAsync(int transactionId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TransactionMD> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.TransactionMDs.FirstOrDefaultAsync(x=>x.TransactionId == transactionId);
        }

        public async Task<IReadOnlyList<TransactionMD>> GetTransactionsAsync()
        {
            return await _context.TransactionMDs.ToListAsync();
        }

        public Task<TransactionMD> UpdateTransactionAsync(TransactionMD entity)
        {
            throw new System.NotImplementedException();
        }
    }
}

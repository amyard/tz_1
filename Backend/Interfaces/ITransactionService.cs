using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionMD> GetTransactionByIdAsync(int transactionId);
        Task<IReadOnlyList<TransactionMD>> GetTransactionsAsync();
        Task<TransactionMD> UpdateTransactionAsync(TransactionMD entity);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}

using Backend.Dtos;
using Backend.Helpers;
using Backend.Models;
using Backend.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionMD> GetTransactionByIdAsync(int transactionId);
        Task<IReadOnlyList<TransactionMD>> GetTransactionsAsync();
        Task<Pagination<TransactionMDDto>> GetTransactionsWithFiltersAsync(TransactionFilterModel filters);
        Task<TransactionMD> UpdateTransactionAsync(TransactionMD entity);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}

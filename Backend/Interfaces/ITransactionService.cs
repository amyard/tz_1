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
        Task<IReadOnlyList<TransactionMDDto>> GetTransactionsWithFiltersDonwloadAsync(TransactionFilterDownloadModel filters);
        Task UpdateTransactionAsync(TransactionMD entity, TransactionMDDto transDto);
        Task DeleteTransactionAsync(TransactionMD transactionMD);
        Task CreateTransactionAsync(TransactionMD transactionMD);
        Task UpdateTransactionByIdAsync(TransactionMD transactionMD);
        bool CheckTransactionExists(int transactionId);
    }
}

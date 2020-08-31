using AutoMapper;
using Backend.Data;
using Backend.Dtos;
using Backend.Enums;
using Backend.Helpers;
using Backend.Interfaces;
using Backend.Models;
using Backend.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private int pageSize = 10;

        public TransactionService(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

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

        // TODO - remove extra ToList()
        public async Task<Pagination<TransactionMDDto>> GetTransactionsWithFiltersAsync(TransactionFilterModel filters)
        {
            var result = await _context.TransactionMDs.ToListAsync();

            if(!string.IsNullOrWhiteSpace(filters.Status))
            {
                TransactionStatus status = GetEnumStatusName(filters.Status);

                if (status != TransactionStatus.Default)
                { 
                    result = result.Where(x => x.Status == status).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(filters.Type))
            {
                TransactionType types = GetEnumTypeName(filters.Type);

                if (types != TransactionType.Default)
                {
                    result = result.Where(x => x.Type == types).ToList();
                }
            }

            // pagination
            var totalItems = result.Count();
            var pagesLast = (int)Math.Ceiling((double)totalItems / (double)pageSize);

            if(filters.Page > pagesLast) filters.Page = pagesLast;
            
            if(filters.Page == 0) filters.Page = 1;

            result = result.Skip((filters.Page - 1) * pageSize).Take(pageSize).ToList();
            var data = _mapper.Map<IReadOnlyList<TransactionMD>, IReadOnlyList<TransactionMDDto>>(result);
            bool previousPage = filters.Page-1 != 0 ? true : false;
            bool nextPage = filters.Page+1 > pagesLast ? false : true;

            return new Pagination<TransactionMDDto>(filters.Page, pagesLast, pageSize, totalItems, result.Count(), previousPage, nextPage, data);
        }


        public Task<TransactionMD> UpdateTransactionAsync(TransactionMD entity)
        {
            throw new System.NotImplementedException();
        }


        # region private methods
        private string GetCamelCaseString(string text)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        private TransactionType GetEnumTypeName(string typeName)
        {
            string typeCamelCase = GetCamelCaseString(typeName);

            if (typeCamelCase == "Withdrawal")
                return TransactionType.Withdrawal;
            else if (typeCamelCase == "Refill")
                return TransactionType.Refill;
            else
                return TransactionType.Default;
        }

        private TransactionStatus GetEnumStatusName(string status)
        {
            string statusCamelCase = GetCamelCaseString(status);

            if (statusCamelCase == "Pending")
                return TransactionStatus.Pending;
            else if (statusCamelCase == "Completed")
                return TransactionStatus.Completed;
            else if (statusCamelCase == "Cancelled")
                return TransactionStatus.Cancelled;
            else
                return TransactionStatus.Default;
        }

        # endregion
    }
}

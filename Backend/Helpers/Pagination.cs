using System.Collections.Generic;

namespace Backend.Helpers
{
    public class Pagination<T> where T: class
    {
        public Pagination(int page, int pages, int pageSize, int count, int itemsAmount, bool prevPage, bool nextPage, IReadOnlyList<T> data)
        {
            Page = page;
            AmountOfPages = pages;
            ItemsPerPage = pageSize;
            AmountOfItems = count;
            CurrenctItemsAmount = itemsAmount;
            PreviousPage = prevPage;
            NextPage = nextPage;
            Data = data;
        }

        public int Page {get;set;}
        public int AmountOfPages {get;set;}
        public int ItemsPerPage {get;set;}
        public int AmountOfItems {get;set;}
        public int CurrenctItemsAmount { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
        public IReadOnlyList<T> Data {get;set;}
    }
}
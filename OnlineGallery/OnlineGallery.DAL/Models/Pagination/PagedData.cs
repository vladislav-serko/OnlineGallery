using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineGallery.DAL.Models.Pagination
{
    public class PagedData<T>
    {
        private const int MaxItemCount = 50;
        public int PageCount { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemCount { get; set; }
        public List<T> Data { get; set; }

        public async Task<PagedData<T>> Create(IQueryable<T> queryable, PaginationOptions options)
        {
            if (options.ItemCount > MaxItemCount)
                ItemCount = MaxItemCount;
            else if (options.ItemCount < 1)
                ItemCount = 1;
            else
                ItemCount = options.ItemCount;

            var count = queryable.Count();
            TotalItems = count;

            PageCount = (int) Math.Ceiling((double) count / ItemCount);

            if (options.Page < 1 || PageCount == 0)
                CurrentPage = 1;
            else if (options.Page > PageCount)
                CurrentPage = PageCount;
            else
                CurrentPage = options.Page;

            Data = await queryable
                .Skip((CurrentPage - 1) * ItemCount)
                .Take(ItemCount)
                .ToListAsync();

            return this;
        }
    }
}
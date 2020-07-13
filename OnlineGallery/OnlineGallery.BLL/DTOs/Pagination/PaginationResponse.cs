using System.Collections.Generic;

namespace OnlineGallery.BLL.DTOs.Pagination
{
    public class PaginationResponse<T>
    {
        public int PageCount { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemCount { get; set; }
        public List<T> Data { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModelsDTO
{
    public class BaseEntityDTO
    {
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }

    public class Search1Param
    {
        public string? City { get; set; }
        public string? Currency { get; set; }
        public string? Ratebasis { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? Adults { get; set; }
        public string? Children { get; set; }
        public string? Rooms { get; set; }
        public string? Page { get; set; }
        public string? PageSize { get; set; }
    }

    public class SearchParam
    {
        public string City { get; set; }
        public string Currency { get; set; }
        public string Ratebasis { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Adults { get; set; }
        public string Children { get; set; }
        public string Rooms { get; set; }
        public string Page { get; set; }
        public string PageSize { get; set; }

        public SearchParam CloneWithPage(string newPage)
        {
            return new SearchParam
            {
                City = this.City,
                Currency = this.Currency,
                Ratebasis = this.Ratebasis,
                FromDate = this.FromDate,
                ToDate = this.ToDate,
                Adults = this.Adults,
                Children = this.Children,
                Rooms = this.Rooms,
                Page = newPage,
                PageSize = this.PageSize
            };
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_OnlineStore.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string Thumbnail { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public virtual Category Category { get; set; }
    }
}
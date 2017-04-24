using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CRUD_OnlineStore.Models
{
    public class CategoryViewModel
    {
        
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
      
        public string Thumbnail { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string ParentString { get; set; }
        public Nullable<int> TotalItems { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public virtual Category Category { get; set; }
    }
}
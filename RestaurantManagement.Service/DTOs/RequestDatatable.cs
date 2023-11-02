using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.UI.Areas.Admin.Models
{
    public class RequestDatatable
    {
        public int Draw { get; set; }
        [BindProperty(Name = "length")]
        public int PageSize { get; set; }
        [BindProperty(Name = "start")]
        public int SkipItems { get; set; }
        [BindProperty(Name = "search[value]")]
        public string Keyword { get; set; }
    }
}

namespace RestaurantManagement.Service.DTOs
{
    public class ResponseDatatable
    {
        public object[] Data { get; set; }
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}

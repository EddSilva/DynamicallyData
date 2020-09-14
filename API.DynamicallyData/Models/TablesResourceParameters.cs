namespace API.DynamicallyData.Models
{
    public class TablesResourceParameters
    {
        private const int maxPageSize = 20;
        private const int minPageNumber = 0;

        private int _pageSize = 5;
        private int _pageNumber = 0;

        public string TableName { get; set; } = string.Empty;
        
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = ( value <= minPageNumber) ? _pageNumber : value - 1;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}

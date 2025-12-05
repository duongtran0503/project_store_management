namespace StoreManagement.API.Common.Responses
{
    public class PaginationRequest
    {
        private const int MinPageSize = 1;
        private const int MaxPageSize = 20;




        private int _pageNumber = 1;

        public bool? All { get; set; }
        public int PageNumber
        {
            get => _pageNumber;

            set => _pageNumber = (value <= 0) ? 1 : value;
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {

                if (value <= 0)
                {
                    _pageSize = MinPageSize;
                }

                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }

                else
                {
                    _pageSize = value;
                }
            }
        }
    }
}

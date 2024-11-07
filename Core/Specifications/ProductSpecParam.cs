namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;

        // Default PageIndex is 1
        public int PageIndex { get; set; } = 1;

        // Default PageSize is 6, with a max size limit of 50
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Nullable fields allow optional query parameters
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        // Sort as string can be used to define sorting type (e.g., "asc", "desc")
        public string Sort { get; set; } = "name";  // Default to "name" sorting if not provided

        // Search term for filtering products, convert to lowercase to ensure case-insensitive search
        private string _search="";
        public string Search
        {
            get => _search;
            set => _search = string.IsNullOrEmpty(value) ? null : value.ToLower();  // Make null if empty
        }
    }
}

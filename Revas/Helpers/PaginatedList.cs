namespace Revas.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int ActivePage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }

        public bool HasPrevius { get => ActivePage > 1; }
        public bool HasNext { get => ActivePage < TotalPage; }


        public PaginatedList(List<T> values, int count, int page, int pageSize)
        {
            ActivePage = page;
            TotalPage = (int)Math.Ceiling((double)count / pageSize);
            PageSize = pageSize;
            AddRange(values);
        }

        public static PaginatedList<T> Create(IQueryable<T> query, int page, int pageSize)
        {
            return new PaginatedList<T>(query.Skip((page - 1) * pageSize).Take(pageSize).ToList(), query.Count(), page, pageSize);
        }

    }
}

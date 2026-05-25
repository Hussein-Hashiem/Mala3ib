
namespace Mala3ib.DAL.Abstraction
{
    public class PaginatedList<T> where T : class
    {
        public PaginatedList(List<T> items, int pageNumber, int count, int pageSize)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling((double)count / pageSize);
        }
        public List<T> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);

            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, pageNumber, count, pageSize);
        }
    }
}

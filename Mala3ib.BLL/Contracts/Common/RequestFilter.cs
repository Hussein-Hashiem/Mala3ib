namespace Mala3ib.BLL.Contracts.Common
{
    public record RequestFilter
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SearchValue { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; init; } = "ASC";
    }
}

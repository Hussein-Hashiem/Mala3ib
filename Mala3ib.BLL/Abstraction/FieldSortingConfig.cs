namespace Mala3ib.BLL.Abstraction
{
    public static class FieldSortingConfig
    {
        public static readonly Dictionary<string, string> ColumnMap = new()
        {
            ["PRICEPERHOUR"] = "PricePerHour",
            ["RATING"] = "Rating",
        };

        public const string DefaultColumn = "RATING";
    }
}

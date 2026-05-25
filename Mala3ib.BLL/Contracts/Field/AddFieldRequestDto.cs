namespace Mala3ib.BLL.Contracts.Field
{
    public record AddFieldRequestDto(
        string Name,
        string Location,
        decimal PricePerHour
    );
}
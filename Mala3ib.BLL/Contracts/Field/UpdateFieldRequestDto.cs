namespace Mala3ib.BLL.Contracts.Field
{
    public record UpdateFieldRequestDto(
        string Name,
        string Location,
        decimal PricePerHour
    );
}
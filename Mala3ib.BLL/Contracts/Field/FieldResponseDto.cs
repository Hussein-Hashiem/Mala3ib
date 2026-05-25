namespace Mala3ib.BLL.Contracts.Field
{
    public record FieldResponseDto(
        int Id,
        string Name,
        string Location,
        decimal PricePerHour,
        float Rating,
        List<GetFieldImages> Images
    );
}
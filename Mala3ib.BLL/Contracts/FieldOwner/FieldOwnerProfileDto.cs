namespace Mala3ib.BLL.Contracts.FieldOwner
{
    public record FieldOwnerProfileDto(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        DateOnly DateOfBirth,
        string? Image
    );
}
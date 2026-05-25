namespace Mala3ib.BLL.Contracts.FieldOwner
{
    public record UpdateFieldOwnerRequestDto(
        string FirstName,
        string LastName,
        string PhoneNumber,
        DateOnly DateOfBirth
    );
}
namespace Mala3ib.BLL.Contracts.FieldOwner
{
    public record RegisterFieldOwnerDto (
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string PhoneNumber,
        DateOnly DateOfBirth
    );
}

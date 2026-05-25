namespace Mala3ib.BLL.Contracts.Player
{
    public record UpdatePlayerRequestDto (
        string FirstName,
        string LastName,
        string PhoneNumber,
        DateOnly DateOfBirth
    );
}

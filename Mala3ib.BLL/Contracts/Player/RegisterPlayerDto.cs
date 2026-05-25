namespace Mala3ib.BLL.Contracts.Player
{
    public record RegisterPlayerDto(
        string Email,
        string Password, 
        string FirstName, 
        string LastName, 
        string PhoneNumber,
        DateOnly DateOfBirth
    );
}

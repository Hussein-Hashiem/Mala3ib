namespace Mala3ib.BLL.Contracts.Player
{
    public record PlayerProfileDto (
        string UserId,
        string Email, 
        string FirstName,
        string LastName,
        string PhoneNumber, 
        string? Image,
        DateOnly DateOfBirth,
        int FollowersCount,
        int FollowingCount,
        bool IsFollowing
    );
}

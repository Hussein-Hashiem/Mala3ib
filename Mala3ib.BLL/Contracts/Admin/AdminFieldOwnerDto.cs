using System.Text.Json.Serialization;

namespace Mala3ib.BLL.Contracts.Admin
{
    public record AdminFieldOwnerDto(
        int Id,
        string UserId,
        string Email,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        DateOnly DateOfBirth,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        Status Status
    );
}

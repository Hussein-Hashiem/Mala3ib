using System.Text.Json.Serialization;

namespace Mala3ib.BLL.Contracts.Invitation
{
    public record InvitationResponseDto(
        int SenderId,
        int RecieverId,
        int FieldSlotId,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        InvitationStatus Status,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        InvitationType Type
    );
}
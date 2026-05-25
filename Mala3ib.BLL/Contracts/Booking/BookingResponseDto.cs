using System.Text.Json.Serialization;

namespace Mala3ib.BLL.Contracts.Booking
{
    public record BookingResponseDto(
        int Id,
        DateTime Date,
        int FieldSlotId,
        int PlayerId,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        BookingStatus Status
    );
}

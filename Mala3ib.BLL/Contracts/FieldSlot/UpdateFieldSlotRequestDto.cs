namespace Mala3ib.BLL.Contracts.FieldSlot
{
    public record UpdateFieldSlotRequestDto(
        DateTime StartDate,
        DateTime EndDate,
        int MaxPlayers
    );
}
namespace Mala3ib.BLL.Contracts.FieldSlot
{
    public record AddFieldSlotRequestDto(
        DateTime StartDate,
        DateTime EndDate,
        int MaxPlayers
    );
}
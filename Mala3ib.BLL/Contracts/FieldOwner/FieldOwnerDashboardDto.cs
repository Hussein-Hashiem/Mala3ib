namespace Mala3ib.BLL.Contracts.FieldOwner
{
    public record FieldOwnerDashboardDto(
        int TotalFields,
        int AvailableSlotsToday,
        int BookedSlotsToday
    );
}

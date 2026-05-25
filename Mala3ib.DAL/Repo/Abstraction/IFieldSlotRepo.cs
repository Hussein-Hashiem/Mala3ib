namespace Mala3ib.DAL.Repo.Abstraction
{
    public interface IFieldSlotRepo
    {
        IQueryable<FieldSlot> GetByFieldId(int fieldId);
        IQueryable<FieldSlot> GetAvailableSlots(int fieldId, DateTime day);
        IQueryable<FieldSlot> GetById(int id);
        Task<bool> IsSlotAvailableAsync(int fieldId, DateTime start, DateTime end, int? excludeSlotId = null, CancellationToken cancellation = default);
        Task AddAsync(FieldSlot fieldSlot);
        Task AddPlayerToSlotAsync(int fieldSlotId, int playerId, bool isCaptain = false, CancellationToken cancellation = default);
        Task<bool> IsPlayerInSlotAsync(int fieldSlotId, int playerId, CancellationToken cancellation = default);
        Task ClearPlayersFromSlotAsync(int fieldSlotId, CancellationToken cancellation = default);
        Task UpdateBookedStatusAsync(int fieldSlotId, bool isBooked, CancellationToken cancellation = default);
        Task<bool> IsExist(int id, CancellationToken cancellation = default);
        Task UpdateAsync(int id, FieldSlot request, CancellationToken cancellation = default);
        Task DeleteAsync(int id, CancellationToken cancellation = default);
    }
}
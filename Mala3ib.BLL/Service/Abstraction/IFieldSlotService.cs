using Mala3ib.BLL.Contracts.FieldSlot;

namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFieldSlotService
    {
        Task<Result> AddAsync(AddFieldSlotRequestDto request, string userId, int fieldId, CancellationToken cancellation = default);
        Task<Result> UpdateAsync(int id, UpdateFieldSlotRequestDto request, string userId, int fieldId, CancellationToken cancellation = default);
        Task<Result> DeleteAsync(int id, int fieldId, string userId, CancellationToken cancellation = default);
        Task<Result<IEnumerable<FieldSlotResponseDto>>> GetAvailableSlots(int fieldId, DateTime? date, CancellationToken cancellation = default);
        Task<Result<FieldSlotResponseDto>> GetByIdAsync(int id, CancellationToken cancellation = default);
        Task<Result<IEnumerable<FieldSlotResponseDto>>> GetByFieldIdAsync(int fieldId, CancellationToken cancellation = default);
    }
}
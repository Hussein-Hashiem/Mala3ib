namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFieldService
    {
        Task<Result<FieldResponseDto>> AddAsync(AddFieldRequestDto request, string userId, CancellationToken cancellation = default);
        Task<Result<PaginatedList<FieldResponseDto>>> GetAllAsync(RequestFilter filter, CancellationToken cancellation = default);
        Task<Result<FieldResponseDto>> GetByIdAsync(int id, CancellationToken cancellation = default);
        Task<Result<IEnumerable<FieldResponseDto>>> GetByOwnerIdAsync(string ownerId, CancellationToken cancellation = default);
        Task<Result> UpdateAsync(int id, UpdateFieldRequestDto request, string userId, CancellationToken cancellation = default);
        Task<Result> DeleteAsync(int id, string userId, CancellationToken cancellation = default);
        Task<Result> UploadImagesAsync(int id, string userId, IFormFileCollection images, CancellationToken cancellation = default);
        Task<Result> DeleteImageAsync(int fieldId, string userId, int imageId, CancellationToken cancellation);
    }
}
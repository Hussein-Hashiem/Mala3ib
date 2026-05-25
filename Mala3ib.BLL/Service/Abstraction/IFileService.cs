namespace Mala3ib.BLL.Service.Abstraction
{
    public interface IFileService
    {
        Task UploadImageAsync(string userId, IFormFile image, CancellationToken cancellation = default);
        Task DeleteProfileImageAsync(string userId, CancellationToken cancellationToken = default);
    }
}

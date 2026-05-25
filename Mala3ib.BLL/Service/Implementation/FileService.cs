namespace Mala3ib.BLL.Service.Implementation
{
    public class FileService : IFileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task UploadImageAsync(string userId, IFormFile image, CancellationToken cancellation = default)
        {
            var _imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var path = Path.Combine(_imagesPath, fileName);

            using var stream = File.Create(path);

            await image.CopyToAsync(stream, cancellation);

            var user = await _userManager.FindByIdAsync(userId);

            if(!string.IsNullOrEmpty(user!.Image))
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, user.Image);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            user!.Image = $"images/{fileName}";

            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteProfileImageAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(user!.Image))
                return;

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, user.Image);

            if (File.Exists(fullPath))
                    File.Delete(fullPath);

            user!.Image = null;

            await _userManager.UpdateAsync(user);
        }
    }
}

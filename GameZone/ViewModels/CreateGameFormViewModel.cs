
namespace GameZone.ViewModels
{
    public class CreateGameFormViewModel:GameFormViewModel
    {
        [AllowedExtension(FileSettings.AllowedExtensions)]
        [AllowedFileSize(FileSettings.MaxFileSizeInMB)]
        public IFormFile Cover { get; set; } = default!;
    }
}

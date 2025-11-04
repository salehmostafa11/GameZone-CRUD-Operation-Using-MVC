namespace GameZone.ViewModels
{
    public class EditGameFormViewModel : GameFormViewModel
    {
        public int Id { get; set; }
        public string? CurrentCover { get; set; }
        [AllowedExtension(FileSettings.AllowedExtensions)]
        [AllowedFileSize(FileSettings.MaxFileSizeInMB)]
        public IFormFile? Cover { get; set; } = default!;
    }
}

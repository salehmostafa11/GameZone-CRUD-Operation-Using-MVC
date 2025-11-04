namespace GameZone.ViewModels
{
    public class GameFormViewModel
    {
        [MaxLength(70)]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Supported Devices")]
        public List<int> SelectedDevices { get; set; } = default!;
        //عشان نعرضله الليست اللي هيختار منها
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Devices { get; set; } = Enumerable.Empty<SelectListItem>();

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;
    }
}

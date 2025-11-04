namespace GameZone.Services.IServices
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetAllAsSelectList();
    }
}

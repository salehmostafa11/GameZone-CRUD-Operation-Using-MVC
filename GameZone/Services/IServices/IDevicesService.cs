namespace GameZone.Services.IServices
{
    public interface IDevicesService
    {
        IEnumerable<SelectListItem> GetAllAsSelectList();
    }
}

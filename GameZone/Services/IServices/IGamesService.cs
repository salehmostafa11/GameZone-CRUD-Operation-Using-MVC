namespace GameZone.Services.IServices
{
    public interface IGamesService
    {
        Task Create (CreateGameFormViewModel game);
        IEnumerable<Game> GetAllWithCategory();
        Game? GetById(int id);
        Task<Game?> Edit(EditGameFormViewModel model);
        bool Delete(int id);
    }
}

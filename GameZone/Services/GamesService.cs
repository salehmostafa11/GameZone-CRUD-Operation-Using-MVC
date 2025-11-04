namespace GameZone.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // to get wwwroot
        private readonly string _imagesPath;

        public GamesService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}"; // this is the 3w root 
            //we make this field for the duplicated use to save multiple images 
        }
        public async Task Create(CreateGameFormViewModel game)
        {
            // make a name
            var coverName = await SaveCover(game.Cover);

            // to save the game into db
            Game NewGame = new()
            {
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                Devices = game.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList(),
                Cover = coverName
            };
            _context.Add(NewGame);
            _context.SaveChanges();
        }

        public IEnumerable<Game> GetAllWithCategory()
        {
            return _context.Games.Include(g=>g.Category).Include(G=>G.Devices).ThenInclude(D=>D.Device)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetById(int id)
        {
            return _context.Games.Include(g => g.Category).Include(G => G.Devices).ThenInclude(D => D.Device)
               .AsNoTracking().FirstOrDefault(g=>g.Id== id);
        }
        public async Task<Game?> Edit(EditGameFormViewModel model)
        {
            var game = _context.Games.Include(g=> g.Devices)
                .SingleOrDefault(g=>g.Id == model.Id);
            if (game == null)
                return null;

            var hasNewCover = model.Cover != null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToHashSet();
            
            if (hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!); // saved in server
            }
            var effectedRows = await _context.SaveChangesAsync();

            //to remove the old image
            if (effectedRows > 0)
            {  // if the object updated succefully 
                if (hasNewCover) // and the cover changed
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    if(File.Exists(cover))
                        File.Delete(cover);
                }
                return game; // after updating
            }
            else
            { // in the case that not updated we need to remove the new image 
                var cover = Path.Combine(_imagesPath, game.Cover);
                if (File.Exists(cover))
                    File.Delete(cover);
                return null;
            }

        }
        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.SingleOrDefault(g => g.Id == id);
            if (game == null)
                return isDeleted; // false

            // game is here => delete it and images
            var cover = Path.Combine(_imagesPath, game.Cover);
            _context.Remove(game);

            var effectedRows = _context.SaveChanges();
            if(effectedRows > 0)
            {
                isDeleted = true;
                if(File.Exists(cover))
                    File.Delete(cover);
            }

            return isDeleted;
        }
        private async Task<string> SaveCover(IFormFile Cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}"; //unique Name + extension
            // specify the location to save
            var path = Path.Combine(_imagesPath, coverName);// location + nameOfImage
            // to make the stream creates or overrides on the path
            using var stream = File.Create(path);
            await Cover.CopyToAsync(stream);
            return coverName;
        }

    }
}

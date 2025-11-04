
namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        public ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;

        public GamesController(ICategoriesService categoriesService, IDevicesService devicesService, IGamesService gamesService)
        {
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gamesService = gamesService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var games = _gamesService.GetAllWithCategory();
            return View(games);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewmodel = new()
            {
                Categories = _categoriesService.GetAllAsSelectList(),

                 Devices = _devicesService.GetAllAsSelectList()
            };

            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel viewmodel)
        {
            if(!ModelState.IsValid)
            {
                // fill lists 
                viewmodel.Categories = _categoriesService.GetAllAsSelectList();

                viewmodel.Devices = _devicesService.GetAllAsSelectList();

                return View(viewmodel);
            }
            //save game in db
            await _gamesService.Create(viewmodel);
            //save cover to server
            //return to index
            return RedirectToAction("Index");
                
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if (game != null)
                return View(game);
            return NotFound();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);

            if (game == null)
                return NotFound();

            EditGameFormViewModel model = new EditGameFormViewModel {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                CurrentCover = game.Cover,
                SelectedDevices = game.Devices.Select(d=> d.DeviceId).ToList(),
                Categories = _categoriesService.GetAllAsSelectList(),
                Devices = _devicesService.GetAllAsSelectList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetAllAsSelectList();
                model.Devices= _devicesService.GetAllAsSelectList();
                return View(model);
            }
            //update db and return
            var game = await _gamesService.Edit(model);
            if (game == null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }
        //[HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;

namespace RunGroup.Controllers
{
    public class ClubController : Controller
    {
        //injecting dependency (Dependency Injection)
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index() //Controller 
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll(); //Model
            return View(clubs); //View
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetIdByAsync(id);

            if (club == null)
            {
                return NotFound();
            }
            else
            {
                return View(club);
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }               
                };
                    _clubRepository.Add(club);
                    return RedirectToAction("Index");                
            } 
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }

            return View(clubVM);
           
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetIdByAsync(id);
            
            if(club == null) return View("Error");

            var clubVm = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = (int)club.AddressId,
                Address = club.Address, 
                URL = club.Image,
                ClubCategory = club.ClubCategory

            };

            return View(clubVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Failed to edit club");
                return View("Edit", clubVm);
            }

            var userClub = await _clubRepository.GetIdByAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch(Exception ex) 
                {
                    ModelState.AddModelError("", "Could not delete  photo");
                    return View(clubVm);
                }

                var photoResult = await _photoService.AddPhotoAsync(clubVm.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVm.AddressId,
                    Address = clubVm.Address,  
                };

                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVm);
            }
        }
    }
}
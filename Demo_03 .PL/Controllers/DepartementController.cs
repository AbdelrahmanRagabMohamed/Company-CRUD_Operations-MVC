using AutoMapper;
using Demo_03.BLL.Interfaces;
using Demo_03.DAL.Models;
using Demo_03_.PL.Helpers;
using Demo_03_.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo_03_.PL.Controllers
{
    [Authorize]  // it is means : To Can Enter in This Controller You Must be Make Login in 
    public class DepartementController : Controller
    {

        // private readonly IDepartementRepository _unitOfWork.DepartementRepository;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        // Ask CLR for Creating Object from Class That Implement Interface

        //public DepartementController(IDepartementRepository departementRepository
        //    , IMapper mapper)

        public DepartementController(IUnitOfWork unitOfWork
      , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            // _unitOfWork.DepartementRepository = departementRepository;

            _mapper = mapper;
        }


        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Departement> departements;
            if (string.IsNullOrEmpty(SearchValue))
                departements = await _unitOfWork.DepartementRepository.GetAllAsync();

            else
                departements = _unitOfWork.DepartementRepository.SearchDepartementByName(SearchValue);

            var MappedDepartement = _mapper.Map<IEnumerable<Departement>, IEnumerable<DepartementViewModel>>(departements);
            return View(MappedDepartement);  // model


        }

        //////////////////////////////////////////

        public IActionResult Create()  // Goes To View Not Add Departement (in DB)
        {
            return View();
        }

        [HttpPost] // => Submit can only excuted when the action work as HttpPost
        public async Task<IActionResult> Create(DepartementViewModel departementVM)  // Excuted When Submit form
        {
            if (ModelState.IsValid)  // Check Server Side Validation
            {
                departementVM.FileName = DocumentSettings.UploadFile(departementVM.file, "Images");

                // Use Map
                var MappedDepartement = _mapper.Map<DepartementViewModel, Departement>(departementVM);

                await _unitOfWork.DepartementRepository.AddAsync(MappedDepartement); // Added Locally

                int Result = await _unitOfWork.CompleteAsync();  // Added Remotly (in DB)

                if (Result > 0)
                {
                    TempData["Message"] = "Departement Created Successfully";
                } // TempData

                return Redirect(nameof(Index));
            }
            return View(departementVM);
        }

        //////////////////////////////////////////

        public async Task<IActionResult> Details(int? id, string ViewName)
        {
            if (id is null)
                return BadRequest();  // Status Code => 400

            var departement = await _unitOfWork.DepartementRepository.GetByIdAsync(id.Value);

            if (departement is null)
                return NotFound();

            var MappedDepartement = _mapper.Map<Departement, DepartementViewModel>(departement);
            return View(MappedDepartement);

        }

        //////////////////////////////////////////

        [HttpGet]  // For Get View
        public async Task<IActionResult> Edit(int? id)
        {
           ///if (id is null)
           ///    return BadRequest();
           ///var departement = _unitOfWork.DepartementRepository.GetById(id.Value);
           ///if (departement is null)
           ///    return NotFound();
           ///return View(departement);
           ///return Details(id); // invalid because it will too return view with the same name of action (Details)

            return await Details(id, "Edit"); // return Right View (Edit)

        }


        [HttpPost]
        [ValidateAntiForgeryToken] // To Prevent any Tool From able to make To unallowed action

        public async Task<IActionResult> Edit(DepartementViewModel departementVM, [FromRoute] int id)
        {
            if (departementVM.Id != id)  // prevent user from make an unallowed action
                return BadRequest();

            var MappedDepartement = _mapper.Map<DepartementViewModel, Departement>(departementVM);

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartementRepository.Update(MappedDepartement); // Updated Locally

                    int Result = await _unitOfWork.CompleteAsync(); // Updated Remotly (in DB)

                    if (Result > 0)
                    {
                        TempData["Message"] = "Departement Updated Successfully";
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(MappedDepartement);

        }

        //////////////////////////////////////////
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // To Prevent any Tool From able to make To unallowed action
        public async Task<IActionResult> Delete(DepartementViewModel departementVM, [FromRoute] int id)
        {
            if (departementVM.Id != id)  // prevent user from make an unallowed action
                return BadRequest();

            var MappedDepartement = _mapper.Map<DepartementViewModel, Departement>(departementVM);

            if (ModelState.IsValid)
                try
                {
                    _unitOfWork.DepartementRepository.Delete(MappedDepartement); // Deleted Locally

                    int Result = await _unitOfWork.CompleteAsync(); // Deleted Remotly (in DB)

                    if (Result > 0)
                    {

                        TempData["Message"] = "Departement Deleted Successfully";
                    }  // TempData Message

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            return View(MappedDepartement);


        }
    }
}

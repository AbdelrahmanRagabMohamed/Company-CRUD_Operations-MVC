using AutoMapper;
using Demo_03.BLL.Interfaces;
using Demo_03.BLL.Repositores;
using Demo_03.DAL.Contexts;
using Demo_03.DAL.Models;
using Demo_03_.PL.Helpers;
using Demo_03_.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
namespace Demo_03_.PL.Controllers
{
    [Authorize]  // it is means : To Can Enter in This Controller You Must be Make Login in 

    public class EmployeeController : Controller
    {
        // Without Using Unit of Work
        //private readonly IEmployeeRepository _unitOfWork.EmployeeRepository;
        //private readonly IDepartementRepository _departementRepository;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        /// Without Using Unit of Work
        ///public EmployeeController(IEmployeeRepository employeeRepository,
        ///    IDepartementRepository departementRepository
        ///   , IMapper mapper)
        /// Ask CLR for Creating Object from Class That Implement Interface


        // Using Unit of Work => EmployeeController Now is Based on UnitOFWork
        public EmployeeController(IUnitOfWork unitOfWork   // Ask CLR for Creating Object from Class That Implement Interface
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // Actions

        public async Task <IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;

            ViewBag.departements = await _unitOfWork.DepartementRepository.GetAllAsync();

            if (string.IsNullOrEmpty(SearchValue))
                employees = await  _unitOfWork.EmployeeRepository.GetAllAsync();

            else
                employees = _unitOfWork.EmployeeRepository.SearchEmployeeByName(SearchValue);

            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);

            /// ViewData 
            /// ViewData["Message"] = "Hello From ViewData";
            /// ViewBag
            /// ViewBag.Message = "Hello From ViewBag";


        }

        //////////////////////////////////////////

        [HttpGet]
        public async Task <IActionResult> Create()
        {
            ViewBag.departements = await _unitOfWork.DepartementRepository.GetAllAsync();
            return View();
        }


        [HttpPost]  // => Submit can only excuted when the action work as HttpPost
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)  // Check Server Side Validation
            {
                #region  Manual Mapping       
                //var MappedEmployee = new Employee();
                //{
                //    Name = employeeVM.Name,
                //    Age = employeeVM.Age,
                //    Address = employeeVM.Address,
                //    Salary = employeeVM.Salary,
                //    PhoneNumber = employeeVM.PhoneNumber,
                //    DepartementId = employeeVM.DepartementId
                //}
                // Employee employee = (Employee) employeeVM ; // Casting
                #endregion


                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM?.Image, "Images");

                // Use Map
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                /// if We have a some of Transactions in This Action lik Add , Update , Delete,Update
                /// We must Make All Transactions firstly and SaveChanges in DB If All Changes 
                /// has been Excuted and Don't Save any of it if All don't Excuted
                /// So => We Use UnitofWork To Do it

                await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);  // Add Locally

                int Result =await _unitOfWork.CompleteAsync();   // Add (Remotly) in DB

                if (Result > 0)
                {
                    TempData["Message"] = "Employee Added Successfully";
                }


                return Redirect(nameof(Index));
            }

            ViewBag.departements = _unitOfWork.DepartementRepository.GetAllAsync();  // عشان ترجع الأقسام للـ View لو فيه خطأ
            return View(employeeVM);

        }


        //////////////////////////////////////////

        public async Task< IActionResult> Details(int? id, string ViewName)
        {
            if (id is null)
                return BadRequest();

            var employees = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employees == null)
                return NotFound();

            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employees);


            return View(ViewName, MappedEmployee);

        }

        //////////////////////////////////////////

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
                return NotFound();

            // استخدام AutoMapper لتحويل Employee إلى EmployeeViewModel
            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);

            ViewBag.departements = await _unitOfWork.DepartementRepository.GetAllAsync();  // أضف الأقسام هنا عشان القائمة تظهر في الـ View
            return View(employeeVM);  // قم بتمرير EmployeeViewModel إلى الـ View
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // To Prevent any Tool From able to make To unallowed action
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (employeeVM.Id != id)  // prevent user from make an unallowed action
                return BadRequest();

            if (ModelState.IsValid)
                try
                {

                    if (employeeVM.Image is not null)
                    {
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    }

                    // Use Map
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);  // Updated Locally
                    int Result = await _unitOfWork.CompleteAsync();  // Updated (Remotly) in DB 

                    if (Result > 0)
                    {
                        TempData["Message"] = "Employee Updated Successfully";
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            ViewBag.departements = await _unitOfWork.DepartementRepository.GetAllAsync();  // عشان تعرض الأقسام لو حصل خطأ في الـ ModelState

            return View(employeeVM);
        }

        //////////////////////////////////////////

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // To Prevent any Tool From able to make To unallowed action
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (employeeVM.Id != id)  // prevent user from make an unallowed action
                return BadRequest();

            if (ModelState.IsValid)  // Server Side Validation
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee); // Deleted Locally 

                    int Result = await _unitOfWork.CompleteAsync(); // Deleted in DB (Remotly)

                    if (Result > 0 && employeeVM.ImageName is not null)
                    {
                        TempData["Message"] = "Employee Deleted Successfully";

                        DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            return View(employeeVM);


        }

    }
}

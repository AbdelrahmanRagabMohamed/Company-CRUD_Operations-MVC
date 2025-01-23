using AutoMapper;
using Demo_03.DAL.Models;
using Demo_03_.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_03_.PL.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admins have the permission to access Roles

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // Index Action: عرض جميع الأدوار أو البحث عن دور محدد
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var mappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(roles);

                return View(mappedRoles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(SearchValue);
                if (role == null)
                {
                    //ViewBag.Message = "No role's Name Founded.";
                    return View(new List<RoleViewModel>()); // إرسال قائمة فارغة
                }

                var mappedRole = _mapper.Map<RoleViewModel>(role);
                return View(new List<RoleViewModel>() { mappedRole });
            }
        }


        // عرض صفحة الإنشاء
        public IActionResult Create()
        {
            return View();
        }

        // إنشاء دور جديد
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<IdentityRole>(model);
                await _roleManager.CreateAsync(mappedRole);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // عرض تفاصيل الدور
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var mappedRole = _mapper.Map<RoleViewModel>(role);
            return View(ViewName, mappedRole);
        }

        // عرض صفحة التعديل
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        // تعديل الدور
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model, [FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role == null) return NotFound(); // تحقق من وجود الدور

                    role.Name = model.RoleName;
                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        // عرض صفحة الحذف
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        // تأكيد الحذف
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return NotFound(); // تحقق من وجود الدور

                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

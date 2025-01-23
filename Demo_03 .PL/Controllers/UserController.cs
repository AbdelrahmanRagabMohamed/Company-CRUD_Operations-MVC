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
    [Authorize(Roles = "Admin")] // Only Admins have the permission to access Users
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

       
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                // اجلب جميع المستخدمين
                var users = await _userManager.Users.ToListAsync();

                // إنشاء قائمة ViewModel بشكل تسلسلي لتجنب الأخطاء
                var userViewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                    var userViewModel = new UserViewModel
                    {
                        Id = user.Id,
                        Fname = user.Fname,
                        Lname = user.Lname,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = await _userManager.GetRolesAsync(user)
                    };
                    userViewModels.Add(userViewModel);
                }

                return View(userViewModels);
            }
            else
            {
                // البحث عن المستخدم بالبريد الإلكتروني
                var user = await _userManager.FindByEmailAsync(SearchValue);

                if (user == null)
                {
                    return View(new List<UserViewModel>());
                }

                var mappedUser = new UserViewModel
                {
                    Id = user.Id,
                    Fname = user.Fname,
                    Lname = user.Lname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = await _userManager.GetRolesAsync(user)
                };

                return View(new List<UserViewModel> { mappedUser });
            }
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var mappedUser = _mapper.Map<UserViewModel>(user);
            mappedUser.Roles = await _userManager.GetRolesAsync(user);

            return View(ViewName, mappedUser);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    if (user == null)
                        return NotFound();

                    // تحديث البيانات الشخصية
                    user.Fname = model.Fname;
                    user.Lname = model.Lname;
                    user.PhoneNumber = model.PhoneNumber;

                    // تحديث الأدوار
                    var currentRoles = await _userManager.GetRolesAsync(user); // جلب الأدوار الحالية
                    await _userManager.RemoveFromRolesAsync(user, currentRoles); // حذف الأدوار الحالية
                    if (model.Roles.Any())
                    {
                        await _userManager.AddToRolesAsync(user, model.Roles); // إضافة الأدوار الجديدة
                    }

                    await _userManager.UpdateAsync(user); // تحديث المستخدم

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
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

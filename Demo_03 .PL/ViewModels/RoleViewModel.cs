using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;

namespace Demo_03_.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }

        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

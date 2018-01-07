using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Models;

namespace Taqweem.ViewModels.AdminViewModels
{
    public class UserListViewModel
    {
        public List<ApplicationUser> Users;

        public UserListViewModel()
        {
            Users = new List<ApplicationUser>();
        }

    }
}

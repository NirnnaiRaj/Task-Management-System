using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Buisness.ViewModels;

namespace TaskManagement.Buisness.Interface
{
    public interface IUserBuisness
    {
        Task<int> ValidateUser(LoginViewModel loginModel);
        Task<bool> UserRegistration(UserModel user);
    }
}

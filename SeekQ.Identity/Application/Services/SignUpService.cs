using App.Common.Exceptions;
using App.Common.SeedWork;
using Microsoft.AspNetCore.Identity;
using SeekQ.Identity.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SeekQ.Identity.Application.Services
{
    public class SignUpService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Utils _utls;

        public SignUpService(
            UserManager<ApplicationUser> userManager,
            Utils utils
        )
        {
            _userManager = userManager;
            _utls = utils;
        }

        public async Task<ApplicationUser> CreateUserFromPhoneOrEmailAsync(
            string phoneNumberOrEmail
        )
        {
            //Email/Phone number needs to be validated previously
            var isEmail = _utls.IsEmail(phoneNumberOrEmail);

            var existingUser = await _userManager.FindByNameAsync(phoneNumberOrEmail);

            if (existingUser != null)
            {
                if (isEmail)
                {
                    if (existingUser.EmailConfirmed)
                    {
                        throw new AppException("User account already exists");
                    }
                    else
                    {
                        return existingUser;
                    }
                }
                else
                {
                    if (existingUser.PhoneNumberConfirmed)
                    {
                        throw new AppException("User account already exists");
                    }
                    else
                    {
                        return existingUser;
                    }
                }
            }

            ApplicationUser appUser = new ApplicationUser
            {
                UserName = phoneNumberOrEmail,
                Email = isEmail ? phoneNumberOrEmail : "",
                PhoneNumber = !isEmail ? phoneNumberOrEmail : "",
            };

            var result = await _userManager.CreateAsync(appUser);
            if (result.Succeeded == false)
            {
                throw new AppException(result.Errors.ElementAt(0).Description);
            }

            return appUser;
        }

        public async Task<ApplicationUser> ConfirmUserPhoneOrEmail(
            Guid userId,
            string phoneNumberOrEmail
        )
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            if (existingUser == null)
            {
                throw new AppException("The provided user id does not exist");
            }

            //Email/Phone number needs to be validated previously
            var isEmail = _utls.IsEmail(phoneNumberOrEmail);
            if (isEmail)
            {
                existingUser.EmailConfirmed = true;
            }
            else
            {
                existingUser.PhoneNumberConfirmed = true;
            }

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded == false)
            {
                throw new AppException(result.Errors.ElementAt(0).Description);
            }

            return existingUser;
        }
    }
}

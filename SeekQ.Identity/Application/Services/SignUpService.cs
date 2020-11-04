using App.Common.Exceptions;
using App.Common.SeedWork;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
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
                EmailConfirmationCode = isEmail ? GetEmailVerificationCode() : ""
            };

            var result = await _userManager.CreateAsync(appUser);
            if (result.Succeeded == false)
            {
                throw new AppException(result.Errors.ElementAt(0).Description);
            }

            return appUser;
        }

        public async Task<ApplicationUser> UpdateUserAsConfirmed(
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

        private string GetEmailVerificationCode()
        {
            Random ran = new Random();

            string b = "0123456789";
            int length = 6;
            string random = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(10);
                random = random + b.ElementAt(a);
            }

            return random;
        }
    }
}

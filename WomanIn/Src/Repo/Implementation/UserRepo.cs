﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WomanInAPI.Src.Context;
using WomanInAPI.Src.Models;

namespace WomanInAPI.Src.Repo.Implementation
{
    public class UserRepo : IUser
    {
        #region Attributes

        private readonly WomanInContext _context;

        #endregion


        #region Constructors
        public UserRepo(WomanInContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods 

        public async Task<User> GetUserByEmailAsync (string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task NewUserAsync(User user)
        {
            await _context.Users.AddAsync(
                new User
                {
                    Email = user.Email,
                    Name = user.Name,
                    Password = user.Password,
                    CPF_CNPJ = user.CPF_CNPJ,
                    Area = user.Area,
                });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            if (!EmailExist(user.Email)) throw new Exception("Email não existente");

            var userExist = await GetUserByEmailAsync(user.Email);
                userExist.Email = user.Email;
            _context.Users.Update(userExist);
            await _context.SaveChangesAsync();

            //aux
            bool EmailExist(string Email) 
            {
                var aux = _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
                return aux != null;
            }
        }

        public async Task DeleteUserAsync(string email)
        {
            _context.Users.Remove(await GetUserByEmailAsync(email));
            await _context.SaveChangesAsync();
        }

        #endregion

    }
}

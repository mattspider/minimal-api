using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.domain.entities;
using minimal_api.DTOs;
using minimal_api.infraestructure.DBContext;
using minimal_api.infraestructure.Interfaces;

namespace minimal_api.domain.services
{
    public class AdminService : IAdmin
    {
        private readonly DBContext _context;

        public AdminService(DBContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.email == email);
        }

        public Admin Login(LoginDto loginDto)
        {
            var admins = _context.Admins
                .Where(a => a.email == loginDto.Email && a.Password == loginDto.Password)
                .ToList().FirstOrDefault();
            return admins;
        }

        public async Task<bool> ValidateAdminCredentialsAsync(string email, string password)
        {
            var admin = await GetAdminByEmailAsync(email);
            return admin != null && admin.Password == password;
        }
    }

}
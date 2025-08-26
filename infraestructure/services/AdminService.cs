using Microsoft.EntityFrameworkCore;
using minimal_api.domain.DTO;
using minimal_api.domain.entities;
using minimal_api.DTOs;
using minimal_api.infraestructure.Data;
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

        public Admin? BuscarAdminPorId(int id)
        {
            return _context.Admins.Find(id);
        }

        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.email == email);
        }

        public Admin incluir(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();

            return admin;
        }

        public Admin Login(LoginDto loginDto)
        {
            var admins = _context.Admins
                .Where(a => a.email == loginDto.Email && a.Password == loginDto.Password)
                .ToList().FirstOrDefault();
            return admins;
        }

        public List<Admin> Todos(int? pagina)
        {
            var query = _context.Admins.AsQueryable();
            int itensPorPagina = 10;

            if (pagina != null)
            {
                query.Skip(((int)pagina - 1) * itensPorPagina)
                     .Take(itensPorPagina);
            }
            return query.ToList();

        }

        public async Task<bool> ValidateAdminCredentialsAsync(string email, string password)
        {
            var admin = await GetAdminByEmailAsync(email);
            return admin != null && admin.Password == password;
        }
    }

}
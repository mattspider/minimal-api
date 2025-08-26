using minimal_api.domain.DTO;
using minimal_api.domain.entities;
using minimal_api.DTOs;

namespace minimal_api.infraestructure.Interfaces
{
    public interface IAdmin
    {
        public Admin? Login(LoginDto loginDto);
        Admin incluir(Admin admin);
        List<Admin> Todos(int? pagina);
        Admin? BuscarAdminPorId(int id);
    }
}
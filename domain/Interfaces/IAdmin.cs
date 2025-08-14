using minimal_api.domain.entities;
using minimal_api.DTOs;

namespace minimal_api.infraestructure.Interfaces
{
    public interface IAdmin
    {
        public Admin Login(LoginDto loginDto);
    }
}
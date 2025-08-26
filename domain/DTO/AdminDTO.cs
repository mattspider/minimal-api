
using minimal_api.domain.Enums;

namespace minimal_api.domain.DTO
{
    public class AdminDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public Role? Role { get; set; }
    }
}
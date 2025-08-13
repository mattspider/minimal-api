using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace minimal_api.domain.entities
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; } = default;

        [Required
        , StringLength(50)]
        public string role { get; set; } = default;

        [Required
        , StringLength(50)]
        public string Password { get; set; } = default;

        [Required
        , StringLength(50)]
        public string email { get; set; } = default;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Shared.Constants;

namespace TaskManager.Domain.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("PasswordHash")]
        [Required]
        [DisallowNull]
        public string PasswordHash { get; set; }
        [Column("passwordsalt")]
        public string PasswordSalt { get; set; }
        [Column("Role")]
        public string Role { get; set; }
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }
    }

}

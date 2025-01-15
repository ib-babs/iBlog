using Microsoft.AspNetCore.Identity;
using iBlog.Models;
using System.ComponentModel.DataAnnotations;
using Azure.Core;

namespace iBlog.Data
{
    public class ApplicationUser: IdentityUser
    {
        //[Key]
        //public required string Id { get; set; }
        public virtual ICollection<BlogModel>? Posts { get; set; }
    }
}

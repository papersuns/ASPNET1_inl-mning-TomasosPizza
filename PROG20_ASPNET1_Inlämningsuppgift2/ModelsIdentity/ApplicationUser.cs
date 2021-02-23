using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PROG20_ASPNET1_Inlämningsuppgift2.ModelsIdentity
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [StringLength(100, ErrorMessage = "Namn får vara max 100 tecken")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gatuadress är obligatoriskt")]
        [StringLength(50, ErrorMessage = "Gatuadress får vara max 50 tecken")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Postnr är obligatoriskt")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Får bara innehålla siffror")]
        [StringLength(20, ErrorMessage = "Postnr får vara max 20 tecken")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Postort är obligatoriskt")]
        [StringLength(100, ErrorMessage = "Postort får vara max 100 tecken")]
        public string City { get; set; }
    }
}

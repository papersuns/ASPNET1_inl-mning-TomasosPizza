using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PROG20_ASPNET1_Inlämningsuppgift2.Models
{
    public partial class Kund
    {
        public Kund()
        {
            Bestallnings = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [StringLength(100, ErrorMessage = "Namn får vara max 100 tecken")]
        public string Namn { get; set; }

        [Required(ErrorMessage = "Gatuadress är obligatoriskt")]
        [StringLength(50, ErrorMessage = "Gatuadress får vara max 50 tecken")]
        public string Gatuadress { get; set; }

        [Required(ErrorMessage = "Postnr är obligatoriskt")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Får bara innehålla siffror")]
        [StringLength(20, ErrorMessage = "Postnr får vara max 20 tecken")]
        public string Postnr { get; set; }

        [Required(ErrorMessage = "Postort är obligatoriskt")]
        [StringLength(100, ErrorMessage = "Postort får vara max 100 tecken")]
        public string Postort { get; set; }

        [EmailAddress(ErrorMessage = "Ej giltigt emailadress format")]
        [StringLength(50, ErrorMessage = "Email får vara max 50 tecken")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Får bara innehålla siffror")]
        [StringLength(50, ErrorMessage = "Telefon får vara max 50 tecken")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
        [StringLength(20, ErrorMessage = "Användarnamn får vara max 20 tecken")]
        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Lösenord får vara max 20 tecken")]
        public string Losenord { get; set; }
        public string Id { get; set; }

        public virtual ICollection<Bestallning> Bestallnings { get; set; }
    }
}

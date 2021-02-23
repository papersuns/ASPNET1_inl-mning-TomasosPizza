using System;
using System.Collections.Generic;

#nullable disable

namespace PROG20_ASPNET1_Inlämningsuppgift2.Models
{
    public partial class Bestallning
    {
        public Bestallning()
        {
            BestallningMatratts = new HashSet<BestallningMatratt>();
        }

        public int BestallningId { get; set; }
        public DateTime BestallningDatum { get; set; }
        public int Totalbelopp { get; set; }
        public bool Levererad { get; set; }
        public int KundId { get; set; }

        public virtual Kund Kund { get; set; }
        public virtual ICollection<BestallningMatratt> BestallningMatratts { get; set; }
    }
}

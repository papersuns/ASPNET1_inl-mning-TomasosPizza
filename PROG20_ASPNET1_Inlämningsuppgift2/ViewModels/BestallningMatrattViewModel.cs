using PROG20_ASPNET1_Inlämningsuppgift2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG20_ASPNET1_Inlämningsuppgift2.ViewModels
{
    public class BestallningMatrattViewModel
    {
        public List<Matratt> Matratter { get; set; }
        public Bestallning newBestallning { get; set; }
        public BestallningMatratt newBestallningMatratt { get; set; }

    }
}

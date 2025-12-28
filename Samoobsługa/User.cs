using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samoobsługa
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public int Discount { get; set; } = 0; // w procentach
        public bool CardActive { get; set; } = false;
    }
}
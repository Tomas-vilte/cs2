using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Counter_Strike_2_Multi
{
    public class Entity
    {   
        public IntPtr address { get; set; }

        public int health { get; set; }

        public Vector3 origin { get; set; }
    }
}
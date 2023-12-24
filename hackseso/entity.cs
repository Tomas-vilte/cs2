using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Counter_Strike_2_Multi
{
    public class Entity
    {   
        public IntPtr address { get; set; }

        public int health { get; set; }

        public Vector3 origin { get; set; }

        public int teamNum {  get; set; }

        public int jumpFlag { get; set; }

        public Vector3 abs {  get; set; }

        public Vector3 viewOffset { get; set; }

        public Vector2 originScreenPosition { get; set; }

        public Vector2 absScreenPosition { get; set; }

        public IntPtr pawnAddress { get; set; }

        public IntPtr controllerAddress { get; set; }

        public int team {  get; set; }

        public uint lifeState { get; set; }

        public float distance {  get; set; }

        public List<Vector3> bones { get; set; }

        public List<Vector2> bones2d { get; set; }

    }
}
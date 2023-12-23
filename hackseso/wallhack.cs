using ClickableTransparentOverlay;
using Counter_Strike_2_Multi;
using Swed64;
using System.Numerics;

namespace CS2MULTI
{
    class Program : Overlay
    {
        Swed swed = new Swed("cs2");

        Entity localPlayer = new Entity();
        List<Entity> entities = new List<Entity>();

        IntPtr client;

        protected override void Render()
        {
            // solo renderiza cosas aca
        }
        static void Main(string[] args)
        {
            // Corre la logica y metodos
        }
    }
}
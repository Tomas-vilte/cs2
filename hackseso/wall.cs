using BoneEspTutorial;
using Counter_Strike_2_Multi;
using Swed64;
using System.Numerics;

Swed swed = new Swed("cs2");

IntPtr client = swed.GetModuleBase("client.dll");

Renderer renderer = new Renderer();
renderer.Start().Wait();

// entity
List<Entity> entities = new List<Entity>();
Entity localPlayer = new Entity();
Vector2 screen = new Vector2(1920, 1080);

renderer.overlaySize = screen;

while (true)
{   
    // Eliminar entidades viejas y consola
    entities.Clear();
    Console.Clear();

    IntPtr entityList = swed.ReadPointer(client, 0x0);
}
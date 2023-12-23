using ClickableTransparentOverlay;
using Counter_Strike_2_Multi;
using Swed64;

namespace CS2MULTI
{
    class Program : Overlay
    {
        Swed swed = new Swed("cs2");
        Offsets offsets = new Offsets();

        Entity localPlayer = new Entity();
        List<Entity> entities = new List<Entity>();

        IntPtr client;

        protected override void Render()
        {
            // solo renderiza cosas aca
        }

        void MainLogic()
        {
            client = swed.GetModuleBase("client.dll");


            while (true) // Siempre corre 
            {
                entities.Clear(); // client lists

                localPlayer.address = swed.ReadPointer(client, offsets.localPlayer); // establece la dirección para que pueda actualizar
                UpdateEntity(localPlayer);

                Console.WriteLine($"health -> {localPlayer.health}");
            }
        }

        void UpdateEntity(Entity entity)
        {
            entity.health = swed.ReadInt(entity.address, offsets.health);
            entity.origin = swed.ReadVec(entity.address, offsets.origin);
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start().Wait();

            Thread mainLogicThread = new Thread(program.MainLogic) { IsBackground = true };
            mainLogicThread.Start();
        }


    }
}
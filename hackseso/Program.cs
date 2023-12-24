using ClickableTransparentOverlay;
using Counter_Strike_2_Multi;
using ImGuiNET;
using Swed64;
using System.Numerics;

namespace CS2MULTI
{
    class Program : Overlay
    {
        Swed swed = new Swed("cs2");
        Offsets offsets = new Offsets();

        Entity localPlayer = new Entity();
        List<Entity> entities = new List<Entity>();
        List<Entity> enemyTeam = new List<Entity>();
        List<Entity> playerTeam = new List<Entity>();

        IntPtr client;

        // global colors
        Vector4 teamColor = new Vector4(0,0,0,1); // color azul los compa
        Vector4 enemyColor = new Vector4(1,0,0,1); // color rojo los enemigos
        Vector4 healthBarColor = new Vector4 (0,1,0,1); // verde la vida
        Vector4 healthTextColor = new Vector4(0, 0, 0, 1); // negro el texto

        Vector2 windowLocation = new Vector2 (0,0);
        Vector2 windowSize = new Vector2 (1920, 1080);
        Vector2 lineOrigin = new Vector2(1920 / 2, 1080);

        JumpController jumpController;

        protected override void Render()
        {
            // solo renderiza cosas aca
            ImGui.Begin("counter");
        }

        void MainLogic()
        {
            client = swed.GetModuleBase("client.dll");


            while (true) // Siempre corre 
            {

                //foreach (var entity in entities)
                //{
                //    Console.WriteLine($"Entity health -> {entity.health} entity position: {entity.origin}");
                //}
                //Thread.Sleep(100);
                //Console.Clear();
            }
        }

        void ReloadEntities()
        {
            entities.Clear(); // client lists

            localPlayer.address = swed.ReadPointer(client, offsets.localPlayer); // establece la dirección para que pueda actualizar
            UpdateEntity(localPlayer);

            UpdateEntities();
        }
        void UpdateEntities()
        {
            for (int i = 0; i < 64; i++)
            {
                IntPtr tempEntityAddress = swed.ReadPointer(client, offsets.entityList + i * 0x08);

                if (tempEntityAddress == IntPtr.Zero)
                    continue;

                Entity entity = new Entity();
                entity.address = tempEntityAddress;

                UpdateEntity(entity);

                if (entity.health < 1 || entity.health > 100)
                    continue;

                if (!entities.Any(element => element.origin.X == entity.origin.X))
                {
                    entities.Add(entity);
                }

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

            program.jumpController = new JumpController();
            program.jumpController.StartJumpControl();
        }


    }
}
using ClickableTransparentOverlay;
using Counter_Strike_2_Multi;
using ImGuiNET;
using Swed64;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CS2MULTI
{
    class Program : Overlay
    {
        [DllImport("user32.dll")]   
        static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public RECT GetWindowRect(IntPtr hWnd)
        {
            RECT rect = new RECT();
            GetWindowRect(hWnd, out rect);
            return rect;
        }
        

        Swed swed = new Swed("cs2");
        Offsets offsets = new Offsets();
        ImDrawListPtr drawList;

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

        Vector2 windowLocation = new Vector2(0,0);
        Vector2 windowSize = new Vector2 (1920, 1080);
        Vector2 lineOrigin = new Vector2(1920 / 2, 1080);
        Vector2 windowsCenter = new Vector2(1920 / 2, 1080 / 2);

        bool enableEsp = true;

        bool enableTeamLine = true;
        bool enableTeamBox = true;
        bool enableTeamDot = false;
        bool enableTeamHealthBar = true;
        bool enableTeamDistance = true;

        bool enableEnemyLine = true;
        bool enableEnemyBox = true;
        bool enableEnemyDot = false;
        bool enableEnemyHealthBar = true;
        bool enableEnemyDistance = true;

        JumpController jumpController;

        protected override void Render()
        {
            // solo renderiza cosas aca
            ImGui.Begin("counter");
        }

        void MainLogic()
        {
            client = swed.GetModuleBase("client.dll");

            var window = GetWindowRect(swed.GetProcess().MainWindowHandle);
            windowLocation = new Vector2(window.left, window.top);
            windowSize = Vector2.Subtract(new Vector2(window.right, window.bottom), windowLocation);
            lineOrigin = new Vector2(windowLocation.X + windowLocation.X/2, window.bottom);
            windowsCenter = new Vector2(lineOrigin.X, window.bottom - windowSize.Y/2);

            while (true) // Siempre corre 
            {

                ReloadEntities();

                Thread.Sleep(3);
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

                    if (entity.teamNum == localPlayer.teamNum)
                    {
                        playerTeam.Add(entity);
                    }
                    else
                    {
                        enemyTeam.Add(entity);
                    }
                }

            }
        }

        void UpdateEntity(Entity entity)
        {
            entity.health = swed.ReadInt(entity.address, offsets.health);
            entity.origin = swed.ReadVec(entity.address, offsets.origin);
            entity.teamNum = swed.ReadInt(entity.address, offsets.teamNum);
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
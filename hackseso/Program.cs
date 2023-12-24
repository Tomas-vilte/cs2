using ClickableTransparentOverlay;
using Counter_Strike_2_Multi;
using hackseso;
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
            DrawMenu();
            DrawOverlay();
        }

        ViewMatrix ReadMatrix(IntPtr matrixAddress)
        {
            var viewMatrix = new ViewMatrix();
            var floatMatrix = swed.ReadMatrix(matrixAddress);

            viewMatrix.m11 = floatMatrix[0];
            viewMatrix.m12 = floatMatrix[1];
            viewMatrix.m13 = floatMatrix[2];
            viewMatrix.m14 = floatMatrix[3];

            viewMatrix.m21 = floatMatrix[4];
            viewMatrix.m22 = floatMatrix[5];
            viewMatrix.m23 = floatMatrix[6];
            viewMatrix.m24 = floatMatrix[7];

            viewMatrix.m31 = floatMatrix[8];
            viewMatrix.m32 = floatMatrix[9];
            viewMatrix.m33 = floatMatrix[10];
            viewMatrix.m34 = floatMatrix[11];

            viewMatrix.m41 = floatMatrix[12];
            viewMatrix.m42 = floatMatrix[13];
            viewMatrix.m43 = floatMatrix[14];
            viewMatrix.m44 = floatMatrix[15];

            return viewMatrix;
        }

        Vector2 WorldToScreen(ViewMatrix matrix, Vector3 pos, int width,  int height)
        {   
            Vector2 screenCoordinates = new Vector2();

            float screenW = (matrix.m41 * pos.X) + (matrix.m42 * pos.Y) + (matrix.m43 * pos.Z) + matrix.m44;

            if (screenW > 0.001f)
            {
                float screenX = (matrix.m11 * pos.X) + (matrix.m12 * pos.Y) + (matrix.m13 * pos.Z) + matrix.m14;

                float screenY = (matrix.m21 * pos.X) + ( matrix.m22 * pos.Y) + (matrix.m23 * pos.Z) + matrix.m24;

                float camX = width / 2;
                float camY = height / 2;

                float X = camX * (camX * screenX / screenW);
                float Y = camY * (camY * screenY / screenW);

                screenCoordinates.X = X;
                screenCoordinates.Y = Y;

                return screenCoordinates;
            }
            else
            {
                return new Vector2(-99, -99);
            }
        }

        void DrawMenu()
        {
            ImGui.Begin("counter");

            if (ImGui.BeginTabBar("Tabs"))
            {
                if (ImGui.BeginTabItem("General"))
                {
                    ImGui.Checkbox("Esp", ref enableEsp);
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("colors"))
                {   

                    // Color del team
                    ImGui.ColorPicker4("Team color", ref teamColor);
                    ImGui.Checkbox("Team line", ref enableTeamLine);
                    ImGui.Checkbox("Team box", ref enableTeamBox);
                    ImGui.Checkbox("Team dot", ref enableTeamDot);
                    ImGui.Checkbox("Team HealthBar", ref enableTeamHealthBar);

                    // Color del team contrario
                    ImGui.ColorPicker4("Enemy color", ref enemyColor);
                    ImGui.Checkbox("Enemy line", ref enableEnemyLine);
                    ImGui.Checkbox("Enemy box", ref enableEnemyBox);
                    ImGui.Checkbox("Enemy dot", ref enableEnemyDot);
                    ImGui.Checkbox("Enemy HealthBar", ref enableEnemyHealthBar);
                    ImGui.EndTabItem();
                }
            }
            ImGui.EndTabBar();
        }

        void DrawOverlay()
        {
            ImGui.SetNextWindowSize(windowSize);
            ImGui.SetWindowPos(windowLocation);
            ImGui.Begin("Overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse
                );
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
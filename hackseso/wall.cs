using Swed64;
using System.Numerics;

namespace hackseso
{
    public class WallHack
    {
        private readonly Swed swed;
        private readonly Reader reader;
        private readonly Renderer renderer;

        public WallHack()
        {
            swed = new Swed("cs2");
            reader = new Reader(swed);
            renderer = new Renderer();
        }


        public void RunWallhack()

        {
            renderer.Start().Wait();

            List<Entity> entities = new List<Entity>();
            Entity localPlayer = new Entity();
            Vector2 screen = new Vector2(1920, 1080);

            renderer.overlaySize = screen;

            while (true)
            {
                entities.Clear();
                Console.Clear();

                IntPtr client = swed.GetModuleBase("client.dll");
                IntPtr entityList = swed.ReadPointer(client, Player.dwEntityList);
                IntPtr localPawn = swed.ReadPointer(client, Player.dwLocalPlayerPawn);

                localPlayer.pawnAddress = localPawn;
                localPlayer.team = swed.ReadInt(localPawn, Player.m_iTeamNum);
                localPlayer.origin = swed.ReadVec(localPawn, Player.m_vOldOrigin);

                for (int i = 0; i < 64; i++)
                {
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);
                    if (listEntry == IntPtr.Zero)
                        continue;

                    IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);
                    if (currentController == IntPtr.Zero)
                        continue;

                    int pawnHandle = swed.ReadInt(currentController, Player.m_hPlayerPawn);
                    if (pawnHandle == 0)
                        continue;

                    IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                    IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

                    if (currentPawn == localPlayer.pawnAddress)
                        continue;

                    IntPtr sceneMode = swed.ReadPointer(currentPawn, Player.m_pGameSceneNode);
                    IntPtr boneMatrix = swed.ReadPointer(sceneMode, Player.m_modelState + 0x80);

                    ViewMatrix viewMatrix = reader.readMatrix(client + Player.dwViewMatrix);
                    Console.WriteLine(viewMatrix);

                    int team = swed.ReadInt(currentPawn, Player.m_iTeamNum);
                    uint lifeState = swed.ReadUInt(currentPawn, Player.m_lifeState);

                    if (lifeState != 256)
                        continue;

                    Entity entity = new Entity();

                    entity.pawnAddress = currentPawn;
                    entity.controllerAddress = currentController;
                    entity.team = team;
                    entity.lifeState = lifeState;
                    entity.origin = swed.ReadVec(currentPawn, Player.m_vOldOrigin);
                    entity.distance = Vector3.Distance(entity.origin, localPlayer.origin);
                    entity.bones = reader.ReadBones(boneMatrix);
                    entity.bones2d = reader.ReadBones2d(entity.bones, viewMatrix, screen);

                    entities.Add(entity);

                    Console.ForegroundColor = ConsoleColor.Green;

                    if (team != localPlayer.team)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.ResetColor();
                }

                renderer.entitiesCopy = entities;
                renderer.LocalPlayerCopy = localPlayer;
                Thread.Sleep(3);
            }
        }
    }
}

/*
using hackseso;
using Swed64;
using System.Numerics;

Swed swed = new Swed("cs2");

IntPtr client = swed.GetModuleBase("client.dll");

Reader reader = new Reader(swed);

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
    // obtenemos la lista de entidades
    IntPtr entityList = swed.ReadPointer(client, Player.dwEntityList);

    // primera entida
    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

    localPlayer.pawnAddress = swed.ReadPointer(client, Player.dwLocalPlayerPawn);
    localPlayer.team = swed.ReadInt(localPlayer.pawnAddress, Player.m_iTeamNum);
    localPlayer.origin = swed.ReadVec(localPlayer.pawnAddress, Player.m_vOldOrigin);

    for (int i = 0; i < 64; i++)
    {
        if (listEntry == IntPtr.Zero)
            continue;

        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

        if (currentController == IntPtr.Zero)
            continue;

        int pawnHandle = swed.ReadInt(currentController, Player.m_hPlayerPawn);

        if (pawnHandle == 0)
            continue;

        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);

        IntPtr currenPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

        if (currenPawn == localPlayer.pawnAddress)
            continue;

        IntPtr sceneMode = swed.ReadPointer(currenPawn, Player.m_pGameSceneNode);
        IntPtr boneMatrix = swed.ReadPointer(sceneMode, Player.m_modelState + 0x80);

        ViewMatrix viewMatrix = reader.readMatrix(client + Player.dwViewMatrix);
        Console.WriteLine(viewMatrix);

        int team = swed.ReadInt(currenPawn, Player.m_iTeamNum);
        uint lifeState = swed.ReadUInt(currenPawn, Player.m_lifeState);

        if (lifeState != 256)
            continue;

        Entity entity = new Entity();


        entity.pawnAddress = currenPawn;
        entity.controllerAddress = currentController;
        entity.team = team;
        entity.lifeState = lifeState;
        entity.origin = swed.ReadVec(currenPawn, Player.m_vOldOrigin);
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
*/
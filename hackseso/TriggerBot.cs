using Swed64;
using System.Runtime.InteropServices;

namespace hackseso
{
    public class TriggerBot
    {
        private Swed swed;
        private IntPtr client;
        private IntPtr forceAttack;

        public TriggerBot()
        {
            swed = new Swed("cs2");
            client = swed.GetModuleBase("client.dll");
            forceAttack = client + 0x16C1E90; // Actualice esta dirección de memoria con la encontrada
        }

        public async void Run()
        {
            while (true)
            {
                IntPtr entityList = swed.ReadPointer(client, Player.dwEntityList);
                IntPtr localPlayerPawn = swed.ReadPointer(client, Player.dwLocalPlayerPawn);

                int team = swed.ReadInt(localPlayerPawn, Player.m_iTeamNum);
                int entIndex = swed.ReadInt(localPlayerPawn, Player.m_iIDEntIndex);
                Console.WriteLine($"Crosshair/Entity ID {entIndex}");

                if (entIndex != -1)
                {
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x8 * ((entIndex & 0x7FFF) >> 9) + 0x10);
                    IntPtr currentPawn = swed.ReadPointer(listEntry, 0x78 * (entIndex & 0x1FF));

                    int entityTeam = swed.ReadInt(currentPawn, Player.m_iTeamNum);
                    if (team != entityTeam)
                    {
                        if (GetAsyncKeyState(0x43) < 0)
                        {
                            swed.WriteInt(forceAttack, 65537);
                            await Task.Delay(1);
                            swed.WriteInt(forceAttack, 256);
                            await Task.Delay(1);
                        }
                    }
                }
               
                await Task.Delay(1);
            }
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
    }
}

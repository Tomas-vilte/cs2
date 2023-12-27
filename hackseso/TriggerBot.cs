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
            forceAttack = client + 0x16C1E70; // Actualice esta dirección de memoria con la encontrada
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();

                IntPtr localPlayerPawn = swed.ReadPointer(client, Player.dwLocalPlayerPawn);
                int entIndex = swed.ReadInt(localPlayerPawn, Player.m_iIDEntIndex);

                Console.WriteLine($"Crosshair/Entity ID {entIndex}");

                if (GetAsyncKeyState(0x06) < 0) // Cambie la direccion de memoria de la letra que quiera
                {
                    if (entIndex > 0)
                    {
                        swed.WriteInt(forceAttack, 65537);
                        Thread.Sleep(1);
                        swed.WriteInt(forceAttack, 256);
                    }
                }

                Thread.Sleep(1);
            }
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
    }
}

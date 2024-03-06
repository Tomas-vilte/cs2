using Swed64;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace hackseso
{
    public class Glow
    {
        private Swed swed;
        private IntPtr client;
        private bool playersVisible = true; // Variable para controlar el estado de visibilidad de los jugadores
        private const float HIDDEN_TIME = 0f; // Valor para ocultar a los jugadores
        private const float VISIBLE_TIME = 86400f; // Valor para mostrar a los jugadores

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private bool IsKeyDownX()
        {
            const int VK_X = 0x58;
            return (GetAsyncKeyState(VK_X) & 0x8001) != 0;
        }

        public Glow()
        {
            swed = new Swed("cs2");
            client = swed.GetModuleBase("client.dll");
        }

        public void Run()
        {
            while (true)
            {
                if (IsKeyDownX()) // Verificar si la tecla 'X' está presionada
                {
                    playersVisible = !playersVisible; // Cambiar el estado de visibilidad
                    Thread.Sleep(100); // Evitar cambios rápidos si la tecla se mantiene presionada
                }

                if (playersVisible) // Verificar si los jugadores deben ser visibles
                {
                    SetPlayerVisibility(VISIBLE_TIME);
                }
                else
                {
                    SetPlayerVisibility(HIDDEN_TIME);
                }

                Thread.Sleep(50);
                Console.Clear();
            }
        }

        private void SetPlayerVisibility(float visibilityTime)
        {
            IntPtr entityList = swed.ReadPointer(client, Player.dwEntityList);
            IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

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
                IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FFF));

                // Establecer el valor de visibilidad del jugador
                swed.WriteFloat(currentPawn, Player.m_flDetectedByEnemySensorTime, visibilityTime);
                Console.WriteLine($"{i}: {currentPawn}");
            }
        }
    }
}

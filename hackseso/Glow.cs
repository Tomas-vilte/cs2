using Swed64;

namespace hackseso
{
    public class Glow
    {
        private Swed swed;
        private IntPtr client;

        public Glow()
        {
            swed = new Swed("cs2");
            client = swed.GetModuleBase("client.dll");
        }

        public void Run()
        {
            while (true)
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

                    swed.WriteFloat(currentPawn, Player.m_flDetectedByEnemySensorTime, 86400);
                    Console.WriteLine($"{i}: {currentPawn}");


                }
                Thread.Sleep(50);
                Console.Clear();

                
            }
        }
    }

   
}
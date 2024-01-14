using Swed64;

namespace hackseso
{
   public class AntiFlash
    {
        private Swed swed;
        private IntPtr client;

        public AntiFlash()
        {
            swed = new Swed("cs2");
            client = swed.GetModuleBase("client.dll");
        }

        public void RunAntiFlash()
        {
            while (true)
            {
                IntPtr localPlayerPawn = swed.ReadPointer(client, Player.dwLocalPlayerPawn);
                float flashDuration = swed.ReadFloat(localPlayerPawn, Player.m_flFlashBangTime);

                if (flashDuration > 0)
                {
                    swed.WriteFloat(localPlayerPawn, Player.m_flFlashBangTime, 0);;
                    Console.WriteLine("No me flasheaste");
                }
                Thread.Sleep(2);
            }
        }
    }
}

using hackseso;
using Swed64;
using System.Runtime.InteropServices;

public class TriggerBot
{
    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    public void Run()
    {
        Swed swed = new Swed("cs2");
        IntPtr client = swed.GetModuleBase("client.dll");
        IntPtr forceAttack = client + 0x16C1E70;

        // trigger
        while (true)
        {
            IntPtr localPlayerPawn = swed.ReadPointer(client, Player.dwLocalPlayerPawn);
            int entIndex = swed.ReadInt(localPlayerPawn, Player.m_iIDEntIndex);
            Console.WriteLine($"Crosshair/Entity ID: {entIndex}");

            if (GetAsyncKeyState(0x6) < 0)
            {
                if (entIndex < 0)
                {
                    swed.WriteInt(forceAttack, 65537);
                    Thread.Sleep(1);
                    swed.WriteInt(forceAttack, 256);
                }
            }
            Thread.Sleep(1);
        }
    }
}

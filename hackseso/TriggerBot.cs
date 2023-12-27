using hackseso;
using Swed64;
using System.Runtime.InteropServices;

public class TriggerBot
{
    private readonly Swed swed;

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    public TriggerBot()
    {
        swed = new Swed("cs2");
    }

    public void Run()
    {
        IntPtr client = swed.GetModuleBase("client.dll");
        IntPtr forceAttack = client + 0x16C1F00;

        while (true)
        {
            Console.Clear();

            IntPtr localPlayerPawn = swed.ReadPointer(client, Player.dwLocalPlayerPawn);
            int entIndex = swed.ReadInt(client, Player.m_iIDEntIndex);
            Console.WriteLine($"Crosshair/Entity ID: {entIndex}");

            if (GetAsyncKeyState(0x6) < 0)
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
}

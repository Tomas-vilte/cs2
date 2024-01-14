using hackseso;

class Program
{   
    static async Task Main(string[] args)
    {
        WallHack wallhack = new WallHack();
        TriggerBot triggerBot = new TriggerBot();
        AntiFlash flash = new AntiFlash();

        Task wallhackTask = Task.Run(() => wallhack.RunWallhack());
        flash.RunAntiFlash();
        triggerBot.Run();
        await wallhackTask;
    }
}

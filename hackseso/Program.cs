using hackseso;

class Program
{   
    static async Task Main(string[] args)
    {
        WallHack wallhack = new WallHack();
        TriggerBot triggerBot = new TriggerBot();

        Task wallhackTask = Task.Run(() => wallhack.RunWallhack());
        triggerBot.Run();
        await wallhackTask;
    }
}

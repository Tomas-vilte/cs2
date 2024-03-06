using hackseso;

class Program
{   
    static async Task Main(string[] args)
    {
        //WallHack wallhack = new WallHack();
        Glow glow = new Glow();
        TriggerBot triggerBot = new TriggerBot();

        //Task wallhackTask = Task.Run(() => wallhack.RunWallhack());
        Task task = Task.Run(() => glow.Run());
        triggerBot.Run();
        await task;
        //await wallhackTask;
    }
}

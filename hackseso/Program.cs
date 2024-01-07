using hackseso;

class Program
{   
    static async Task Main(string[] args)
    {
        //WallHack wallhack = new WallHack();
        //TriggerBot triggerBot = new TriggerBot();
        Glow glow = new Glow();
        glow.Run();

        //Task wallhackTask = Task.Run(() => wallhack.RunWallhack());

        //triggerBot.Run();

        //await wallhackTask;
    }
}

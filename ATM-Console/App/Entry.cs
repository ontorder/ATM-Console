namespace AtmConsole.App
{
    internal class Entry
    {
        static void Main(string[] args)
        {
            AtmApp atmApp = new();
            atmApp.InitializeData();
            atmApp.Run();
            //Utility.PressEnterToContinue();
        }
    }
}

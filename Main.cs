using Life;
using System;

namespace MyCloths
{
    public class Main : Plugin
    {
        public Main(IGameAPI api) :base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            Console.WriteLine($"Plugin \"MyCloths\" initialisé avec succès.");
        }
    }
}
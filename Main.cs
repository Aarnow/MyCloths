using Life;
using Life.Network;
using Life.UI;
using MyCloths.Panels;
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

            new SChatCommand("/mycloths", new string[] { "/mc", "/myc" }, "Permet d'ouvrir le panel du plugin MyCloths", "/mycloths", (player, arg) =>
            {
                if (player.IsAdmin) MainPanel.OpenMyClothsMenu(player);
            }).Register();

            Console.WriteLine($"Plugin \"MyCloths\" initialisé avec succès.");
        }
    }
}
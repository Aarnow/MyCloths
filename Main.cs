using Life;
using Life.Network;
using MyCloths.Panels;
using System;
using System.IO;

namespace MyCloths
{
    public class Main : Plugin
    {
        public static string directoryPath;
        public static string clothPath;

        public Main(IGameAPI api) :base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();

            new SChatCommand("/mycloths", new string[] { "/mc", "/myc" }, "Permet d'ouvrir le panel du plugin MyCloths", "/mycloths", (player, arg) =>
            {
                if (player.IsAdmin) MainPanel.OpenMyClothsMenu(player);
            }).Register();

            Console.WriteLine($"Plugin \"MyCloths\" initialisé avec succès.");
        }

        public void InitDirectory()
        {
            directoryPath = pluginsPath + "/MyCloths";
            clothPath = directoryPath + "/Cloth";

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            if (!Directory.Exists(clothPath)) Directory.CreateDirectory(clothPath);
        }
    }
}
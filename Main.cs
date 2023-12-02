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

        public static string girlColor = "#E30B5C";
        public static string boyColor = "#19A8AF";

        public static int shirtIcon = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(153).icon);
        public static int pantIcon = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1073).icon);

        public Main(IGameAPI api) :base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();

            new SChatCommand("/mycloths", new string[] { "/mc", "/myc" }, "Permet d'ouvrir le panel du plugin MyCloths", "/mycloths", (player, arg) =>
            {
                if (player.IsAdmin) ClothListPanels.ShowClostList(player);
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
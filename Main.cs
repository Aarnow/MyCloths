using Life;
using Life.InventorySystem;
using Life.Network;
using Mirror;
using MyCloths.Panels;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        public static List<Cloth> clothList = new List<Cloth>();

        public Main(IGameAPI api) :base(api)
        {
            foreach (BuyableCloth c in Nova.server.buyableCloths)
            {
                Cloth newCloth = new Cloth();
                newCloth.Slug = c.name;
                newCloth.Name = c.name;
                newCloth.Price = 0;
                switch (c.clothType)
                {
                    case 0:
                        newCloth.ClothType = CharacterCustomization.ClothesPartType.Hat;
                        break;
                    case 1:
                        newCloth.ClothType = CharacterCustomization.ClothesPartType.Accessory;
                        break;
                    case 2:
                        newCloth.ClothType = CharacterCustomization.ClothesPartType.Shirt;
                        break;
                    case 3:
                        newCloth.ClothType = CharacterCustomization.ClothesPartType.Pants;
                        break;
                    case 4:
                        newCloth.ClothType = CharacterCustomization.ClothesPartType.Shoes;
                        break;
                    default:
                        break;
                }
                newCloth.ClothId = c.clothId;
                newCloth.SexId = c.sexId;
                newCloth.IsCustom = false;
                clothList.Add(newCloth);
            }
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

        public override void OnPlayerInput(Player player, KeyCode keyCode, bool onUI)
        {
            base.OnPlayerInput(player, keyCode, onUI);

            if (keyCode == KeyCode.O)
            {
                if (!onUI)
                {
                    PlayerClothPanels.PlayerClothMenu(player);
                }
            }
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
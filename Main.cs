using Life;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyCloths.Components;
using MyCloths.Panels;
using MyMenu.Entities;
using System;
using System.IO;
using System.Linq;
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

        public static PClothList clothList = new PClothList();

        public Main(IGameAPI api) :base(api)
        {
            foreach (BuyableCloth c in Nova.server.buyableCloths)
            {
                PCloth newCloth = new PCloth();
                newCloth.Slug = c.name;
                newCloth.Name = c.name;
                newCloth.Price = 0;
                newCloth.ClothId = c.clothId;
                newCloth.SexId = c.sexId;
                newCloth.IsCustom = false;
                newCloth.IsRestricted = false;

                switch (c.clothType)
                {
                    case 0:
                        newCloth.ClothType = ClothType.Hat;
                        clothList.Hats.Add(newCloth);
                        break;
                    case 1:
                        newCloth.ClothType = ClothType.Accessory;
                        clothList.Accessories.Add(newCloth);
                        break;
                    case 2:
                        newCloth.ClothType = ClothType.Shirt;
                        clothList.Shirts.Add(newCloth);
                        break;
                    case 3:
                        newCloth.ClothType = ClothType.Pants;
                        clothList.Pants.Add(newCloth);
                        break;
                    case 4:
                        newCloth.ClothType = ClothType.Shoes;
                        clothList.Shoes.Add(newCloth);
                        break;
                    default:
                        break;
                }
            }

            PCloth.AddRestrictedCloth("Casquette de policier", ClothType.Hat, 3);
            PCloth.AddRestrictedCloth("Casque de pompier", ClothType.Hat, 4);
            PCloth.AddRestrictedCloth("Casque de protection", ClothType.Hat, 5);
            PCloth.AddRestrictedCloth("Casque du SWAT", ClothType.Hat, 6);
            PCloth.AddRestrictedCloth("Chapeau de cowboy", ClothType.Hat, 7);
            PCloth.AddRestrictedCloth("Bonnet de Noël", ClothType.Hat, 8);

            PCloth.AddRestrictedCloth("Gilet de policier", ClothType.Shirt, 10);
            PCloth.AddRestrictedCloth("Manteau de pompier", ClothType.Shirt, 11);
            PCloth.AddRestrictedCloth("Blouse de médecin", ClothType.Shirt, 12);
            PCloth.AddRestrictedCloth("Gilet jaune", ClothType.Shirt, 13);
            PCloth.AddRestrictedCloth("Haut du SWAT", ClothType.Shirt, 14);
            PCloth.AddRestrictedCloth("Torse nu", ClothType.Shirt, -1);

            PCloth.AddRestrictedCloth("Pantalon de policier", ClothType.Pants, 10);
            PCloth.AddRestrictedCloth("Pantalon de pompier", ClothType.Pants, 11);
            PCloth.AddRestrictedCloth("Pantalon de médecin", ClothType.Pants, 12);
            PCloth.AddRestrictedCloth("Pantalon de chantier", ClothType.Pants, 13);
            PCloth.AddRestrictedCloth("Pantalon du SWAT", ClothType.Pants, 14);
            PCloth.AddRestrictedCloth("Sous-vêtement", ClothType.Pants, -1);

            PCloth.AddRestrictedCloth("Chaussures de policier", ClothType.Shoes, 10);
            PCloth.AddRestrictedCloth("Chaussures de pompier", ClothType.Shoes, 11);
            PCloth.AddRestrictedCloth("Chaussures de médecin", ClothType.Shoes, 12);
            PCloth.AddRestrictedCloth("Chaussures de chantier", ClothType.Shoes, 13);
            PCloth.AddRestrictedCloth("Chaussures du SWAT", ClothType.Shoes, 14);
            PCloth.AddRestrictedCloth("Pieds nu", ClothType.Shoes, -1);
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();

            new SChatCommand("/mycloths", new string[] { "/mc", "/myc" }, "Permet d'ouvrir le panel de configuration du plugin MyCloths", "/mycloths", (player, arg) =>
            {
                if (player.IsAdmin) ClothListPanels.ShowClostList(player);
            }).Register();

            //MyMenu
            Section section = new Section(Section.GetSourceName(), "MyCloth", "v1.0.1", "Aarnow");
            Action<UIPanel> action = ui =>
            {
                Player player = section.GetPlayer(ui);
                if (Nova.a.GetAreaById(player.setup.areaId).permissions.owner.characterId == player.character.Id ||
                  Nova.a.GetAreaById(player.setup.areaId).permissions.coOwners.Any(co => co.characterId == player.character.Id))
                {
                    PlayerClothPanels.PlayerClothMenu(player);
                }
                else
                {
                    PlayerClothPanels.PlayerClothMenuOutside(player);
                }
            };
            section.Line = new UITabLine(section.Title, action);
            section.Insert();

            Console.WriteLine($"Plugin \"MyCloths\" initialisé avec succès.");
        }

        public override void OnPlayerInput(Player player, KeyCode keyCode, bool onUI)
        {
            base.OnPlayerInput(player, keyCode, onUI);
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
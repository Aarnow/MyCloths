using Life;
using Life.Network;
using Life.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UIPanelManager;

namespace MyCloths.Panels
{
    abstract class ClothListPanels
    {
        public static void ShowClostList(Player player)
        {
            string[] jsonFiles = Directory.GetFiles(Main.clothPath, "*.json");
            List<Cloth> clothList = new List<Cloth>();

            foreach ((string jsonFile, int index) in jsonFiles.Select((jsonFile, index) => (jsonFile, index)))
            {
                string json = File.ReadAllText(jsonFile);
                Cloth currentCloth = JsonConvert.DeserializeObject<Cloth>(json);
                clothList.Add(currentCloth);
            }

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.TabPrice).SetTitle($"Liste des vêtements personnalisés");

            foreach ((Cloth currentCloth, int index) in clothList.Select((currentCloth, index) => (currentCloth, index)))
            {
                panel.AddTabLine($"{(currentCloth.IsMale ? $"<color={Main.boyColor}>[H]</color>" : $"<color={Main.girlColor}>[F]</color>")} {currentCloth.Name}",
                    currentCloth.Price.ToString("F2") + "€",
                    (currentCloth.ClothType == CharacterCustomization.ClothesPartType.Shirt ? Main.shirtIcon : Main.pantIcon),
                    (ui) => ui.selectedTab = index);
            }

                panel.AddButton("Récupérer", (ui) => PanelManager.Quit(ui, player));
            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () => ClothDataPanels.SetupClothType(player)));
            panel.AddButton("Supprimer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                clothList[ui.selectedTab].Delete(player);
                ShowClostList(player);
            }));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}

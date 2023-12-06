using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyCloths.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UIPanelManager;

namespace MyCloths.Panels
{
    abstract class PlayerClothPanels
    {

        public static void PlayerClothMenu(Player player)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Tab).SetTitle($"Vos vêtements");

            foreach ((ClothType partType, int index) in Enum.GetValues(typeof(ClothType)).Cast<ClothType>().Select((value, index) => (value, index)))
            {
                panel.AddTabLine($"{partType}", (ui) => ui.selectedTab = index);
            }
           
            panel.AddButton("Voir", (ui) =>
            {
                if (Enum.IsDefined(typeof(ClothType), ui.selectedTab))
                {
                    ClothType clothPartType = (ClothType)Enum.ToObject(typeof(ClothType), ui.selectedTab);
                    PanelManager.NextPanel(player, ui, () => PlayerClothSelection(player, clothPartType));
                }
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void PlayerClothMenuBis(Player player)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Tab).SetTitle($"Retirer/Mettre un vêtement");

            foreach ((ClothType partType, int index) in Enum.GetValues(typeof(ClothType)).Cast<ClothType>().Select((value, index) => (value, index)))
            {
                panel.AddTabLine($"{partType}", (ui) => ui.selectedTab = index);
            }

            panel.AddButton("Equiper/Retirer", (ui) =>
            {
                if (Enum.IsDefined(typeof(ClothType), ui.selectedTab))
                {
                    ClothType clothPartType = (ClothType)Enum.ToObject(typeof(ClothType), ui.selectedTab);
                    PCloth.EquipClothByTypeBis(player, clothPartType);
                }
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static async void PlayerClothSelection(Player player, ClothType clothType)
        {
            List<PCloth> pCloths = await Main.clothList.GetClothListOfPlayer(player, clothType);

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Tab).SetTitle($"Changer un vêtement");

            if(pCloths.Count <= 0) panel.AddTabLine($"Vous n'avez aucun vêtement de ce type.", (ui) => ui.selectedTab = 0);
            else
            {
                foreach ((PCloth cloth, int index) in pCloths.Select((cloth, index) => (cloth, index)))
                {
                    panel.AddTabLine($"{(cloth.SexId == 0 ? $"<color={Main.boyColor}>[H]</color>" : $"<color={Main.girlColor}>[F]</color>")} {cloth.Name}", (ui) => ui.selectedTab = index);
                }
                panel.AddButton("Equiper", (ui) =>
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        PCloth.EquipClothByType(player, clothType, pCloths[ui.selectedTab].ClothId);
                        PlayerClothSelection(player, clothType);
                    });
                });
            }

            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => PlayerClothMenu(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}

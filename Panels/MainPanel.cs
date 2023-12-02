using Life;
using Life.Network;
using Life.UI;
using System;
using UIPanelManager;

namespace MyCloths.Panels
{
    public class MainPanel
    {
        public static void OpenMyClothsMenu(Player player)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Tab).SetTitle($"MyCloths Menu");

            panel.AddTabLine("Liste des vêtements personnalisés", (ui) => ui.selectedTab = 0);
            panel.AddTabLine("Créer un vêtement personnalisé", (ui) => ui.selectedTab = 1);
            
            panel.AddButton("Sélection", (ui) =>
            {
                if (ui.selectedTab == 0) PanelManager.NextPanel(player, ui, () => Console.WriteLine("next panel"));
                else if (ui.selectedTab == 1) PanelManager.NextPanel(player, ui, () => ClothDataPanels.SetupClothType(player));
                else PanelManager.Notification(player, "Erreur", "Vous devez sélectionner un choix", NotificationManager.Type.Error);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}

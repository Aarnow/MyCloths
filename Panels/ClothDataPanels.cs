using Life;
using Life.AirportSystem;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using System;
using UIPanelManager;

namespace MyCloths.Panels
{
    internal class ClothDataPanels
    {
        public static void SetupClothType(Player player)
        {
            Cloth newCloth = new Cloth();
            newCloth.IsCustom = true;

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Text).SetTitle($"Définir le type de vêtement");

            panel.text = "Voulez-vous créer un T-Shirt ou un Pantalon ?";

            panel.AddButton("T-Shirt", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                newCloth.ClothType = CharacterCustomization.ClothesPartType.Shirt;
                SetupClothGender(player, newCloth);
            }));
            panel.AddButton("Pantalon", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                newCloth.ClothType = CharacterCustomization.ClothesPartType.Pants;
                SetupClothGender(player, newCloth);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => MainPanel.OpenMyClothsMenu(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothGender(Player player, Cloth newCloth)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Text).SetTitle($"Définir la genre du vêtement");

            panel.text = "Est-ce pour les Femmes ou les Hommes ?";

            panel.AddButton("Homme", (ui) =>
            {
                newCloth.isMale = true;
                PanelManager.NextPanel(player, ui, () => SetupClothData(player, newCloth));
            });
            panel.AddButton("Femme", (ui) =>
            {
                newCloth.isMale = false;
                PanelManager.NextPanel(player, ui, () => SetupClothData(player, newCloth));
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothType(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothData(Player player, Cloth newCloth)
        {
            ClothData newClothData = new ClothData();

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Input).SetTitle($"Définir le style du vêtement");

            panel.inputPlaceholder = "Coller l'URL de votre UV";

            panel.AddButton("Confirmer", (ui) =>
            {
                if (ui.inputText.Length <= 0)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        newClothData.url = ui.inputText;
                        PanelManager.NextPanel(player, ui, () => SetupClothPrice(player, newCloth, newClothData));
                    });
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner l'URL de votre UV", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothGender(player, newCloth)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothPrice(Player player, Cloth newCloth, ClothData newClothData)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Input).SetTitle($"Définir le prix du vêtement");

            panel.inputPlaceholder = "Donner un prix à votre vêtement";

            panel.AddButton("Confirmer", (ui) =>
            {
                if (double.TryParse(ui.inputText, out double clothPrice) && clothPrice >= 0)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        newCloth.Price = clothPrice;
                        PanelManager.NextPanel(player, ui, () => SetupClothName(player, newCloth, newClothData));
                    });
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez définir un prix au bon format et positif.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothData(player, newCloth)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothName(Player player, Cloth newCloth, ClothData newClothData)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Input).SetTitle($"Définir le nom du vêtement");

            panel.inputPlaceholder = "Donner un nom à votre vêtement";

            panel.AddButton("Sauvegarder", (ui) =>
            {
                if (ui.inputText.Length <= 0)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        newCloth.Name = ui.inputText;
                        newClothData.clothName = ui.inputText;
                        PanelManager.NextPanel(player, ui, () => SetupClothName(player, newCloth, newClothData));
                    });
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner le nom de votre vêtement", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothPrice(player, newCloth, newClothData)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }  
    }
}

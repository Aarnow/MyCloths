using Life;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyCloths.Components;
using Newtonsoft.Json;
using UIPanelManager;

namespace MyCloths.Panels
{
    abstract class ClothDataPanels
    {
        public static void SetupClothType(Player player)
        {
            PCloth newCloth = new PCloth();
            newCloth.IsCustom = true;

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Text).SetTitle($"Définir le type de vêtement");

            panel.text = "Voulez-vous créer un T-Shirt ou un Pantalon ?";

            panel.AddButton("T-Shirt", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                newCloth.ClothType = ClothType.Shirt;
                SetupClothGender(player, newCloth);
            }));
            panel.AddButton("Pantalon", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                newCloth.ClothType = ClothType.Pants;
                SetupClothGender(player, newCloth);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => ClothListPanels.ShowClostList(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothGender(Player player, PCloth newCloth)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Text).SetTitle($"Définir la genre du vêtement");

            panel.text = "Est-ce pour les Femmes ou les Hommes ?";

            panel.AddButton("Homme", (ui) =>
            {
                newCloth.SexId = 0;
                PanelManager.NextPanel(player, ui, () => SetupClothData(player, newCloth));
            });
            panel.AddButton("Femme", (ui) =>
            {
                newCloth.SexId = 1;
                PanelManager.NextPanel(player, ui, () => SetupClothData(player, newCloth));
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothType(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothData(Player player, PCloth newCloth)
        {
            ClothData newClothData = new ClothData();

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Input).SetTitle($"Définir le style du vêtement");

            panel.inputPlaceholder = "Coller l'URL de votre UV";

            panel.AddButton("Confirmer", (ui) =>
            {
                if (ui.inputText.Length > 0)
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

        public static void SetupClothPrice(Player player, PCloth newCloth, ClothData newClothData)
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
                        PanelManager.NextPanel(player, ui, () => SetupClothRestriction(player, newCloth, newClothData));
                    });
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez définir un prix au bon format et positif.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothData(player, newCloth)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothRestriction(Player player, PCloth newCloth, ClothData newClothData)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Text).SetTitle($"Disponibilité du vêtement");

            panel.text = "Est ce que votre vêtement est disponible pour le grand public ?\n (exemple: Indiquez \"Privé\" s'il s'agit d'un vêtement dédié à un métier)";

            panel.AddButton("Public", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                newCloth.IsRestricted = false;
                SetupClothName(player, newCloth, newClothData);
            }));
            panel.AddButton("Privé", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                newCloth.IsRestricted = true;
                SetupClothName(player, newCloth, newClothData);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetupClothPrice(player, newCloth, newClothData)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupClothName(Player player, PCloth newCloth, ClothData newClothData)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Input).SetTitle($"Définir le nom du vêtement");

            panel.inputPlaceholder = "Donner un nom à votre vêtement";

            panel.AddButton("Sauvegarder", (ui) =>
            {
                if (ui.inputText.Length > 0)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        newCloth.Name = ui.inputText;
                        newClothData.clothName = ui.inputText;
                        newCloth.ClothData = JsonConvert.SerializeObject(newClothData);
                        newCloth.Save();
                        PanelManager.Notification(player, "Succès", "Votre vêtement à bien été sauvegardé.", NotificationManager.Type.Success);
                        ClothListPanels.ShowClostList(player);
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

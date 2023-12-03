using Life;
using Life.CharacterSystem;
using Life.DB;
using Life.Network;
using Life.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIPanelManager;
using static CharacterCustomization;

namespace MyCloths.Panels
{
    abstract class PlayerClothPanels
    {

        public static void PlayerClothMenu(Player player)
        {
            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Tab).SetTitle($"Vos vêtements");

            foreach ((ClothesPartType partType, int index) in Enum.GetValues(typeof(ClothesPartType)).Cast<ClothesPartType>().Select((value, index) => (value, index)))
            {
                panel.AddTabLine($"{partType}", (ui) => ui.selectedTab = index);
            }
           
            panel.AddButton("Voir", (ui) =>
            {
                if (Enum.IsDefined(typeof(ClothesPartType), ui.selectedTab))
                {
                    ClothesPartType clothPartType = (ClothesPartType)Enum.ToObject(typeof(ClothesPartType), ui.selectedTab);
                    PanelManager.NextPanel(player, ui, () => PlayerClothSelection(player, clothPartType));
                }
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void PlayerClothSelection(Player player, ClothesPartType clothType)
        {
            List<Cloth> clothsByType = new List<Cloth>();

            foreach((Cloth cloth, int index) in Main.clothList.Select((cloth, index) => (cloth, index)))
            {
                if (cloth.ClothType == clothType && player.character.SexId == cloth.SexId) clothsByType.Add(cloth);
            }

            UIPanel panel = new UIPanel("MyCloths Menu", UIPanel.PanelType.Tab).SetTitle($"Changer un vêtement");

            foreach ((Cloth cloth, int index) in clothsByType.Select((cloth, index) => (cloth, index)))
            {
                panel.AddTabLine($"{cloth.Name}", (ui) => ui.selectedTab = index);
            }

            panel.AddButton("Equiper", (ui) =>
            {
                SwapClotheByType(player, clothType, clothsByType[ui.selectedTab].ClothId);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SwapClotheByType(Player player, ClothesPartType clotheType, int clotheId)
        {
            switch (clotheType)
            {
                case ClothesPartType.Hat:
                    player.setup.characterSkinData.Hat = clotheId;
                    break;
                case ClothesPartType.Accessory:
                    player.setup.characterSkinData.Accessory = clotheId;
                    break;
                case ClothesPartType.Shirt:
                    player.setup.characterSkinData.TShirt = clotheId;
                    break;
                case ClothesPartType.Pants:
                    player.setup.characterSkinData.Pants = clotheId;
                    break;
                case ClothesPartType.Shoes:
                    player.setup.characterSkinData.Shoes = clotheId;
                    break;
                default:
                    break;
            }

            player.character.Skin = player.setup.characterSkinData.SerializeToJson();
            player.setup.RpcSkinChange(player.setup.characterSkinData); //apply         
        }
    }
}

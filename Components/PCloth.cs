using Life;
using Life.InventorySystem;
using Life.Network;
using Newtonsoft.Json;
using System;
using System.IO;
using UIPanelManager;

namespace MyCloths.Components
{
    [Serializable]
    public class PCloth
    {
        public string Slug;
        public string Name;
        public double Price;
        public ClothType ClothType;
        public int ClothId;
        public int SexId;

        public bool IsCustom;
        public string ClothData;

        public bool IsRestricted;

        public PCloth()
        { 
        }

        public static void AddRestrictedCloth(string name, ClothType clothType, int clothId)
        {
            PCloth restrictedCloth = new PCloth();

            restrictedCloth.Slug = name;
            restrictedCloth.Name = name;
            restrictedCloth.Price = 0;
            restrictedCloth.ClothType = clothType;
            restrictedCloth.ClothId = clothId;

            restrictedCloth.SexId = 2;
            restrictedCloth.IsCustom = false;
            restrictedCloth.ClothData = null;
            restrictedCloth.IsRestricted = true;

            Main.clothList.clothTypes[clothType].Add(restrictedCloth);
        } 

        public void CreateCustomCloth(Player player)
        {
            if (ClothType == ClothType.Shirt) player.setup.inventory.AddItem(SexId == 0 ? 153 : 154, 1, ClothData);
            else player.setup.inventory.AddItem(SexId == 0 ? 1073 : 1074, 1, ClothData);
        }

        public void EquipCustomCloth(Player player)
        {

            if (ClothType == ClothType.Shirt)
            {
                player.setup.interaction.currentClothes.shirtId = SexId == 0 ? 153 : 154;
                player.setup.interaction.currentClothes.shirtData = ClothData;
                player.setup.interaction.UseCloth(player.setup.interaction.currentClothes);
                player.setup.interaction.currentClothes.shirtId = 0;
            }
            else
            {
                player.setup.interaction.currentClothes.pantsId = SexId == 0 ? 1073 : 1074;
                player.setup.interaction.currentClothes.pantsData = ClothData;
                player.setup.interaction.UseCloth(player.setup.interaction.currentClothes);
                player.setup.interaction.currentClothes.pantsId = 0;
            }
        }

        public static void EquipClothByType(Player player, ClothType clotheType, int clotheId)
        {
            switch (clotheType)
            {
                case ClothType.Hat:
                    player.setup.characterSkinData.Hat = clotheId;
                    break;
                case ClothType.Accessory:
                    player.setup.characterSkinData.Accessory = clotheId;
                    break;
                case ClothType.Shirt:
                    player.setup.characterSkinData.TShirt = clotheId;
                    break;
                case ClothType.Pants:
                    player.setup.characterSkinData.Pants = clotheId;
                    break;
                case ClothType.Shoes:
                    player.setup.characterSkinData.Shoes = clotheId;
                    break;
                default:
                    break;
            }

            player.character.Skin = player.setup.characterSkinData.SerializeToJson();
            player.setup.RpcSkinChange(player.setup.characterSkinData);
        }

        public static void EquipClothByTypeBis(Player player, ClothType clotheType)
        {
            CharacterCustomizationSetup skin = JsonConvert.DeserializeObject<CharacterCustomizationSetup>(player.character.Skin);
            switch (clotheType)
            {
                case ClothType.Hat:
                    player.setup.characterSkinData.Hat = player.setup.characterSkinData.Hat != -1 ? -1 : skin.Hat;
                    break;
                case ClothType.Accessory:
                    player.setup.characterSkinData.Accessory = player.setup.characterSkinData.Accessory != -1 ? -1 : skin.Accessory;
                    break;
                case ClothType.Shirt:
                    player.setup.characterSkinData.TShirt = player.setup.characterSkinData.TShirt != -1 ? -1 : skin.TShirt;
                    break;
                case ClothType.Pants:
                    player.setup.characterSkinData.Pants = player.setup.characterSkinData.Pants != -1 ? -1 : skin.Pants;
                    break;
                case ClothType.Shoes:
                    player.setup.characterSkinData.Shoes = player.setup.characterSkinData.Shoes != -1 ? -1 : skin.Shoes;
                    break;
                default:
                    break;
            }

            player.setup.RpcSkinChange(player.setup.characterSkinData);
        }

        public void Delete(Player player)
        {
            string path = Main.clothPath + "/Cloth_" + Slug + ".json";
            Console.WriteLine(path);
            if (File.Exists(path)) File.Delete(path);

            PanelManager.Notification(player, "Succès", "Le vêtement à bien été supprimé.", NotificationManager.Type.Success);
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                Slug = $"{Name}_{number}";
                filePath = Path.Combine(Main.clothPath, $"Cloth_{Slug}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}

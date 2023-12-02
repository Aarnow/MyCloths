using Life;
using Life.Network;
using Newtonsoft.Json;
using System;
using System.IO;
using UIPanelManager;

namespace MyCloths
{
    [Serializable]
    public class Cloth
    {
        public string Slug;
        public string Name;
        public double Price;
        public CharacterCustomization.ClothesPartType ClothType;
        public int ClothId;
        public bool IsMale;
        public bool IsCustom;
        public string ClothData;

        public Cloth()
        {
        }

        public void CreateCustomCloth(Player player)
        {
            if (ClothType == CharacterCustomization.ClothesPartType.Shirt) player.setup.inventory.AddItem(IsMale ? 153 : 154, 1, ClothData);
            else player.setup.inventory.AddItem(IsMale ? 1073 : 1074, 1, ClothData);      
        }

        public void EquipCustomCloth(Player player)
        {

            if (ClothType == CharacterCustomization.ClothesPartType.Shirt)
            {
                player.setup.interaction.currentClothes.shirtId = IsMale ? 153 : 154;
                player.setup.interaction.currentClothes.shirtData = ClothData;
                player.setup.interaction.UseCloth(player.setup.interaction.currentClothes);
                player.setup.interaction.currentClothes.shirtId = 0;
            }
            else
            {
                player.setup.interaction.currentClothes.pantsId = IsMale ? 1073 : 1074;
                player.setup.interaction.currentClothes.pantsData = ClothData;
                player.setup.interaction.UseCloth(player.setup.interaction.currentClothes);
                player.setup.interaction.currentClothes.pantsId = 0;
            }
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

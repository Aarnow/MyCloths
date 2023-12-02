using Life.InventorySystem;
using Life.Network;
using Newtonsoft.Json;
using System.IO;

namespace MyCloths
{
    public class Cloth
    {
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

        public void Create(Player player, Cloth newCloth)
        {
            /*if (newCloth.IsCustom)
            {
                if (newCloth.ClothType == CharacterCustomization.ClothesPartType.Shirt)
                {
                    player.setup.interaction.currentClothes.shirtId = newCloth.IsMale ? 153 : 154;
                }
                else
                {
                    player.setup.interaction.currentClothes.pantsId = newCloth.IsMale ? 1073 : 1074;
                }
            }

            string jsonClothData = JsonConvert.SerializeObject(newCloth.ClothData);
            player.setup.interaction.currentClothes.shirtData = jsonClothData;

            player.setup.interaction.UseCloth(player.setup.interaction.currentClothes);*/
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                Name += $"_{number}";
                filePath = Path.Combine(Main.clothPath, $"Cloth_{Name}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

}

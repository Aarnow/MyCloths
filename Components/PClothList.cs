using Life.DB;
using Life.InventorySystem;
using Life.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCloths.Components
{
    public class PClothList
    {
        public Dictionary<ClothType, List<PCloth>> clothTypes = new Dictionary<ClothType, List<PCloth>>();

        public List<PCloth> Hats => GetClothList(ClothType.Hat);
        public List<PCloth> Accessories => GetClothList(ClothType.Accessory);
        public List<PCloth> Shirts => GetClothList(ClothType.Shirt);
        public List<PCloth> Pants => GetClothList(ClothType.Pants);
        public List<PCloth> Shoes => GetClothList(ClothType.Shoes);

        public PClothList()
        {
            InitializeClothList(ClothType.Hat);
            InitializeClothList(ClothType.Accessory);
            InitializeClothList(ClothType.Shirt);
            InitializeClothList(ClothType.Pants);
            InitializeClothList(ClothType.Shoes);
        }
        private void InitializeClothList(ClothType clothType)
        {
            clothTypes[clothType] = new List<PCloth>();
        }
        private List<PCloth> GetClothList(ClothType clothType)
        {
            return clothTypes.ContainsKey(clothType) ? clothTypes[clothType] : null;
        }

        public async Task<List<PCloth>> GetClothListOfPlayer(Player player, ClothType clothType)
        {
            List<PCloth> pClothsByType = Main.clothList.clothTypes[clothType]
                .Where(c => (player.character.SexId == 0 && c.SexId != 1) || (player.character.SexId == 1 && c.SexId != 0))
                .ToList();

            int clothTypeIndex = Convert.ToInt32(Enum.ToObject(typeof(ClothType), clothType));
            List<Clothes> playerClothByType = await LifeDB.db.Table<Clothes>().Where(c => c.CharacterId == player.character.Id && c.ClothType == clothTypeIndex).ToListAsync();
            List<int> clothIds = playerClothByType.Select(c => c.ClothId).ToList();
            pClothsByType.RemoveAll(c => !clothIds.Contains(c.ClothId));

            return pClothsByType;
        }
    }
}

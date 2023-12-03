using Life.InventorySystem;
using System.Collections.Generic;

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
    }
}

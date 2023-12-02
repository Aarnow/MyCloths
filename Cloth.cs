﻿using Life.InventorySystem;

namespace MyCloths
{
    public class Cloth
    {
        public string Name;
        public double Price;
        public CharacterCustomization.ClothesPartType ClothType;
        public int ClothId;

        public bool IsUsedCloth;
        public ClothData ClothData;
    }
}
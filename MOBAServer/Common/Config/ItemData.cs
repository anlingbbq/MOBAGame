using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Config
{
    public class ItemData
    {
        static Dictionary<int, ItemModel> ItemDict = new Dictionary<int, ItemModel>();

        static ItemData()
        {
            CreateItem(ServerConfig.ItemTypeId, "长剑", 15, 5, 80, 450);
            CreateItem(ServerConfig.ItemTypeId + 1, "圆盾", 0, 10, 180, 450);
            CreateItem(ServerConfig.ItemTypeId + 2, "弯弓", 20, 0, 40, 450);
        }

        public static void CreateItem(int typeId, string name, int attack, int defense, int hp, int price)
        {
            ItemDict.Add(typeId, new ItemModel(name, typeId, attack, defense, hp, price));
        }

        public static ItemModel GetItem(int id)
        {
            ItemModel model;
            ItemDict.TryGetValue(id, out model);
            return model;
        }
    }

    public class ItemModel
    {
        public int Id;
        public int Attack;
        public int Defense;
        public int Hp;
        public string Name;
        public int Price;

        public ItemModel()
        {
        }

        public ItemModel(string name, int id, int attack, int defense, int hp, int price)
        {
            Name = name;
            Id = id;
            Attack = attack;
            Defense = defense;
            Hp = hp;
            Price = price;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Common.Config
{
    public class ItemData
    {
        public static Dictionary<int, ItemModel> ItemDict = new Dictionary<int, ItemModel>();

        static ItemData()
        {
            CreateItem(ServerConfig.ItemTypeId, "长剑", 15, 5, 80, 450);
            CreateItem(ServerConfig.ItemTypeId + 1, "圆盾", 0, 10, 180, 450);
            CreateItem(ServerConfig.ItemTypeId + 2, "弯弓", 20, 0, 40, 450);
        }

        /// <summary>
        /// 创建物品
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="name"></param>
        /// <param name="attack"></param>
        /// <param name="defense"></param>
        /// <param name="hp"></param>
        /// <param name="price"></param>
        public static void CreateItem(int typeId, string name, int attack, int defense, int hp, int price)
        {
            ItemDict.Add(typeId, new ItemModel(name, typeId, attack, defense, hp, price));
        }

        /// <summary>
        /// 根据id获取物品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ItemModel GetItem(int id)
        {
            ItemModel model;
            ItemDict.TryGetValue(id, out model);
            return model;
        }

        public static ItemModel[] GetArray()
        {
            int i = 0;
            ItemModel[] array = new ItemModel[ItemDict.Count];
            foreach (ItemModel item in ItemDict.Values)
            {
                array[i++] = item;
            }
            return array;
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

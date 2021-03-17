using System;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using static Player;
using static Skills;
using BepInEx.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Gluttony
{
    [HarmonyPatch(typeof(Player), "GetTotalFoodValue")]
    static class EatFood_Patch
    {
        private static List<Point> foodLocation = new List<Point>
        {
            new Point(0,1),
            new Point(0,2),
            new Point(0,3),
        };
        private static Logger log = new Logger();

        [HarmonyPostfix]
        static void Postfix(Player __instance, ref float hp)
        {
            var foods = __instance.GetFoods();
            var foodnames = new List<string>();
            if (foods != null && foods.Count > 0)
            {
                foreach (var f in foods)
                {
                    if(!f.CanEatAgain())
                    {
                        foodnames.Add(f.m_item.m_shared.m_name);
                    }
                }
            }

            if (foodnames.Count < 3)
            {
                var inv = __instance.GetInventory();
                var foodToEat = GetFoodFromInventry(inv).Where((x) => !foodnames.Contains(x.m_shared.m_name)).ToList();
                var foodTotal = foodToEat.Count;
                if (foodTotal > 0)
                {
                    foreach (var eat in foodToEat)
                    {
                        if (foodTotal >= 3)
                            break;

                        __instance.UseItem(inv, eat, false);
                        log.write("Eat: {0}", eat.m_shared.m_name);
                        foodTotal++;
                    }
                }
            }
        }

        static List<ItemDrop.ItemData> GetFoodFromInventry(Inventory inv)
        {
            var foundFood = new List<ItemDrop.ItemData>();
            foreach (var p in foodLocation)
            {
                var item = inv.GetItemAt(p.X, p.Y);
                if(item is ItemDrop.ItemData && item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable)
                {
                    foundFood.Add(item);
                }
            }
            return foundFood;
        }

    }
    static class Helper
    {
        private static Logger log = new Logger();

        public static float Percent(ConfigEntry<Int32> value)
        {
            return ((float)value.Value / 100); 
        }

        public static ItemDrop.ItemData GetItemByName(Player __instance, string name)
        {
            var inven = __instance.GetInventory();
            var items = inven.GetAllItems();
            return items.Find((item) => item.m_dropPrefab.name == name);
        }
    }
    class Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

}

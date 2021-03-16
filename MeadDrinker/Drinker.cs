using System;
using BepInEx.Configuration;
using HarmonyLib;
using static Player;
using static Skills;
using BepInEx.Logging;

namespace MeadDrinker
{
    [HarmonyPatch(typeof(Player), "OnDamaged")]
    static class OnDamaged_Patch
    {
        private static Logger log = new Logger();
        [HarmonyPrefix]
        static void Postfix(Player __instance, ref HitData hit)
        {
            if (!__instance.IsPlayer())
                return;

            var hP = __instance.GetHealthPercentage();
            var healMead = Helper.GetItemByName(__instance, "MeadHealthMedium");
            if (healMead == null)
                healMead = Helper.GetItemByName(__instance, "MeadHealthMinor");

            if (healMead != null && hP <= Helper.Percent(MeadDrinker.healthPercent))
            {
                log.write("Try to drink MeadHealth");
                __instance.UseItem(__instance.GetInventory(), healMead, false);
            }
        }

    }
    [HarmonyPatch(typeof(Player), "UseStamina")]
    static class UseStamina_Patch
    {
        private static Logger log = new Logger();
        [HarmonyPostfix]
        static void Postfix(Player __instance, ref float v)
        {
            var sP = __instance.GetStaminaPercentage();
            var hP = __instance.GetHealthPercentage();

            var tHPercent = Helper.Percent(MeadDrinker.tastyHealthPercent);
            var tSPercent = Helper.Percent(MeadDrinker.tastyStaminaPercent);
            var sPercent = Helper.Percent(MeadDrinker.staminaPercent);

            if (hP > tHPercent && sP < tSPercent)
            {
                var tastyMead = Helper.GetItemByName(__instance, "MeadTasty");
                if (tastyMead != null)
                {
                    log.write("Try to drink MeadTasty");
                    __instance.UseItem(__instance.GetInventory(), tastyMead, false);
                }
            }
            if (sP < sPercent)
            {
                var staminaMead = Helper.GetItemByName(__instance, "MeadStaminaMedium");

                if (staminaMead == null)
                    staminaMead = Helper.GetItemByName(__instance, "MeadStaminaMinor");

                if (staminaMead != null)
                {
                    log.write("Try to drink MeadStamina");
                    __instance.UseItem(__instance.GetInventory(), staminaMead, false);
                }
            }
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
}

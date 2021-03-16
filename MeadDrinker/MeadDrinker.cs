using System;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
namespace MeadDrinker
{

    [BepInPlugin("wotofos.meaddrinker", "MeadDrinker", "1.0.0")]
    class MeadDrinker : BaseUnityPlugin
    {
        private static ConfigFile config;

        private static ManualLogSource logSource;

        public static ConfigEntry<Int32> healthPercent;
        public static ConfigEntry<Int32> tastyHealthPercent;
        public static ConfigEntry<Int32> staminaPercent;
        public static ConfigEntry<Int32> tastyStaminaPercent;

        private void Awake()
        {
            config = Config;

            logSource = Logger;

            healthPercent = GetConfig().Bind("Health", "HealthPercent", 20, "Drink Health Mead when below this health percent");
            tastyHealthPercent = GetConfig().Bind("Health", "TastyHealthPercent", 90, "Drink Tasty Mead when below this health percent");

            staminaPercent = GetConfig().Bind("Stamina", "StaminaPercent", 10, "Drink Health Mead when below this stamina percent");
            tastyStaminaPercent = GetConfig().Bind("Stamina", "TastyStaminaPercent", 30, "Drink Tasty Mead when below this stamina percent");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public static void Log(LogLevel level, object data) => logSource.Log(level, data);

        public static ConfigFile GetConfig() => config;

    }

    class Logger
    {
        public void write(string text, params object[] values)
        {
            MeadDrinker.Log(LogLevel.Info, String.Format(
                text,
                values
            ));
        }
        public void write(string text)
        {
            MeadDrinker.Log(LogLevel.Info, text);
        }
    }
}

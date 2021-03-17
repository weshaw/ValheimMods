using System;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
namespace Gluttony
{

    [BepInPlugin("wotofos.gluttony", "Gluttony", "1.0.0")]
    class Gluttony : BaseUnityPlugin
    {
        private static ConfigFile config;

        private static ManualLogSource logSource;

        //public static ConfigEntry<Int32> configValue;

        private void Awake()
        {
            config = Config;

            logSource = Logger;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public static void Log(LogLevel level, object data) => logSource.Log(level, data);

        public static ConfigFile GetConfig() => config;

    }

    class Logger
    {
        public void write(string text, params object[] values)
        {
            Gluttony.Log(LogLevel.Info, String.Format(
                text,
                values
            ));
        }
        public void write(string text)
        {
            Gluttony.Log(LogLevel.Info, text);
        }
    }
}

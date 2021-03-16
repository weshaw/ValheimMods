
using System;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
namespace DefaultPlugin
{

    [BepInPlugin("wotofos.plugintemplate", "DefaultPlugin", "1.0.0")]
    class DefaultPlugin : BaseUnityPlugin
    {
        private static ConfigFile config;

        private static ManualLogSource logSource;

        public static ConfigEntry<string> configValue;

        private void Awake()
        {
            config = Config;

            logSource = Logger;

            configValue = GetConfig().Bind("ConfigCategory", "Name", "Value", "Config Description");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public static void Log(LogLevel level, object data) => logSource.Log(level, data);

        public static ConfigFile GetConfig() => config;

    }

    class Logger
    {
        public void write(string text, params object[] values)
        {
            DefaultPlugin.Log(LogLevel.Info, String.Format(
                text,
                values
            ));
        }
        public void write(string text)
        {
            DefaultPlugin.Log(LogLevel.Info, text);
        }
    }
}

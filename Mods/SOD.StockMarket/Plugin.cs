﻿using BepInEx;
using SOD.Common.BepInEx;
using System.Reflection;
using SOD.StockMarket.Core;

namespace SOD.StockMarket
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(Common.Plugin.PLUGIN_GUID)]
    public class Plugin : PluginController<Plugin, IPluginBindings>
    {
        public const string PLUGIN_GUID = "Venomaus.SOD.StockMarket";
        public const string PLUGIN_NAME = "StockMarket";
        public const string PLUGIN_VERSION = "1.0.0";

        /// <summary>
        /// The stockmarket running during the game, used to do all calculations
        /// </summary>
        internal Market Market { get; private set; }

        public override void Load()
        {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo("Plugin is patched.");

            Market = new Market();
        }

        public override void OnConfigureBindings()
        {
            base.OnConfigureBindings();
            ValidateBindingValues();
        }

        /// <summary>
        /// Validates if all bindings are valid and updates invalid ones.
        /// </summary>
        private void ValidateBindingValues()
        {
            var maxTrendSteps = Config.MaxHoursTrendsCanPersist;
            if (maxTrendSteps < 1)
                Config.MaxHoursTrendsCanPersist = 1;

            var minTrendSteps = Config.MinHoursTrendsMustPersist;
            if (minTrendSteps < 1)
                Config.MinHoursTrendsMustPersist = 1;

            var trendChancePerStock = Config.StockTrendChancePercentage;
            if (trendChancePerStock < 0)
                Config.StockTrendChancePercentage = 0;
            else if (trendChancePerStock > 100)
                Config.StockTrendChancePercentage = 100;

            // -1 is infinite, no point in going lower
            var maxTrends = Config.MaxTrends;
            if (maxTrends < -1)
                Config.MaxTrends = -1;

            var pastHistoricalDataVolatility = Config.PastHistoricalDataVolatility;
            if (pastHistoricalDataVolatility < 1.0)
                Config.PastHistoricalDataVolatility = 1.0;
        }
    }
}
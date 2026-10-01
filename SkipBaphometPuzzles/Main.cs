using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace SkipBaphometPuzzles
{
    public class Main
    {
        public static bool Enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        [HarmonyPatch(typeof(BlueprintsCache))]
        static class BlueprintsCache_Patches
        {
            static bool loaded = false;
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            static void Postfix(ref BlueprintsCache __instance)
            {
                if (loaded) return;
                loaded = true;

                ShieldMazePuzzleHelper.SkipPuzzle(ref __instance);


            }
        }


    }
}

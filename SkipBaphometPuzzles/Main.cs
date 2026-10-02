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

                ShieldMazePuzzleHelper.SkipPuzzle(ref __instance);      //盾牌迷宫
                DefendersHeartPuzzleHelper.SkipPuzzle(ref __instance);  //铁卫雄心
                GreyGarrisonPuzzleHelper.SkipPuzzle(ref __instance);    //灰兵营
                LostChapelPuzzleHelper.SkipPuzzle(ref __instance);      //失陷教堂
                CitadelDrezenPuzzleHelper.SkipPuzzle(ref __instance);   //眷泽城要塞
                SacredLandsPuzzleHelper.SkipPuzzle(ref __instance);     //圣地


            }
        }


    }
}

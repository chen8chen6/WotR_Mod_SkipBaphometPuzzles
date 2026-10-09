using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.EventConditionActionSystem.Events;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class DLC2_TheatrePuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Complete all etudes which oversee lever states
            ActionList stopAllTriggers = new ActionList { Actions = new GameAction[TRIGGER_BRIDGE_TO_MOVE.Length * 2] };
            for (int i = 1; i < TRIGGER_BRIDGE_TO_MOVE.Length; ++i)
            {
                stopAllTriggers.Actions[i * 2 - 2] = StopTrigger(TRIGGER_BRIDGE_TO_MOVE[i]);
                stopAllTriggers.Actions[i * 2 - 1] = StopTrigger(TRIGGER_BRIDGE_TO_RESET[i]);
            }

            //Lower all bridges down
            ActionList lowerAllBridges = new ActionList();
            int[] bridgesToLower = { 1, 2, 3, 5, 6, 7, 10, 11 };

            foreach (int idx in bridgesToLower)
                AppendActions(ref lowerAllBridges, TRIGGER_BRIDGE_TO_MOVE[idx], ref __instance);

            //Register an actionHolder with actions above
            var skipPuzzle = new ActionsHolder() {
                Actions = new ActionList(stopAllTriggers, lowerAllBridges),
                AssetGuid = BlueprintGuid.Parse(BPHelper.MOD_GUID_DLC2_THEATRE_SKIP_PUZZLE),
                name = "MOD_SBP_DLC2_Theater_SkipPuzzle",
            };
            __instance.AddCachedBlueprint(skipPuzzle.AssetGuid, (SimpleBlueprint)skipPuzzle);   //add to BlueprintsCache

            //Set every trigger to run the actionHolder
            var runSkipPuzzle = new ActionList() { Actions = new GameAction[1]};
            runSkipPuzzle.Actions[0] = new RunActionHolder() { Holder = skipPuzzle.ToReference<ActionsReference>() };

            for (int i = 1; i < TRIGGER_BRIDGE_TO_MOVE.Length; ++i)
            {
                SetActions(TRIGGER_BRIDGE_TO_MOVE[i], runSkipPuzzle, ref __instance);
                SetActions(TRIGGER_BRIDGE_TO_RESET[i], runSkipPuzzle, ref __instance);
            }
        }

        static string[] TRIGGER_BRIDGE_TO_MOVE = {
            null,
            "1869f56713b043bdbc412c984b34d2db",     //State_1
            "1c52802a660f438989c9a617e3fc991d",     //2State_1
            "228329af3a214853bb767940d56dac46",     //3State_1
            "2dda2f9255264effb20fcff954c80b70",     //4State_1
            "6c5744f4c70948078a50e9a41d141056",     //5State_1
            "f761fb89d9e742e1910c3e31103da66f",     //6State_1
            "b9f524ef8fb04e938152aa9d7b70a534",     //7State_1
            "4f214e295c314bd6bfe7452c42c84219",     //8State_1
            "e86a8824e0524536b57c0788be3a0cb8",     //9State_1
            "61fd067a132e4661bfbe096a13b05f36",     //10State_1
            "dc30dfc4a00f48238104036c51911d93",     //11State_1
        };

        static string[] TRIGGER_BRIDGE_TO_RESET = {
            null,
            "3ca11886de0d40159dabc8cbe3633bc7",     //State_0
            "41f42b3427f649038b59a57dceafbd56",     //2State_0
            "6fa2d2661265403995a43c454d541a29",     //3State_0
            "833a1583568742e7a0af56f3da474671",     //4State_0
            "3f71044b097c465f815e71fbd6b9e9bf",     //5State_0
            "6a911a6df3a24d19b1c2ca7cb7e3efd4",     //6State_0
            "980b34e8b3254114ac27ac20205b2339",     //7State_0
            "b04c75c64c4c4cff83b6a8ae28216ae4",     //8State_0
            "f811b1e6397643f4916d0d6af1e3bc62",     //9State_0
            "1f815765df0d4d7ba19f731dfa064081",     //10State_0
            "650d4346f2534e25abbb618156164e67",     //11State_0
        };

        static private void AppendActions(ref ActionList actions, string guid, ref BlueprintsCache __instance)
        {
            var etude = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(guid);
            var trigger = (EtudePlayTrigger)etude.ComponentsArray[0];
            actions = new ActionList(actions, trigger.Actions);
        }

        static private void SetActions(string guid, ActionList actions, ref BlueprintsCache __instance)
        {
            var st = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(guid);
            var tg = (EtudePlayTrigger)st.ComponentsArray[0];
            tg.Actions = actions;
        }

        static private CompleteEtude StopTrigger(string guid)
        {
            var monitor = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(guid);
            return new CompleteEtude() { Etude = monitor.ToReference<BlueprintEtudeReference>() };
        }
    }
}

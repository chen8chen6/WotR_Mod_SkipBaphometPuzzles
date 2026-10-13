using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class WizardsTowerPuzzleHelper
    {
        //原逻辑: 每根火把(DLC5_GoblinsPartyInTheTower_Puzzle_Torch_[1,...,6]_[ON,OFF])都维护自己的状态(DLC5_GoblinsPartyInTheTower_Puzzle_Torch_[1,...,6])
        //        检测到只有2, 6点燃时, 播放DLC5_GoblinsPartyInTheTower_Puzzle_DoorOpen显示密门
        //现逻辑: 与任意火把交互, 都把6根火把的状态设置到能开门的组合.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Switch torchs to expect state
            var switchTorch = new ActionList();
            switchTorch.Actions = new GameAction[]{
                SwitchTorch(TORCH_OFF[1], ref __instance),
                SwitchTorch(TORCH_ON[2], ref __instance),
                SwitchTorch(TORCH_OFF[3], ref __instance),
                SwitchTorch(TORCH_OFF[4], ref __instance),
                SwitchTorch(TORCH_OFF[5], ref __instance),
                SwitchTorch(TORCH_ON[6], ref __instance),
            };

            //Register the actionHolder
            var skipPuzzle = new ActionsHolder {
                Actions = switchTorch,
                AssetGuid = BlueprintGuid.Parse(BPHelper.MOD_GUID_DLC5_WIZARD_TOWER_SKIP_PUZZLE),
                name = "MOD_SBP_DLC5_WizardTower_SkipPuzzle",
            };
            __instance.AddCachedBlueprint(skipPuzzle.AssetGuid, (SimpleBlueprint)skipPuzzle);

            //Run skip puzzle on any torch touched
            var runSkipPuzzle = new ActionList() { Actions = new GameAction[1] };
            runSkipPuzzle.Actions[0] = new RunActionHolder()
            {
                Holder = skipPuzzle.ToReference<ActionsReference>(),
                Comment = skipPuzzle.NameSafe(),
            };

            for (int i = 1; i < TORCH_ON.Length; ++i)
            {
                BPHelper.SetActions(TORCH_ON[i], runSkipPuzzle, ref __instance);
                BPHelper.SetActions(TORCH_OFF[i], runSkipPuzzle, ref __instance);
            }
        }

        static private string[] TORCH_ON = {
            null,
            "74329aeef7434262beb2245ac8757d74",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_1_ON
            "059be3c024994ab4ad1bec721be215ce",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_2_ON
            "d8363414b38c413e8cfe456c26671b78",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_3_ON
            "26afc9cb6922485bb06845e0564af148",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_4_ON
            "e9064b05c8b94b59855f92707683720d",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_5_ON
            "7be3a5ba24994b7082dc947f40022be8",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_6_ON
        };
        static private string[] TORCH_OFF = {
            null,
            "8fb5ed51b35c4ab9b28deefdee0f764d",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_1_OFF
            "f96be67b401746bf8001482da998828b",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_2_OFF
            "3ba7862d6a894dcfa8edbced75598bec",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_3_OFF
            "e396f39c82604adba00c8907092aede5",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_4_OFF
            "508042d7b07a452d979b0e58be86dff0",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_5_OFF
            "76c58f20730e4bf5a7bd88ba6538c365",     //DLC5_GoblinsPartyInTheTower_Puzzle_Torch_6_OFF
        };

        static private GameAction SwitchTorch(string guid, ref BlueprintsCache __instance)
        {
            var ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            return ah.Actions.Actions[2];
        }
    }
}

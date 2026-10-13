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
    internal class GreyGarrisonPuzzleHelper
    {
        //原逻辑: 雕像在切换On/Off时, 会使用Statue0[1...6]_State记录激活状态,
        //        同时维护StatuePuzzleVariable, 其值只在雕像激活顺序正确时+1.
        //        当杨妮雕像被激活时,如果其它雕像都已激活, 且StatuePuzzleVariable==5, 执行开门动作.
        //现逻辑: 任意雕像在切换On/Off时, 直接执行开门动作.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Actions to open the door
            var checkCombination = BPHelper.GetChecker(YANIEL_STATUE_ON, 1, ref __instance);
            var openDoor = checkCombination.IfTrue;

            //Set actions on switching statues to openDoor;
            foreach (string statue in STATUES)
                BPHelper.SetActions(statue, openDoor, ref __instance);
        }

        static private string[] STATUES = {
            //Golfrey
            "1e77061c4862164428a08bbc5716bd5d", //Statue01_Actions_On
            "e9432adbdb0b8f24d939fa844d7fbf44", //Statue01_Actions_Off

            //Herald
            "56441fa925d646c4e8d5c3d156ea76fd", //Statue02_Actions_On
            "240c12f6b04bb7c41a3dd7ec3bdd65e3", //Statue02_Actions_Off

            //Lariel
            "41c47b59341f04c4c806c315faf1f2b1", //Statue03_Actions_On
            "ceb7c620963e537428aa2248b188140f", //Statue03_Actions_Off

            //Targona
            "8008378c849d6f443916c74f89d02d54", //Statue04_Actions_On
            "e1062339a9b4f5f4b9dc29d820a3c152", //Statue04_Actions_Off

            //Zakarius
            "29ab7e1f6ce690543a670ce093fd3bdb", //Statue05_Actions_On
            "15b4a7f8d4b60724e93ae73f1a3b313a", //Statue05_Actions_Off

            //Yaniel
            "e125f45902ec2394297966635517a721", //Statue06_Actions_On
            "f94ddfc861a9c074986a4de2dc2ead9e", //Statue06_Actions_Off
        };

        static private string YANIEL_STATUE_ON = STATUES[10];
    }
}

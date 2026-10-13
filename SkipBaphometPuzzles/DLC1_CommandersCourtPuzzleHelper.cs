using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class DLC1_CommandersCourtPuzzleHelper
    {
        //原逻辑: TrashAzata_QuestItemsList_slotcheck_Action在与地面宝箱交互后, 判断置入物品是否正确,
        //        正确则召唤愿望传送门
        //现逻辑: TrashAzata_QuestItemsList_slotcheck_Action无条件召唤愿望传送门
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string CHECK_ITEMS_IN_CHEST = "3c419efbf7bb40fb87f4442e07edaf2c";     //TrashAzata_QuestItemsList_slotcheck_Action;
            var checker = BPHelper.GetChecker(CHECK_ITEMS_IN_CHEST, 0, ref __instance);
            BPHelper.SetCheckerAlwaysReturnTrue(checker);
        }
    }
}

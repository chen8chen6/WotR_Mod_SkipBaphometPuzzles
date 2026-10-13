using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Events;

namespace SkipBaphometPuzzles
{
    internal class DLC1_OtherSideOfThresholdPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            SkipClockPuzzle(ref __instance);
            SkipBinaryPuzzle(ref __instance);
        }

        //原逻辑: DLC1_PuzzleMechanics中包含了场景中各拉杆的互动动作,
        //        当滚轮前的拉杆被拉下时, 检测滚轮的数值是否正确,
        //        是则将DLC1_AxisDungeon_PuzzleDone设为start状态, 显示隐藏区域.
        //现逻辑: 拉杆无条件显示隐藏区域.
        //备注:   正确的数值为7, 10, 1, 4, 实际在场景里显示是6, 3, 12, 9.(即13-N)
        static private void SkipClockPuzzle(ref BlueprintsCache __instance)
        {
            const string PUZZLE_MECHANICS = "84d069f1577e4313a43a0d6a0e494ce5";    //DLC1_PuzzleMechanics
            var axisDungeonLevers = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(PUZZLE_MECHANICS);
            var lever = (GenericInteractionTrigger)axisDungeonLevers.ComponentsArray[0];
            var numChecker = (Conditional)lever.Actions.Actions[0];
            BPHelper.SetCheckerAlwaysReturnTrue(numChecker);
        }

        //原逻辑: ProteansPack2Room_slotcheck_Action在与空槽"A,B,C"交互后, 判断置入物品是否正确,
        //        正确则修复拉杆
        //现逻辑: ProteansPack2Room_slotcheck_Action无条件修复拉杆
        static private void SkipBinaryPuzzle(ref BlueprintsCache __instance)
        {
            const string CHECK_PLATES_IN_SLOTS = "544f01ab021a41029ee815baed6847d6";    //ProteansPack2Room_slotcheck_Action
            var slotChecker = BPHelper.GetChecker(CHECK_PLATES_IN_SLOTS, 0, ref __instance);
            BPHelper.SetCheckerAlwaysReturnTrue(slotChecker);
        }
    }
}

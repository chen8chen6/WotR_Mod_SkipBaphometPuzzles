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
    //原逻辑: 3个邪徽共9个环, 状态分别记录在Puzzle_[Baphomet,Deskari,Nocticula][Big,Medium,Small]Circle这9个变量中.
    //        按下按钮(如: BaphometSmallCircle_LesserButtonActions)时, 先改变变量值,
    //        再调用CompletelyDeactivatePuzzleTrap来判断这9个变量值, 如果都为0则满足条件, 解除英勇之锋的陷阱.
    //现逻辑: CompletelyDeactivatePuzzleTrap永远认为条件满足, 解除英勇之锋的陷阱.
    internal class CitadelDrezenPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string UNHOLY_SYMBOL_CHECKER = "2cc78b0eed55d124f816a3aa2c7e7a3f"; //CompletelyDeactivatePuzzleTrap
            const Condition[] CHECKER_ALWAYS_RETURN_TRUE = null;

            var checker = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(UNHOLY_SYMBOL_CHECKER));
            var checkCombination = (Conditional)checker.Actions.Actions[0];
            checkCombination.ConditionsChecker.Conditions = CHECKER_ALWAYS_RETURN_TRUE;
        }
    }
}

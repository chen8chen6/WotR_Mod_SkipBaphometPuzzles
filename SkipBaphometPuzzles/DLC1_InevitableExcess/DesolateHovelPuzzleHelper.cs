using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class DesolateHovelPuzzleHelper
    {
        //原逻辑: 有4个Etude(DLC1_AreeluHouse_[Vase, Oven, CradleI, TableI]tems)处理对应物件的放入/取出动作,
        //       将对应的计数器(itemsPut[Law,Chaos])进行增减操作.
        //       当DLC1_AreeluHouse_PuzzleDone检测到2个计数器都为2时, 触发Areelu出现.
        //现逻辑: DLC1_AreeluHouse_PuzzleDone无条件触发Areelu出现.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string PARADOX_PUZZLE = "7f583030c8c7485c8c44ca9d6f2659c9";    //DLC1_AreeluHouse_PuzzleDone
            var paradoxChecker = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(PARADOX_PUZZLE);
            BPHelper.SetCheckerAlwaysReturnTrue(paradoxChecker);
        }
    }
}

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
    internal class DLC1_DesolateHovelPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string PARADOX_PUZZLE = "7f583030c8c7485c8c44ca9d6f2659c9";    //DLC1_AreeluHouse_PuzzleDone

            var paradoxChecker = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(PARADOX_PUZZLE);

            BPHelper.SetCheckerAlwaysReturnTrue(paradoxChecker);
            //这种改法直接就解锁了 TODO: 用reflection
        }
    }
}

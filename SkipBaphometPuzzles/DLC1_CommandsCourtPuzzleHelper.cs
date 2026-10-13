using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class DLC1_CommandsCourtPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string CHECK_ITEMS_IN_CHEST = "3c419efbf7bb40fb87f4442e07edaf2c"; //3c419efbf7bb40fb87f4442e07edaf2c;
            var checker = BPHelper.GetChecker(CHECK_ITEMS_IN_CHEST, 0, ref __instance);
            BPHelper.SetCheckerAlwaysReturnTrue(checker);
        }
    }
}

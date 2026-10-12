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

        static private void SkipClockPuzzle(ref BlueprintsCache __instance)
        {
            const string PUZZLE_MECHANICS = "84d069f1577e4313a43a0d6a0e494ce5";    //DLC1_PuzzleMechanics
            var axisDungeonLevers = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(PUZZLE_MECHANICS);
            var lever = (GenericInteractionTrigger)axisDungeonLevers.ComponentsArray[0];
            var numChecker = (Conditional)lever.Actions.Actions[0];
            BPHelper.SetCheckerAlwaysReturnTrue(numChecker);
        }

        static private void SkipBinaryPuzzle(ref BlueprintsCache __instance)
        {
            const string CHECK_PLATES_IN_SLOTS = "544f01ab021a41029ee815baed6847d6";    //ProteansPack2Room_slotcheck_Action
            var slotChecker = BPHelper.GetChecker(CHECK_PLATES_IN_SLOTS, 0, ref __instance);
            BPHelper.SetCheckerAlwaysReturnTrue(slotChecker);
        }
    }
}

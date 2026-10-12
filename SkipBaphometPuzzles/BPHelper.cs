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
    internal class BPHelper
    {
        static public string MOD_GUID_DLC2_THEATRE_SKIP_PUZZLE = "D1505EE1A6E448B4AE84660FB712F280";
        static public string MOD_GUID_DLC5_WIZARD_TOWER_SKIP_PUZZLE = "661040DD5BF24CC68E5627407660531D";

        static public void SetActions(string guid, ActionList actions, ref BlueprintsCache __instance)
        {
            if ("" == guid || null == guid)
                return;
            ActionsHolder ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            ah.Actions = actions;
        }

        static public Conditional GetChecker(string guid, int idx, ref BlueprintsCache __instance)
        {
            return (Conditional)GetAction(guid, idx, ref __instance);
        }

        static public GameAction GetAction(string guid, int idx, ref BlueprintsCache __instance)
        {
            var ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            return ah.Actions.Actions[idx];
        }

        static public void SetCheckerAlwaysReturnTrue(Conditional checker)
        {
            checker.ConditionsChecker.Conditions = null;
        }
    }
}

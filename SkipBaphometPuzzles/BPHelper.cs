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
        static public void SetActions(string guid, ActionList actions, ref BlueprintsCache __instance)
        {
            ActionsHolder ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            ah.Actions = actions;
        }

        static public Conditional GetChecker(string guid, int idx, ref BlueprintsCache __instance)
        {
            var ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            return (Conditional)ah.Actions.Actions[idx];
        }
    }
}

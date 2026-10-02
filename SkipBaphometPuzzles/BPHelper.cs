using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
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
    }
}

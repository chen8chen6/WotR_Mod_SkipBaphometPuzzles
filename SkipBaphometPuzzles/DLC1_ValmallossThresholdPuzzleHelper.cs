using Kingmaker.AreaLogic.Cutscenes;
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
    internal class DLC1_ValmallossThresholdPuzzleHelper
    {
        //原逻辑: 3个装置(ClockDevice[A,B,C]_Actions)分别控制时分秒,
        //       当Way2OrderCheck_Actions检测到时分秒全为6时, 打开通往恒则兵的道路.
        //       当Way2FinalCheck_Actions检测到时分秒全为12时, 打开通往DLC最终战场的道路.
        //现逻辑: 当Way2OrderCheck_Actions无条件打开通往恒则兵的道路, 然后打开通往DLC最终战场的道路.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string CHECK_WAY_2_ORDER = "efaab9510d5b4afbaa90fc8b41419b3c";      //Way2OrderCheck_Actions
            string CHECK_WAY_2_FINAL = "670aaa238369423c8c48f54d3b238481";      //Way2FinalCheck_Actions
            string SCENE_WAY_2_ORDER = "2a38f0ecbc8049e5ac60d5e93f8f05ae";      //ClockPuzzleAll6
            string SCENE_WAY_2_FINAL = "d25cfcada62447a1958914e4c6b99068";      //ClockPuzzleAll12

            //Reveal path to order on any device interacted
            var checkWay2Order = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(CHECK_WAY_2_ORDER));
            foreach (GameAction checker in checkWay2Order.Actions.Actions)
            {
                BPHelper.SetCheckerAlwaysReturnTrue((Conditional)checker);
            }

            //Reveal path to final on path to order revealed
            var checkWay2Final = BPHelper.GetChecker(CHECK_WAY_2_FINAL, 0, ref __instance);
            GameAction revealWay2Final = checkWay2Final.IfTrue.Actions[0];
            var sceneWay2Order = (Cutscene)__instance.Load(BlueprintGuid.Parse(SCENE_WAY_2_ORDER));
            sceneWay2Order.OnStopped = new ActionList() {
                Actions = new GameAction[1] { revealWay2Final }
            };

            //Disable clock device interaction after paths revearled
            var sceneWay2Final = (Cutscene)__instance.Load(BlueprintGuid.Parse(SCENE_WAY_2_FINAL));
            for (int i = 8; i <= 10; ++i)
            {
                var cmds = (CommandAction)sceneWay2Final.StartedTracks[1].Commands[2];
                var switchInteraction = (SwitchInteraction)cmds.Action.Actions[i];
                switchInteraction.DisableIfAlreadyEnabled = true;
                switchInteraction.EnableIfAlreadyDisabled = false;
            }
        }
    }
}

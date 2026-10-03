using AK.Wwise;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Events;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class SacredLandsPuzzleHelper
    {
        //原逻辑: 圣地内有9种颜色的按钮共18个, 全都保存在一个叫Puzzle的BuleprintComponentList里.
        //        按下按钮时, 先切换同色的桥的状态, 再切换同色的另一个按钮的状态,
        //        再切换同色的灯的状态, 并将灯状态记录在[Blue,Yellow,Orange...]LampOn中,
        //        最后调用Conditional (Open Final Door )检查, 如果所有的LampOn都解锁,
        //        则升起最后的桥, 并将FinalDoorOpen设置为0.
        //现逻辑: 按下任意按钮, 都会升起所有桥.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string PUZZLE = "34a3dfdf62077474293446264a5ff489";   //puzzle
            const int COLOR_NUM = 9;
            const int COLOR_BRIGE_NUM = 2 * COLOR_NUM;
            const int BTN_NUM = 2 * COLOR_NUM;

            ActionList riseAllBridges = new ActionList() { Actions = new GameAction[COLOR_BRIGE_NUM] };
            var bridgePuzzle = ResourcesLibrary.TryGetBlueprint<BlueprintComponentList>(PUZZLE);

            //find actions to rise every bridge
            for (int idx = 0; idx < BTN_NUM; idx += 2)
            {
                var btn = (GenericInteractionTrigger)bridgePuzzle.ComponentsArray[idx];

                var riseBridge1 = (SwitchDoor)btn.Actions.Actions[0];
                var riseBridge2 = (SwitchDoor)btn.Actions.Actions[1];
                SetBridgeAlwaysRisen(ref riseBridge1);
                SetBridgeAlwaysRisen(ref riseBridge2);
                riseAllBridges.Actions[idx] = riseBridge1;
                riseAllBridges.Actions[idx + 1] = riseBridge2;

                if (COLOR_NUM - 1 == idx)
                {
                    var checkAllLampOn= (Conditional)btn.Actions.Actions[6];
                    var riseFinalBridge = checkAllLampOn.IfTrue;
                    riseAllBridges = new ActionList(riseAllBridges, riseFinalBridge);
                }
            }

            //Rise all bridges on any btn pressed
            for (int idx = 0; idx < BTN_NUM; ++idx)
            {
                var btn = (GenericInteractionTrigger)bridgePuzzle.ComponentsArray[idx];
                btn.Actions = riseAllBridges;
            }
        }

        static private void SetBridgeAlwaysRisen(ref SwitchDoor bridge)
        {
            bridge.OpenIfAlreadyClosed = false;
        }
    }
}

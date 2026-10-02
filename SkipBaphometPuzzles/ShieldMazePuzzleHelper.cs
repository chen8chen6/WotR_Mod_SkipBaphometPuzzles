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
    internal class ShieldMazePuzzleHelper
    {
        //原逻辑: 按下按钮时, 将按下的按键依次记录在slot_torture_1,2,3,4中, 同时Pass_Torture_2计数器+1.
        //        计数器==4时, 播放名为Pass_Torture_2(与计数器同名但guid不同)的cutscene.
        //        这个cutscene判断按钮组合是否正确, 正确则开门, 失败则清空上述计数器和按键记录.
        //现逻辑: 按下任意按钮时, 直接开门
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            const string BTN_RED = "eecff33136ed22c4698824aa73575b83"; //Button_Red_Torture_2
            const string BTN_BLUE = "2b7e48fc9100e0d4d81410a991b1a86c"; //Button_Blue_Torture_2
            const string BTN_GREEN = "35f92ec46b309d9438755514839d6158"; //Button_Green_Torture_2
            const string BTN_YELLOW = "3e1882a89d2ff2c4ba3a09ecd99a1dcb"; //Button_Yellow_Torture_2
            const string CUTSCENE_OPEN_DOOR = "7a6ffa94447ce1d4284d45c1dce4ff28"; //Pass_Torture_2

            //find actions which open the door
            var cutscene_check_combination = ResourcesLibrary.TryGetBlueprint<Cutscene>(CUTSCENE_OPEN_DOOR);
            CommandAction cmd_check_combination = (CommandAction)cutscene_check_combination.StartedTracks[0].Commands[1];
            var check_combination_slot1 = (Conditional)cmd_check_combination.Action.Actions[0];
            var check_combination_slot2 = (Conditional)check_combination_slot1.IfTrue.Actions[0];
            var check_combination_slot3 = (Conditional)check_combination_slot2.IfTrue.Actions[0];
            var check_combination_slot4 = (Conditional)check_combination_slot3.IfTrue.Actions[0];
            var actionList_openDoor = check_combination_slot4.IfTrue.Actions;

            //set action on btn pressed to opendoor
            var btn_r = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_RED));
            var btn_b = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_BLUE));
            var btn_g = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_GREEN));
            var btn_y = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_YELLOW));
            btn_r.Actions.Actions = actionList_openDoor;
            btn_b.Actions.Actions = actionList_openDoor;
            btn_g.Actions.Actions = actionList_openDoor;
            btn_y.Actions.Actions = actionList_openDoor;
        }
    }
}

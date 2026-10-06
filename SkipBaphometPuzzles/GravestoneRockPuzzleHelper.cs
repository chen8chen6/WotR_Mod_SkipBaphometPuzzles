using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.TurnBasedMode.ActionsState;

namespace SkipBaphometPuzzles
{
    internal class GravestoneRockPuzzleHelper
    {
        //原逻辑: 按下按钮Button_[Red,Blue,Green,Yellow]_Password时, 将按下的按键依次记录在Password_Slot_[1,...,4]中,
        //        同时Password_Combo计数器+1. 计数器==4时, 播放名为Door_Password_Check的cutscene.
        //        这个cutscene判断按钮组合是否正确, 正确则开门,同时将当前4个按钮隐藏,
        //        原地显示4个新按钮Button_[Red,Blue,Green,Yellow]_ReverseCode, 这4个按钮用的计数器和变量都跟_Password一样,
        //        最后执行一个叫做ReverseCode_Check的ActionsHolder. 这个ActionsHolder播放名为LootTrap_Avoided的cutscene,
        //        同时将PuzzleInHideout这个etude设为completed
        //现逻辑: 按下任意按钮, 同时开门并解除陷阱.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            const string BTN_RED            = "bc1223cd8b0bfe94ebef48b2a7ebe8a4"; //Button_Red_Password
            const string BTN_BLUE           = "3cbec13e54ce0b0429782f1577a8a980"; //Button_Blue_Password
            const string BTN_GREEN          = "1438c0a7624fa4942bea64bea9bb9a2c"; //Button_Green_Password
            const string BTN_YELLOW         = "a55946ce07e51ae4d88a0d0d6bb75f5b"; //Button_Yellow_Password
            const string CODE_CHECKER       = "2baa51c86793656459315df1c297eef9"; //Password_Check
            const string REV_CODE_CHECKER   = "7f8444434857a4b479f8ad0ca43b515e"; //ReverseCode_Check

            ActionList openDoor = ActionsOnCheckPass(CODE_CHECKER, ref __instance);
            ActionList avoidTrap = ActionsOnCheckPass(REV_CODE_CHECKER, ref __instance);
            ActionList doorAndTrap = new ActionList(openDoor, avoidTrap);

            //Open door and disarm trap on any button pressed
            BPHelper.SetActions(BTN_RED, doorAndTrap, ref __instance);
            BPHelper.SetActions(BTN_BLUE, doorAndTrap, ref __instance);
            BPHelper.SetActions(BTN_GREEN, doorAndTrap, ref __instance);
            BPHelper.SetActions(BTN_YELLOW, doorAndTrap, ref __instance);
        }

        static private ActionList ActionsOnCheckPass(string guid, ref BlueprintsCache __instance)
        {
            Conditional check_combination_slot_1 = BPHelper.GetChecker(guid, 0, ref __instance);
            Conditional check_combination_slot_2 = (Conditional)check_combination_slot_1.IfTrue.Actions[0];
            Conditional check_combination_slot_3 = (Conditional)check_combination_slot_2.IfTrue.Actions[0];
            Conditional check_combination_slot_4 = (Conditional)check_combination_slot_3.IfTrue.Actions[0];
            return  check_combination_slot_4.IfTrue;
        }


    }
}

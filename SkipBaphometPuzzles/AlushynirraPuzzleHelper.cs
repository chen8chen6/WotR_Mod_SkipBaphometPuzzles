using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class AlushynirraPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            SkipMidCityPuzzle(ref __instance);
            SkipUpperCityPuzzle(ref __instance);
        }

        //原逻辑: 五块平台的状态记录在RotatingPlatform[1,...,5]_Position中, 按下按钮Button[1,2]_CheckPassedActions时,
        //        会分别让这些平台翻转一定角度, 当对应的Position为0时, 将堵住该地块的透明障碍移除, 使得此处可以通行
        //现逻辑: 按下按钮时, 将所有平台的状态设为0, 使得整条通路可通行.
        //备注:   直接用setDevStatus翻转平台的话, 有诸多限制条件(只能翻转特定角度且无法连续翻转),
        //        转动动画难以正常播放, 因此直接将平台隐藏, 设置好再显示
        static private void SkipMidCityPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            const string BTN_EAST = "601b9d2d7d4ecfd4482789ebc74f9ef4"; //Button1_CheckPassedActions
            const string BTN_WEST = "a058fae2b60be214f85adfcdc59196b2"; //Button2_CheckPassedActions

            var btnEast = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_EAST));
            var btnWest = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_WEST));

            //btnEast.actions.actions[] = {incrementFlag_[1,...,5], setDevStatus_[1,...,5], checkOverFlow_[1,...,5], navmecCuts, finalPlat}
            var navmecCuts = (RunActionHolder)btnEast.Actions.Actions[15];
            var finalPlat = (Conditional)btnEast.Actions.Actions[16];

            //pathToChest.actions[] = {hidePlat_[1,...,5], setFlag_[1,...,5], setDevStatus_[1,...,5], showPlat_[1,...,5], navmecCuts, finalPlat}
            const int PLATFORM_NUM = 5;
            const int IDX_SET_FLAG = PLATFORM_NUM;
            const int IDX_SET_PLAT = IDX_SET_FLAG + PLATFORM_NUM;
            const int IDX_SHOW_PLAT = IDX_SET_PLAT + PLATFORM_NUM;
            const int IDX_CUTS = IDX_SHOW_PLAT + PLATFORM_NUM;

            ActionList pathToChest = new ActionList() { Actions = new GameAction[PLATFORM_NUM * 4 + 2] };
            for (int i = 0; i < PLATFORM_NUM; ++i)
            {
                var setFlag = ((Conditional)btnEast.Actions.Actions[PLATFORM_NUM + i]).IfTrue.Actions[0];
                var setPlat = (SetDeviceState)btnEast.Actions.Actions[PLATFORM_NUM * 2 + i];
                var objPlat = setPlat.Device;

                pathToChest.Actions[i] = SetPlatVisable(objPlat, false);
                pathToChest.Actions[IDX_SET_FLAG + i] = setFlag;
                pathToChest.Actions[IDX_SET_PLAT + i] = setPlat;
                pathToChest.Actions[IDX_SHOW_PLAT + i] = SetPlatVisable(objPlat, true);
            }
            pathToChest.Actions[IDX_CUTS] = navmecCuts;
            pathToChest.Actions[IDX_CUTS + 1] = finalPlat;

            btnEast.Actions = pathToChest;
            btnWest.Actions = pathToChest;
        }

        //原逻辑: 同中城区. 使用RotatingPlatformHigher[1,...,5]_Position记录平台状态,
        //              HigherButton[1,2]_CheckPassedActions执行交互
        //现逻辑: 同中城区.
        //备注:   上城区的宝箱有时不能开, 陷阱检测到也无法交互, 怀疑是因为用百宝袋开过宝箱, 或者陷阱检测/显示机制的问题
        static private void SkipUpperCityPuzzle(ref BlueprintsCache __instance)
        {
            const string BTN_WEST = "b1fff861ca9511a4fb91f9d8c60371a4"; //HigherButton1_CheckPassedActions
            const string BTN_EAST = "4e4b41fd8fd7908428fed759be779037"; //HigherButton2_CheckPassedActions
            const string SET_DEVS = "2160a6a9322e033418dec3506c327eaa"; //PuzzleSetStateHigher_Holder

            var btnWest = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_WEST));
            var btnEast = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(BTN_EAST));
            var devs = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(SET_DEVS));

            //btnWest.actions.actions[] = {incrementFlag_[1,...,5], checkOverFlow_[1,...,5], setStatus(AcitonsHolder), navmecCuts(ActionHolder)}
            var setDevStatus = (RunActionHolder)btnWest.Actions.Actions[10];
            var navmecCuts = (RunActionHolder)btnWest.Actions.Actions[11];

            //pathToChest.actions[] = {hidePlat_[1,...,5], setFlag_[1,...,5], setStatus(AcitonsHolder), showPlat_[1,...,5], navmecCuts(ActionHolder)}
            const int PLATFORM_NUM = 5;
            const int IDX_SET_FLAG = PLATFORM_NUM;
            const int IDX_SET_PLAT = IDX_SET_FLAG + PLATFORM_NUM;
            const int IDX_SHOW_PLAT = IDX_SET_PLAT + 1;
            const int IDX_CUTS = IDX_SHOW_PLAT + PLATFORM_NUM;

            ActionList pathToChest = new ActionList() { Actions = new GameAction[PLATFORM_NUM * 3 + 3] };
            for (int i = 0; i < PLATFORM_NUM;  ++i)
            {
                var setFlag = ((Conditional)btnWest.Actions.Actions[PLATFORM_NUM + i]).IfTrue.Actions[0];
                var setDev = (SetDeviceState)devs.Actions.Actions[i];
                var objPlat = setDev.Device;

                pathToChest.Actions[i] = SetPlatVisable(objPlat, false);
                pathToChest.Actions[IDX_SET_FLAG + i] = setFlag;
                pathToChest.Actions[IDX_SHOW_PLAT + i] = SetPlatVisable(objPlat, true);
            }
            pathToChest.Actions[IDX_SET_PLAT] = setDevStatus;
            pathToChest.Actions[IDX_CUTS] = navmecCuts;

            btnWest.Actions = pathToChest;
            btnEast.Actions = pathToChest;
        }

        static private HideMapObject SetPlatVisable(MapObjectEvaluator plat, bool visable)
        {
            return new HideMapObject() { MapObject = plat, Unhide = visable };
        }
    }
}

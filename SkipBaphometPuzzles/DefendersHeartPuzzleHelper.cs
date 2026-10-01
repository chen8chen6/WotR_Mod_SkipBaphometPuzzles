using Kingmaker;
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
    internal class DefendersHeartPuzzleHelper
    {
        //原逻辑: 摘下墙上的盾牌时, 显示隐藏按钮. 因为找不到该按钮的actionsHolder, 其后流程不明
        //现逻辑: 摘下盾牌时, 将拉杆设置到正确位置. 需要玩家手动再点下按钮开门
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            const string ON_SHIELD_TAKEN = "c7096aaef3491f246b5aee9ec8446ac1"; //PuzzleLoot_SheildAction
            const string LEVER_WEST = "a6bdaaae49744274b9717f08c581ca97";   //PuzzleFlag_Lever_1
            const string LEVER_EAST = "ad06a535305bc3345b02d642cef33d93";   //PuzzleFlag_Lever_2
            const string LEVER_SOUTH = "96aa93cc8c435cd4199a67f158115b50";  //PuzzleFlag_Lever_3
            const int LEVER_DOWN = 1;
            const int LEVER_UP = 2;

            //GameActions used to set flags
            var setLeverW = new UnlockFlag();
            setLeverW.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(LEVER_WEST);
            setLeverW.flagValue = LEVER_DOWN;

            var setLeverE = new UnlockFlag();
            setLeverE.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(LEVER_EAST);
            setLeverE.flagValue = LEVER_UP;

            var setLeverS = new UnlockFlag();
            setLeverS.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(LEVER_SOUTH);
            setLeverS.flagValue = LEVER_DOWN;

            //Run actions above on shield taken from wall
            var actionsOnShieldTaken = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(ON_SHIELD_TAKEN));
            //pzshield.Actions.Actions.Append(unlockFlag_LeverW);   //TODO: Append接口无效的原因?
            actionsOnShieldTaken.Actions.Actions = new GameAction[4] { actionsOnShieldTaken.Actions.Actions[0], setLeverW, setLeverE, setLeverS };
        }
    }
}

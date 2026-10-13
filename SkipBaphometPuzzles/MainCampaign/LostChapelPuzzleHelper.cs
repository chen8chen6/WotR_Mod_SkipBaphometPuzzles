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
    internal class LostChapelPuzzleHelper
    {
        //原逻辑: 地图, 六分仪, 竖琴放入德丝娜祭坛, 会激活3个交互按钮(Desna[Maps,Astrolabe,Harp]Object_CheckPassedActions)
        //        三个按钮共同维护一个叫AzataMelodyPuzzle的变量, 该变量只有弹奏顺序正确时才+1, 否则被归零.
        //        与竖琴交互时, 如果AzataMelodyPuzzle==3, 则播放cutscene ShortMelody3.
        //        推测由ShortMelody3检测AzataMelodyPuzzle并触发后续流程.
        //现逻辑: 与任意乐器交互均可直接播放ShortMelody3.
        //备注:   本来有打算检测背包/祭坛里是否有3样任务物品. 但考虑到没看攻略的使用场景,
        //        应该是玩家路过时点下祭坛,这时就应该完成解谜. 而不是拿到任务物品后回来摆供品唱歌.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            const string DESNA_MAP          = "656be13224f59c84da5bc3b04740c48b"; //DesnaMapsObject_CheckPassedActions
            const string DESNA_ASTROLABE    = "2b83907e2839fc843aa7e1362858b360"; //DesnaAstrolabeObject_CheckPassedActions
            const string DESNA_HARP         = "a379beabeb5ce8b4f8c28240d45d350d"; //DesnaHarpObject_CheckPassedActions

            //Actions on correct melody combination
            var lastMelody = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(DESNA_HARP));
            var checkCombination = (Conditional)lastMelody.Actions.Actions[0];
            var playAzataMelody = checkCombination.IfTrue;

            //Sing azata song on any musical instrument played
            BPHelper.SetActions(DESNA_MAP, playAzataMelody, ref __instance);
            BPHelper.SetActions(DESNA_ASTROLABE, playAzataMelody, ref __instance);
            BPHelper.SetActions(DESNA_HARP, playAzataMelody, ref __instance);
        }
    }
}

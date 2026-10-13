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
    internal class AreelusLabPuzzleHelper
    {
        //原逻辑: 依次点击摇篮->儿童绘画->魔法课本时, 变量ALR_TE_Sequence会逐渐增加, 
        //        最后点击课本时, 检测ALR_TE_Sequence是否为2, 如果是的话显示柜子里的手套.
        //        拾起手套时, 将一个名为TE_ALR_CorrectSequence的etude设为started.
        //        结局对话时会检测该etude, 解锁对话"你真的先拿了我孩子最喜欢的东西"
        //        期间会出现一些干扰物件, 点击时会将ALR_TE_Sequence锁定或设为0, 这样点击课本时就无法解锁手套了.
        //现逻辑: 点击摇篮(一开始只有这个可点击)时, 显示所有隐藏物件(包括传送门),
        ///       同时将TE_ALR_CorrectSequence设为started

        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            const string CREDLE         = "962afd3fab984d545afff02d5f7d4157";   //ALR_1Cradle_CheckPassedActions
            const string DRAWING        = "c03eb41e7df93254a8da391a9f7e186b";   //ALR_2ChildDrawings_CheckPassedActions
            const string SCHOOL_BOOK    = "166fb9b75206e714a81bd9b66d25e5e1";   //ALR_3MagicSchoolbook_CheckPassedActions
            const string GLOVES         = "44158859b2b0d6f408f2c02454d2c11b";   //ALR_TE_Gloves_Action

            var credle = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(CREDLE));
            var drawing = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(DRAWING));
            var book = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(SCHOOL_BOOK));
            var gloves = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(GLOVES));

            //Reveal all objs and start etude TE_ALR_CorrectSequence
            ActionList revealAll = new ActionList(drawing.Actions, book.Actions);
            revealAll = new ActionList(credle.Actions, revealAll);
            ActionList startTE = gloves.Actions;

            //Do things above on credle interacted
            credle.Actions = new ActionList(revealAll, startTE);
        }
    }
}

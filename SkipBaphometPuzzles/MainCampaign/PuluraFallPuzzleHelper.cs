using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class PuluraFallPuzzleHelper
    {
        //原逻辑: 星座的激活状态记录在Pulura_Fountain_[2, ..., 14]中,
        //        Step_01处于start状态时, 会一直监测星座的状态, 有且仅有星座4(好合/Newly weds)被激活时, 触发Step_02,
        //        此后在激活正确星座时, 依次触发Step_3,4,5,6, 最后由6判断满足密门打开条件.
        //现逻辑: Step_01在任意星座激活时触发Step_02, 随后Step_[N]无条件触发Step_[N+1], 最后Step_06无条件开门.
        //备注:   星座1是界线/Cuse, 是重置按钮, 不适用于开门.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Move to Step_02 on any fountain actived
            BlueprintEtude Step1 = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(STEP[1]);
            Step1.ActivationCondition.Operation = Operation.Or;
            foreach (Condition c in Step1.ActivationCondition.Conditions)
            {
                var checkEtudeIs = (EtudeStatus)c;
                checkEtudeIs.Not = false;
            }

            //Move to Step_[N] on Step_[N-1] starts
            for (int i = 2; i <= 6; ++i)
            {
                var step = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(STEP[i]);
                step.ActivationCondition.Conditions = CHECKER_ALWAYS_RETURE_TRUE;
            }
        }

        static private string PLACE_HOLDER = "";

        static private string[] STEP = {
            PLACE_HOLDER,
            "f537490c22deecf4994728b5d5315e9a", //Step_01
            "cfa2e48f8581b9f43b19382fdf6265b8", //Step_02
            "d6dc70ba45cebc245a5febb2c51ac881", //Step_03
            "98e3d5b6e5ec93a48a8d38f8867a337e", //Step_04
            "7d1cffd44e65b23478f1f496ccaa4582", //Step_05
            "4bf7750e14804f0458dc87747d82dee7", //Step_06
        };

        static private Condition[] CHECKER_ALWAYS_RETURE_TRUE = null;

    }
}

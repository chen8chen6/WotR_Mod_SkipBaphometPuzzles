using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.UI.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static RootMotion.FinalIK.GrounderQuadruped;

namespace SkipBaphometPuzzles
{
    internal class DLC5_ShadowWorldPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            SkipLeverPuzzle(ref __instance);
            SkipDarknessPuzzle(ref __instance);
        }

        enum LV_ST
        {
            UP = 0,
            DOWN,
        };

        static private int Idx_LeverToDown(int leverIdx) { return (leverIdx - 1) * 2; }
        static private int Idx_LeverToUp(int leverIdx) { return Idx_LeverToDown(leverIdx) + 1; }

        //原逻辑: 每根拉杆维护自身的状态, 同时只在按照正确顺序拉动的情况下才能正确交互, 否则将所有拉杆状态重置.
        //        只是多了一步要检测通道上是否存在单位, 存在的话则不做任何动作.
        //现逻辑: 拉下拉杆后, 直接将所有拉杆状态切换到解谜组合1, 再次和拉杆交互, 将所有拉杆状态切换到解谜组合2.
        //备注:   因为每组拉杆有2个解法, 所以需要拉2次拉杆才能拿齐所有奖励.
        static private void SkipLeverPuzzle(ref BlueprintsCache __instance)
        {
            SkipLeverPuzzle_SW(ref __instance);
            SkipLeverPuzzle_NW(ref __instance);
            SkipLeverPuzzle_NE(ref __instance);
            SkipLeverPuzzle_SE(ref __instance);
        }

        //原逻辑: 参考PurulaFallPuzzleHelper.cs
        //现逻辑: 参考PurulaFallPuzzleHelper.cs
        static private void SkipDarknessPuzzle(ref BlueprintsCache __instance)
        {
            string FIRST_LEVER = "f3bb04e1d04d4d28ba71e3d2aa12dc78";    //Lever_Misery_Interaction

            string[] LEVERS = {
                "1ededd9828f4489d87128e951b6c366c",     //Lever_Agony_Interaction
                //"f3bb04e1d04d4d28ba71e3d2aa12dc78",    //Lever_Misery_Interaction
                "0a3b727e18654585bda504bfae4be7d0",     //Lever_Pain_Interaction
                "0fe694b9a48c449985a147d701583c44",     //Lever_Torment_Interaction
                "cfba983a6e9a45088b08d7c0769fccc3",     //Lever_Suffering_Interaction
                "ca542375729b416db1e401f1500d0dbc",     //Lever_Grief_Interaction
            };

            string[] STEP = {
                "07ab27964c704c76bd9069bf8c9bfe2f",     //ChainedDarkness_Puzzle_Step2
                "8fcbd9089cef4d46a9f84b41b2d68213",     //ChainedDarkness_Puzzle_Step3
            };

            const Condition[] CHECKER_ALWAYS_RETUR_TRUE = null;

            var firstLever = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(FIRST_LEVER));

            //move to step2 on any lever pulled
            foreach (string lever in LEVERS)
                BPHelper.SetActions(lever, firstLever.Actions, ref __instance);

            //show release button on step2 starts
            foreach (string step in STEP)
            {
                var s = ResourcesLibrary.TryGetBlueprint<BlueprintEtude>(step);
                s.ActivationCondition.Conditions = CHECKER_ALWAYS_RETUR_TRUE;
            }
        }

        //    A1    A2
        //  A3        A4
        static private void SkipLeverPuzzle_SW(ref BlueprintsCache __instance)
        {
            string[] LEVERS = {
                "660610c0c511465aa12f3bbcb0330b3d", //Leaver_A1
                "0e154e421a4a4eada9c4432e671dfa91", //Leaver_A1_Close
                "12444a62e0ed4d72b4650ce5ab5dae75", //Leaver_A2
                "3288c2da935045959be4b44395ca9125", //Leaver_A2_Close
                "c2bc81f274bc492ba8e42e24ba8ddecb", //Leaver_A3
                "7e04a1c649114a4786e232c276fe3575", //Leaver_A3_Close
                "36612be911a24cb190df40f2a419dfb6", //Leaver_A4
                "68fda352e4f64f9b865fdd468abfbad0", //Leaver_A4_Close
            };

            //243->Path to darkness
            var pathSol = new LV_ST[] { LV_ST.UP, LV_ST.DOWN, LV_ST.DOWN, LV_ST.DOWN };

            //21->Dead Man's Hat
            var lootSol = new LV_ST[] { LV_ST.DOWN, LV_ST.DOWN, LV_ST.UP, LV_ST.UP };

            SkipLeverPuzzle_ChkUnitOnBridge(ref LEVERS, pathSol, lootSol, ref __instance);
        }

        //    B1    B2
        //  B3        X
        static private void SkipLeverPuzzle_NW(ref BlueprintsCache __instance)
        {
            string[] LEVERS = {
                "c4d3b5271cb642d187a9bb40f900ed2a", //Leaver_B1
                "08af740ba9ca4dafa08050f53f29d846", //Leaver_B1_Close
                "48c2a502959f49a9ac35f0cf5b74b2db", //Leaver_B2
                "2fc66185d9cb4c5e91adfc26edbe0082", //Leaver_B2_Close
                "6e95c56f77964d7e8e15054e8bc94691", //Leaver_B3
                "7b070d015aa648bbbfaa89f6de303142", //Leaver_B3_Close
            };

            //21->Deathly Arousal
            var lootSol = new LV_ST[] { LV_ST.DOWN, LV_ST.DOWN, LV_ST.UP };

            //23->Wall ???
            var wallSol = new LV_ST[] { LV_ST.UP, LV_ST.DOWN, LV_ST.DOWN };

            SkipLeverPuzzle(ref LEVERS, lootSol, wallSol, ref __instance);
        }

        //    C1    C2
        //  X         C3
        static private void SkipLeverPuzzle_NE(ref BlueprintsCache __instance)
        {
            string[] LEVERS = {
                "7af9b3ed13d3425f810807942c46c257", //Leaver_C1
                "b8adef3d111b4673893675a71629c725", //Leaver_C1_Close
                "bec16dede60e473288f70f291c2e3ded", //Leaver_C2
                "a03bab4e99cb49499c0115855bc53517", //Leaver_C2_Close
                "4f11ce7ea70242cb890903524c78c7e0", //Leaver_C3
                "9180117506c447fca29409196ef313ee", //Leaver_C3_Close
            };

            //32->Potion of Restoration, Lesser
            var lootSol = new LV_ST[] { LV_ST.UP, LV_ST.DOWN, LV_ST.DOWN };

            //31->Wall ???
            var wallSol = new LV_ST[] { LV_ST.DOWN, LV_ST.UP, LV_ST.DOWN };

            SkipLeverPuzzle(ref LEVERS, lootSol, wallSol, ref __instance);
        }

        //    D1    D2
        //  D3        D4
        static private void SkipLeverPuzzle_SE(ref BlueprintsCache __instance)
        {
            string[] LEVERS = {
                "5151524f57b148f1b32152968140df03", //Leaver_D1
                "edac178e4c0349dfa3a37a0e621025c1", //Leaver_D1_Close
                "44a214b7da4542a18366b46c4c73f393", //Leaver_D2
                "37d821c6126a486c9a94c1fd6e0f8195", //Leaver_D2_Close
                "b400e595c8e84dd4a4e7a02327245f1b", //Leaver_D3
                "99a2cb8688a140008d05e6ed2bd2844a", //Leaver_D3_Close
                "bb1882474ed5493cb1c7b8aa8209d006", //Leaver_D4
                "894524b23dbf487880386f973f5e32f4", //Leaver_D4_Close
            };

            //342-> path to prison
            var pathSol = new LV_ST[] { LV_ST.UP, LV_ST.DOWN, LV_ST.DOWN, LV_ST.DOWN };

            //13->Skinned Leather Cloak
            var lootSol = new LV_ST[] { LV_ST.DOWN, LV_ST.UP, LV_ST.DOWN, LV_ST.UP };

            SkipLeverPuzzle_ChkUnitOnBridge(ref LEVERS, pathSol, lootSol, ref __instance);
        }

        static private void SkipLeverPuzzle(ref string[] levers, LV_ST[] lootSol, LV_ST[] wallSol, ref BlueprintsCache __instance)
        {

            var loot = SetLevers(ref levers, lootSol, ref __instance);
            var wall = SetLevers(ref levers, wallSol, ref __instance);

            int leverNum = levers.Length / 2;
            for (int i = 1; i <= leverNum; ++i)
            {
                BPHelper.SetActions(levers[Idx_LeverToDown(i)], loot, ref __instance);
                BPHelper.SetActions(levers[Idx_LeverToUp(i)], wall, ref __instance);
            }
        }

        static private void SkipLeverPuzzle_ChkUnitOnBridge(ref string[] levers, LV_ST[] pathSol, LV_ST[] lootSol, ref BlueprintsCache __instance)
        {
            const bool HAS_CHECKER = true;
            var pathWithChk = SolWithChecker(
                SetLevers(ref levers, pathSol, ref __instance, HAS_CHECKER),
                BPHelper.GetChecker(levers[Idx_LeverToDown(1)], 0, ref __instance),
                ref __instance);

            var lootWithChk = SolWithChecker(
                SetLevers(ref levers, lootSol, ref __instance, HAS_CHECKER),
                BPHelper.GetChecker(levers[Idx_LeverToDown(2)], 0, ref __instance),
                ref __instance);

            int leverNum = levers.Length / 2;
            for (int i = 1; i <= leverNum; ++i)
            {
                BPHelper.SetActions(levers[Idx_LeverToDown(i)], pathWithChk, ref __instance);
                BPHelper.SetActions(levers[Idx_LeverToUp(i)], lootWithChk, ref __instance);
            }
        }

        static private ActionList SolWithChecker(ActionList sol, Conditional checker, ref BlueprintsCache __instance)
        {
            var solWithChecker = new ActionList() { Actions = new GameAction[1] };
            solWithChecker.Actions[0] = new Conditional()       //Need a deep copy
            {
                ConditionsChecker = checker.ConditionsChecker,
                IfFalse = checker.IfFalse,
                IfTrue = checker.IfTrue,
                Comment = "Any unit on bridge"
            };
            ((Conditional)solWithChecker.Actions[0]).IfFalse = sol;
            return solWithChecker;
        }

        static private ActionList SetLevers(ref string[] levers, LV_ST[] states, ref BlueprintsCache __instance, bool hasChecker = false)
        {
            GameAction updateEtudes = BPHelper.GetAction(levers[Idx_LeverToUp(1)], 1, ref __instance);
            var ret = new ActionList() { Actions = new GameAction[states.Length + 1] };
            for (int i = 0; i < states.Length; ++i)
            {
                int leverIdx = i + 1;
                if (LV_ST.DOWN == states[i])
                {
                    ret.Actions[i] = hasChecker
                        ? SetLeverDown_Nested(ref levers, leverIdx, ref __instance)
                        : SetLeverDown(ref levers, leverIdx, ref __instance);
                }
                else
                {
                    ret.Actions[i] = SetLeverUp(ref levers, leverIdx, ref __instance);
                }
            }
            ret.Actions[states.Length] = updateEtudes;
            return ret;
        }

        static private GameAction SetLeverDown(ref string[] guids, int leverIdx, ref BlueprintsCache __instance)
        {
            var chk = BPHelper.GetChecker(guids[Idx_LeverToDown(leverIdx)], 0, ref __instance);
            return chk.IfTrue.Actions[0];
        }

        static private GameAction SetLeverDown_Nested(ref string[] guids, int leverIdx, ref BlueprintsCache __instance)
        {
            var chk = BPHelper.GetChecker(guids[Idx_LeverToDown(leverIdx)], 0, ref __instance);
            var con = (Conditional)chk.IfFalse.Actions[0];
            return con.IfTrue.Actions[0];
        }

        static private GameAction SetLeverUp(ref string[] guids, int leverIdx, ref BlueprintsCache __instance)
        {
            return BPHelper.GetAction(guids[Idx_LeverToUp(leverIdx)], 0, ref __instance);
        }
    }
}

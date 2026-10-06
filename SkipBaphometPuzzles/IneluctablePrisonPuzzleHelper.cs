using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipBaphometPuzzles
{
    internal class IneluctablePrisonPuzzleHelper
    {
        //原逻辑和现逻辑: 同IvorySanctumPuzzleHelper.cs
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            SkipLinnormRoomPuzzle(ref __instance);
            SkipMinotaurRoomPuzzle(ref __instance); //Room with Bafomet's super Minotaur
            SkipGuardRoomPuzzle(ref __instance);    //Room with a hole to outside on wall
        }

        static private void SkipLinnormRoomPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string[] BTN = {
                "2fb7b63f7fba3a6439fca1c342f45151", //CipherLinnormRoom_0_Actions
                "29049c3e0153ed24ea4e40aef8f32b79", //CipherLinnormRoom_3_Actions
                "49015213365542d478a5a6c1990fa809", //CipherLinnormRoom_4_Actions
                "74bfed7534bba6a468037c7933c2635f", //CipherLinnormRoom_5_Actions
                "a32cc047441c32c44818d2b49296857d", //CipherLinnormRoom_6_Actions
            };

            SkipRoomPuzzle(BTN, Hexagram, ref __instance);  //Right code: 0356
        }

        static private void SkipMinotaurRoomPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string[] BTN = {
                "c32191b61d3a3f447a6a4a7f773e70c0", //CipherMinotaurRoom_0_Actions
                "ce051d2d291da7f4d8346c1f45c17034", //CipherMinotaurRoom_3_Actions
                "be283ccd851c51a4abadc2b36e159d87", //CipherMinotaurRoom_4_Actions
                "bd26cb29d0742a740902fe4ca7aa4475", //CipherMinotaurRoom_5_Actions
                "d48a11110b42f7b44b1fa2c5bf709001", //CipherMinotaurRoom_6_Actions
            };

            SkipRoomPuzzle(BTN, Pentagram, ref __instance); //Right code: 4365
        }

        static private void SkipGuardRoomPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string[] BTN = {
                "90a1ce317e2786c4f84f46006a0c1dc2", //CipherGuardRoom_0_Actions
                "cf42989a20ba8a54a86bbd481678e1d7", //CipherGuardRoom_3_Actions
                "5890bae4e0b866445bda6a58f5cf4cb9", //CipherGuardRoom_4_Actions
                "cf622a2c95fb40a479cd6b2258ec6cb2", //CipherGuardRoom_5_Actions
                "201347ccb8d87c24c9ef5aeb62722f32", //CipherGuardRoom_6_Actions
            };

            SkipRoomPuzzle(BTN, Square, ref __instance);    //Right code: 6054
        }

        static private void SkipRoomPuzzle(string[] btns, int lastBtn, ref BlueprintsCache __instance)
        {
            //Get actionList on right code inputed
            var actionsOnRightCode = BPHelper.GetChecker(btns[lastBtn], 2, ref __instance).IfTrue;

            //Run actions on any btn pressed
            foreach (string btn in btns)
            {
                BPHelper.SetActions(btn, actionsOnRightCode, ref __instance);
            }
        }

        //Index for btn array
        private const int Circle = 0;
        private const int Triangle = 1;
        private const int Square = 2;
        private const int Pentagram = 3;
        private const int Hexagram = 4;

    }
}

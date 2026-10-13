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
    internal class RichHousePuzzleHelper
    {
        //原逻辑: DLC2_RichQuarter启动了3个etude(SteamWall, PinkWall, OrangeWall)检测炉子的状态,
        //        当Purple, Pink, Orange这3个flag为1时, 对应的蒸汽墙消失.
        //现逻辑: 拉动任意拉杆, 将Purple, Pink, Orange这3个flag为1, 使蒸汽墙全部消失
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Set flags of steam walls
            var deactiveSteamWalls = new ActionList();
            deactiveSteamWalls.Actions = new GameAction[3] {
                DeactiveSteamWall(STEAM_WALLS[0]),
                DeactiveSteamWall(STEAM_WALLS[1]),
                DeactiveSteamWall(STEAM_WALLS[2]),
            };

            //deactive 3 steam walls on any lever pulled
            foreach (string lever in LEVERS)
                BPHelper.SetActions(lever, deactiveSteamWalls, ref __instance);
        }

        static private string[] STEAM_WALLS = {
            "447a7f2895d54ed2acce44f08ef0c62e", //Purple
            "8bc7e162d90d4abd959ae3d37825b10b", //Pink
            "461cc6e7d8784d0b99c5e8cdd436292e", //Orange
        };

        static private string[] LEVERS = {
            "6d744209a0c944c58215c99efb1915ae", //FromGreenToPink
            "06ac6574603041fba0d6d68a382a3aa6", //FromGreenToPurple
            "d8f6f1beac994eacbd1ef9d5e7d26b17", //FromGreenToRed
            "580a7ba4694248da874427b823cb2bab", //FromOrangeToPink
            "bab6cff6028d44f592016c3b6c96c356", //FromPinkToGreen
            "1cc9410174b14b4180ab834d3b5c7aaa", //FromPinkToOrange
            "396b16cdc6594644ace7b3516d39e3e7", //FromPurpleToGreen
            "ea3e3433f9704653b4ba67592a28d7ee", //FromPurpleToRed
            "e28ab48ad483403d9256e5b2ea42a911", //FromRedToGreen
            "a34653d0d6c6423b9e0717e049fbdd60", //FromRedToPurple
        };

        static private UnlockFlag DeactiveSteamWall(string guid)
        {
            const int OFF = 1;
            var steamWallFlag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(guid);
            return new UnlockFlag() { flag = steamWallFlag, flagValue = OFF };
        }
    }
}

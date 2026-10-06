using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.AreaLogic.Cutscenes.Commands;
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
    internal class IvorySanctumPuzzleHelper
    {
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            SkipMainHallPuzzle(ref __instance);     //The hall at the center of map, with a large statue inside;
            SkipYerribethHallPuzzle(ref __instance);//
            SkipValutPuzzle(ref __instance);
        }

        //原逻辑: 每个按钮MainHallCipher_[0,3,4,5,6]_Actions按下时, 将对应的MainHallPuzzlePlate_[0,3,4,5,6]置为1,
        //        错误时把MainHallPuzzlePlate_WrongOrder设为1.
        //        按下正确密码(6354)末位对应的按钮(此处为四边形按钮)时, 检查顺序是否正确, 正确则显示宝箱.
        //现逻辑: 按下任意按钮显示宝箱.
        static private void SkipMainHallPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string[] BTN = {
                "c1b615b97f6cf6e4ebe59f267bf339df",   //MainHallCipher_0_Actions
                "009ce72443f1949439b450f80afab492",   //MainHallCipher_3_Actions
                "24ea57e50a7b1134e9c70763923e7803",   //MainHallCipher_4_Actions
                "1f4caf246929b1a4b89da72fb62b07bd",   //MainHallCipher_5_Actions
                "b870ef4987395914a91e26fcfb06d128",   //MainHallCipher_6_Actions
            };

            ActionList revealChest = BPHelper.GetChecker(BTN[Square], 2, ref __instance).IfTrue;

            foreach (string btn in BTN)
                BPHelper.SetActions(btn, revealChest, ref __instance);
        }

        //原逻辑: 同MainHallPuzzle, 只不过维护显示宝箱(4306)和开门(3540)两套变量.
        //现逻辑: 按下任意按钮, 直接开门并显示宝箱.
        //备注1:  开门动作藏得太深, 按流程逐层深入获取的话会需要大量代码, 可读性反而变差, 所以这里直接通过guid获取
        //        开门动作的位置:
        //        YeribethHallCipher_6_ActionsCombined
        //          ->finishCipher1
        //              ->YeribethCipherDoorSuccess
        //                  ->CommandControlCamera
        //                      ->EndGate
        //                          ->StartedTracks
        //                              ->Track1
        //                                  ->Actions[0]
        //备注2: 开门和显示宝箱都执行了CommandControlCamera, 会引起流程延迟. 这里把开门的CommandControlCamera设为立即完成
        static private void SkipYerribethHallPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string[] BTN = {
                "d0d4ac1f69341794ea7d8241637fa421",   //YeribethHallCipher_0_ActionsCombined
                "131406d6450ca3f4f980a35babec28cf",   //YeribethHallCipher_3_ActionsCombined
                "b4565f83c4aa9c34d9f5196f02787de3",   //YeribethHallCipher_4_ActionsCombined
                "67734230cf7c530479e3e57d96b0b9ed",   //YeribethHallCipher_5_ActionsCombined
                "3a7cc66ce7e65884eada98b823e45008",   //YeribethHallCipher_6_ActionsCombined
            };
            const string SCENE_OPEN_DOOR = "a6076df5e5d7fda4e8986d1ad35df773";  //YeribethCipherDoorSuccess

            //Do both thing in one ActionList
            ActionList openDoor = BPHelper.GetChecker(BTN[Circle], 2, ref __instance).IfTrue;
            ActionList revealChest = BPHelper.GetChecker(BTN[Hexagram], 2, ref __instance).IfTrue;
            var doorAndChest = new ActionList(openDoor, revealChest);

            //Instandly finish camera rotation
            var scene = (Cutscene)__instance.Load(BlueprintGuid.Parse(SCENE_OPEN_DOOR));
            var rotateCamera = (CommandControlCamera)scene.StartedTracks[0].Commands[0];
            rotateCamera.TimingMode = CommandControlCamera.TimingModeType.Snap;

            foreach (string btn in BTN)
                BPHelper.SetActions(btn, doorAndChest, ref __instance);
        }

        //原逻辑: 同MainHallPuzzle, 只不过最后执行的不是开门而是检查压力板上是否站人(0563), 是的话才开门.
        //现逻辑: 按下任意按钮, 直接开门.

        static private void SkipValutPuzzle(ref BlueprintsCache __instance)
        {
            //Guids
            string[] BTN = {
                "03c39d830a0bee740a115ec007aaea10",   //VaultCipher_0_Actions
                "d85786ee9db94fe46a49cbdb5383d77b",   //VaultCipher_3_Actions
                "8d249ebe34842904dbd2c3990983a012",   //VaultCipher_4_Actions
                "82895687297bf864cafb1241615f5ad3",   //VaultCipher_5_Actions
                "dcc63693a7a9add4594fa566220b0f5e",   //VaultCipher_6_Actions
            };

            //Find actions to open door
            ActionList checkPressurePlates = BPHelper.GetChecker(BTN[Triangle], 2, ref __instance).IfTrue;
            RunActionHolder checkerHolder = (RunActionHolder)checkPressurePlates.Actions[0];
            Conditional pressurePlateChecker = BPHelper.GetChecker(checkerHolder.Holder.Guid.ToString(), 0, ref __instance);
            ActionList openDoor = pressurePlateChecker.IfTrue;

            foreach (string btn in BTN)
                BPHelper.SetActions(btn, openDoor, ref __instance);
        }

        //Index for btn array
        private const int Circle = 0;
        private const int Triangle = 1;
        private const int Square = 2;
        private const int Pentagram = 3;
        private const int Hexagram = 4;
    }
}

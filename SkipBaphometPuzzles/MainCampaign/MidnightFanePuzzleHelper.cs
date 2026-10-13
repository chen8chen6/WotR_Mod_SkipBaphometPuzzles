using Epic.OnlineServices.AntiCheatClient;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
/*
 * ====================================  Midnight Fane Map  =========================================
 *                                     D == door; S == Switch;
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ --------------- \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|               |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|               |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|               |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\ ---- \\\\\\\\\\\\\\ -----------    |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\ ------ \\\\\\\\\\\\|    |\\\\\\\\\\\\\\\\\\\\\\\\\\|   |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\|      |\\\\\\\\\\\\|    |\\\\\\\\\\\\\\ -----------    |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\|       ------- S5 -     |\\\\\\\\\\\\\\|               |\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 * \\\\\|                   D5   |\\\\\\\\\\\\\\|    ----------- \\\\\\\\\\\\\\ ---------------- \\\\
 * \\\\\|       ------------     |\\\\\\\\\\\\\\|   |\\\\\\\\\\\\\\\\\\\\\\\\\\|                |\\\\
 * \\\\\|      |\\\\\\\\\\\\|    |\\\\\\\\\\\\\\|    ----------- \\\\\\\\\\\\\\|                |\\\\
 * \\\\\ -    - \\\\\\\\\\\\|    |\\\\\\\\\\\\\\|               |\\\\\\\\\\\\\\|                |\\\\
 * \\\\\\\|  |\\\\\\\\\\\\\\|     -----------S1-                 -S3-----------                 |\\\\
 * \\\\\\\|  |\\\\\\\\\\\\\\|                   D1              D3                              |\\\\
 * \\\\\\\|  |\\\\\\\\\\\\\\|     --------------                 -------------------------      |\\\\
 * \\\\\\\|  |\\\\\\\\\\\\\\|    |\\\\\\\\\\\\\\|               |\\\\\\\\\\\\\\\\\\\\\\\\\|     |\\\\
 * \\\\\ -    - \\\\\\\\\\\\|    |\\\\\\\\\\\\\\|      Main     |\\\\\\\\\\\\\\\\\\\\\\\\\|     |\\\\
 * \\\\\|      |\\\\\\\\\\\\|    |\\\\\\\\\\\\\\|      Hall     |\\\\\\\\\\\\\\\\\\\\\\\\\|     |\\\\
 * \\\\\|       ------------  D6  -S6--------S2-                 -S4--------- \\\\\\\\\\\\|     |\\\\
 * \\\\\|                                       D2              D4           |\\\\\\\\\\\\|     |\\\\
 * \\\\\|       -----------------------S7- D7 --                 -------     |\\\\\\\\\\\\|     |\\\\
 * \\\\\|      |\\\\\\\\\\\\\\\\\\\\\\\\\\|  |\\|               |\\\\\\\|    |\\\\\\\\\\\\|     |\\\\
 * \\\\\ ------ \\\\\\\\\\ ---------------   S7\|               |\\\\\\\|    |\\\\\\\\\\\\|     |\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\|                  |\\|               |\\\\\\\| D8  ------------      |\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\ ------------------ \\|               |\\\\\\\|                       |\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ -----     ----- \\\\\\\S8                      |\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|   |\\\\\\\\\\\\\|                       |\\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|   |\\\\\\\\\\\\\ ----------------------- \\\\
 * \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
 */

namespace SkipBaphometPuzzles
{
    internal class MidnightFanePuzzleHelper
    {
        //原逻辑: 8个门的状态记录在MidnightFane_DoorSwitch_[1,...,8]中,
        //        通过各自的开关(door_[1,...,8]_switch_[on,off])切换. 开关和门在地图上的位置见上图.
        //        推测有个checker以不明方式监控着8个门的状态, 当门3,8开启, 其它关闭时则打开主厅密门.
        //现逻辑: 与任意开关交互均可将8个标志位置为可打开密门的状态, 然后被不知何处的checker检测到进而打开主厅密门.
        static public void SkipPuzzle(ref BlueprintsCache __instance)
        {
            //Actions to set flags to open door
            ActionList setFlagsCorrect = new ActionList { Actions = new GameAction[8] };
            setFlagsCorrect.Actions[0] = ActionSetFlag(DOOR_SWITCH_OFF[1], 1, ref __instance);
            setFlagsCorrect.Actions[1] = ActionSetFlag(DOOR_SWITCH_OFF[2], 1, ref __instance);
            setFlagsCorrect.Actions[2] = ActionSetFlag(DOOR_SWITCH_ON[3], 1, ref __instance);
            setFlagsCorrect.Actions[3] = ActionSetFlag(DOOR_SWITCH_OFF[4], 1, ref __instance);
            setFlagsCorrect.Actions[4] = ActionSetFlag(DOOR_SWITCH_OFF[5], 1, ref __instance);
            setFlagsCorrect.Actions[5] = ActionSetFlag(DOOR_SWITCH_OFF[6], 1, ref __instance);
            setFlagsCorrect.Actions[6] = ActionSetFlag(DOOR_SWITCH_OFF[7], 3, ref __instance);
            setFlagsCorrect.Actions[7] = ActionSetFlag(DOOR_SWITCH_ON[8], 0, ref __instance);

            //Set flags after switch finishs its job
            for (int i = 1; i <= 8; ++i)
            {
                AppendActions(DOOR_SWITCH_OFF[i], setFlagsCorrect, ref __instance);
                AppendActions(DOOR_SWITCH_ON[i], setFlagsCorrect, ref __instance);
            }
        }

        static private string PLACE_HOLDER = "";

        static private string[] DOOR_SWITCH_OFF = {
            PLACE_HOLDER,
            "3bec462594c5a0d45bb1a07be2fd9bac", //door_1_switch_off
            "e7e571ad6c32aa6488a9ae801c915c34", //door_2_switch_off
            "c433357e658abf542aa959cf247937b6", //door_3_switch_off
            "a629898a0e5eb2f46bff02a9254cf4ae", //door_4_switch_off
            "51e76cca7d12acf4197d3d5dae271160", //door_5_switch_off
            "70c73428bbb6a474c8d45961e24c420d", //door_6_switch_off
            "d5dddfd60cb6f204e90a2f88ce586dda", //door_7_switch_off
            "dbb9518dce1f3fb409e06c41fd514989", //door_8_switch_off
        };

        static private string[] DOOR_SWITCH_ON = {
            PLACE_HOLDER,
            "a2bc6cd7275a23744a052528551eff8c", //door_1_switch_on
            "9704f688ea4df1e41985a13e70c950b7", //door_2_switch_on
            "081687daa10ba8d428597c957077ac9a", //door_3_switch_on
            "e2c80f6224269fe42ba38aa63f708ba8", //door_4_switch_on
            "bc22b2e14f41bc14cb45b06264826a0e", //door_5_switch_on
            "dc472f2ff6474a447ab6f9a6ed7bf2ea", //door_6_switch_on
            "9cd006d825e526448b9fec73afd72d59", //door_7_switch_on
            "843ec93cc6d6a024aa1be8bb61c23da9", //door_8_switch_on
        };

        static private GameAction ActionSetFlag(string guid, int idx, ref BlueprintsCache __instance)
        {
            ActionsHolder ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            return ah.Actions.Actions[idx];
        }

        static private void AppendActions(string guid, ActionList actions, ref BlueprintsCache __instance)
        {
            ActionsHolder ah = (ActionsHolder)__instance.Load(BlueprintGuid.Parse(guid));
            ah.Actions = new ActionList(ah.Actions, actions);
        }
    }
}

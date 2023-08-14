/**
 * 游戏状态 
 */

using GameBaseFramework.Base;
using GameBaseFramework.Patterns;

namespace GameLogics
{
    internal class GameStartState : BaseState, ICommand
    {
        /// <summary>
        /// 进入开始游戏状态
        /// </summary>
        public override void Enter()
        {
            Debuger.Log("game start state ==> enter");
            this.SendCommand(new UIViewOpenCommand() { Name = "UIViewLogin" });
        }
    }
}

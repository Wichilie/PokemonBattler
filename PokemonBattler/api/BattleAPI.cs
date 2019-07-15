using System.Collections.Generic;

namespace PokemonBattler
{
    namespace BattleAPI
    {
        struct BattleConfig
        {
            string format;
        }
        struct BattleState
        {
            // todo
        }
        struct TurnConfig
        {
            // todo
        }

        interface IBattleAPI
        {
            // launch the simulator
            void InitSimulator();

            // start a battle with the given configuration
            void StartBattle();

            // get the current battle state
            BattleState GetState();
            // do a turn with the given configuration
            void DoTurn(TurnConfig tc);
        }
    }
}

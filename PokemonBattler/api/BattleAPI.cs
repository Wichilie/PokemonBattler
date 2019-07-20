namespace PokemonBattler
{
    namespace BattleAPI
    {
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

            // start a battle
            void StartBattle();

            // get the current battle state
            BattleState GetState();
            // do a turn with the given configuration
            void DoTurn(TurnConfig tc);
        }
    }
}

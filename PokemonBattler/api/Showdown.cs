namespace PokemonBattler
{
    namespace BattleAPI
    {
        class Showdown : IBattleAPI
        {
            public void InitSimulator()
            {
                // a new instance is generated every battle - no initialisation needs to be done
            }

            public void StartBattle(BattleConfig bc)
            {
                // todo
            }

            // get the current battle state
            public BattleState GetState() 
            {
                // todo
                return new BattleState();
            }
            // do a turn with the given configuration
            public void DoTurn(TurnConfig tc)
            {
                // todo
            }
        }
    }
}

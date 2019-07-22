using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace PokemonBattler
{
    namespace BattleAPI
    {
        class PlayerState
        {
            // a JSON array containing the player's pokemon
            public JArray state_self { get; set; }
            public JArray state_opponent { get; set; }

            public PlayerState(string p)
            {
                state_self = new JArray();
                state_opponent = new JArray();
            }
        }
        class BattleState
        {
            // a collection of all player (states)
            public Dictionary<string, PlayerState> players;
            // a JSON object containing the battlefield state (weather, terrain, etc)
            public JObject battlefield;

            public BattleState()
            {
                players = new Dictionary<string, PlayerState>
                {
                    { "p1", new PlayerState("p1") },
                    { "p2", new PlayerState("p2") }
                };
                battlefield = new JObject(); // todo
            }
        }

        class TurnConfig
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

using System.Text;
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

            public override string ToString()
            {
                return $"State:\n{state_self.ToString()}\n\nOpponent state:\n{state_opponent.ToString()}";
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

            public override string ToString()
            {
                StringBuilder string_builder = new StringBuilder();

                // add all players
                foreach (KeyValuePair<string, PlayerState> kvp in players)
                {
                    string_builder.Append($"PLAYER {kvp.Key}:\n{kvp.Value.ToString()}\n\n");
                }
                // add the battlefield
                string_builder.Append($"BATTLEFIELD:\n{battlefield.ToString()}");

                return string_builder.ToString();
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

using System;
using System.Diagnostics;

using Newtonsoft.Json.Linq;

namespace PokemonBattler
{
    namespace BattleAPI
    {
        namespace Showdown
        {
            class Showdown : IBattleAPI
            {
                Process showdown;
                BattleState state;

                public void InitSimulator()
                {
                    // a new instance is generated every battle - no initialisation needs to be done
                }

                public void StartBattle()
                {
                    // configure the showdown process and start it
                    showdown = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "node",
                            Arguments = "Middleman.js",
                            WorkingDirectory = "./PokemonBattler/api/showdown/",
                            CreateNoWindow = false,
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true
                        }
                    };
                    if (!showdown.Start())
                    {
                        throw new Exception("Could not start Showdown instance.");
                    }

                    // create a new battle state
                    state = new BattleState();

                    // initialise the battle
                    GiveCommand(@">start {""formatid"":""gen7randombattle"", ""p1"":{""name"":""bot1""}, ""p2"":{""name"":""bot2""}}");
                }

                public BattleState GetState()
                {
                    return state;
                }
                public void DoTurn(TurnConfig tc)
                {
                    // todo
                }

                // pass a command to showdown
                private void GiveCommand(string command)
                {
                    showdown.StandardInput.WriteLine(command);
                    // read the response
                    ProcessOutput(); ProcessOutput(); ProcessOutput(); // todo
                }
                // process a response from showdown
                private void ProcessOutput()
                {
                    // read the message type and choose the correct line processor
                    string msg_type = showdown.StandardOutput.ReadLine();
                    Func<string[], bool> processor;
                    switch (msg_type)
                    {
                        case "update":
                            processor = process_line_update;
                            break;
                        case "sideupdate":
                            processor = process_line_sideupdate;
                            break;
#if DEBUG
                        default:
                            Console.WriteLine($@"Warning: ignored unrecognised message of type ""{msg_type}""");
                            return;
#endif
                    }

                    // feed each individual line to the line processor, while the delimiter (a double line ending) has not been reached
                    string msg_line;
                    string[] msg_line_parsed;
                    while ((msg_line = showdown.StandardOutput.ReadLine()) != "")
                    {
                        msg_line_parsed = msg_line.Split('|', StringSplitOptions.RemoveEmptyEntries);
                        // ignore empty lines
                        if (msg_line_parsed.Length == 0)
                        {
                            continue;
                        }

#if DEBUG
                        if (!processor(msg_line_parsed))
                        {
                            Console.WriteLine($@"Warning: ignored unrecognised line in message of type ""{msg_type}"": {msg_line}");
                        }
#else
                        processor(msg_line_parsed)
#endif
                    }

                    // process a single message line
                    #region LINE_PROCESSORS
                    bool process_line_sideupdate(string[] line_parsed)
                    {
                        switch (line_parsed[0])
                        {
                            // identifiers
                            case "p1":
                            case "p2":
                                // irrelevant, also present in request line
                                return true;

                            // choice request
                            case "request":
                                // update the player's state to match the data provided
                                JObject request = JObject.Parse(line_parsed[1]);
                                JObject request_side = (JObject)request["side"];

                                state.players[(string)request_side["id"]].state_self = (JArray)request_side["pokemon"];

                                return true;

                            default:
                                return false;
                        }
                    }
                    bool process_line_update(string[] line_parsed)
                    {
                        switch (line_parsed[0])
                        {
                            // metadata
                            case "player":
                            case "teamsize":
                            case "gametype":
                            case "gen":
                            case "tier":
                            case "rated":
                            case "rule":
                            case "start":
                            case "inactive":
                            case "inactiveoff":
                                // irrelevant, battle settings are already known because we initialised the battle
                                return true;

                            // turn data
                            case "split":
                                // get the secret player ID
                                string player = line_parsed[1];

                                // ignore the secret (can be inferred from REQUEST message)
                                showdown.StandardOutput.ReadLine();

                                // the public message will be processed like any other
                                return true;

                            case "upkeep":
                                return true; // todo
                            case "turn":
                                return true; // todo

                            // turn actions
                            case "switch":
                            string[] delimited_player_id_and_pokemon_name = line_parsed[1].Split(' ');
                            string player_id = delimited_player_id_and_pokemon_name[0].Substring(0,2);
                            string pokemon_name = delimited_player_id_and_pokemon_name[1];
                            

                            string[] delimited_percentage_hp = line_parsed[3].Split('/');
                            int pokemon_percentage_hp = Int32.Parse(delimited_percentage_hp[0]);
#if DEBUG                            
                            Console.WriteLine($@"Player ID: {player_id}");
                            Console.WriteLine($@"Pokemon name: {pokemon_name}");
                            Console.WriteLine($@"Pokemon percentage HP: {pokemon_percentage_hp}");
#endif                           
                                return true; // todo save
                            case "move":
                                return true; // todo

                            // turn results
                            // todo

                            default:
                                return false;
                        }
                    }
                    #endregion
                }
            }
        }
    }
}

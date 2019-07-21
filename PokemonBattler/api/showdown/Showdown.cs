using System;
using System.Diagnostics;

namespace PokemonBattler
{
    namespace BattleAPI
    {
        namespace Showdown
        {
            class Showdown : IBattleAPI
            {
                private Process showdown;
                private BattleState state;

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
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true
                        }
                    };

                    showdown.ErrorDataReceived += ReadError;
                    showdown.OutputDataReceived += ReadOutput;
                    if (!showdown.Start())
                    {
                        throw new Exception("Could not start Showdown instance.");
                    }
                    showdown.BeginErrorReadLine();
                    showdown.BeginOutputReadLine();

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
                private void GiveCommand(string cmd)
                {
                    showdown.StandardInput.WriteLine(cmd);
                }
                // read an error from showdown
                private void ReadError(object o, DataReceivedEventArgs d)
                {
                    throw new Exception($"Showdown threw the following exception: {d.Data}");
                }
                // read a response from showdown
                private void ReadOutput(object o, DataReceivedEventArgs d)
                {
                    string[] parsed_output = d.Data.Split('|', StringSplitOptions.RemoveEmptyEntries);
                    // ignore empty messages
                    if (parsed_output.Length == 0)
                    {
                        return;
                    }

                    // handle the different message types
                    switch (parsed_output[0])
                    {
                        #region UPDATE
                        case "update":
                            // todo
                            break;

                        // metadata (irrelevant for us)
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
                            break;

                        // turn data
                        case "split":
                            // todo
                            break;
                        case "upkeep":
                            break;
                        case "turn":
                            // todo
                            break;

                        // turn actions
                        case "switch":
                            // todo
                            break;
                        case "move":
                            // todo
                            break;
                        
                        // turn results
                        // todo
                        #endregion

                        #region SIDEUPDATE
                        case "sideupdate":
                            // todo
                            break;

                        // identifiers
                        case "p1":
                        case "p2":
                            break;

                        // choice request
                        case "request":
                            // todo
                            break;
                        #endregion

                        #region UNIDENTIFIED
                        default:
                            Console.WriteLine($@"Warning: unhandled message of type ""{parsed_output[0]}"": {d.Data}");
                            break;
                            #endregion
                    }
                }
            }
        }
    }
}

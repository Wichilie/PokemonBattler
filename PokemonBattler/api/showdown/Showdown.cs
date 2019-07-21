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
                    if (!showdown.Start())
                    {
                        throw new Exception("Could not start Showdown instance.");
                    }

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
                    ProcessOutput();
                }
                // process a response from showdown
                private void ProcessOutput()
                {
                    // read the message type and choose the correct line processor
                    string msg_type = showdown.StandardOutput.ReadLine();
                    Action<string[]> processor;
                    switch (msg_type)
                    {
                        case "update":
                            processor = process_update_line;
                            break;
                        case "sideupdate":
                            processor = process_sideupdate_line;
                            break;
#if DEBUG
                        default:
                            Console.WriteLine($@"Warning: ignored unrecognised message of type ""{msg_type}""");
                            return;
#endif
                    }

                    // feed each individual message line to the line processor, while the delimiter (a double line ending) has not been reached
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

                        processor(msg_line_parsed);
                    }

                    #region LINE_PROCESSORS
                    void process_update_line(string[] line_parsed)
                    {
                        switch (line_parsed[0])
                        {
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

#if DEBUG
                            default:
                                Console.WriteLine($@"Warning: ignored unrecognised message line of type ""{line_parsed[0]}""");
                                return;
#endif
                        }
                    }
                    void process_sideupdate_line(string[] line_parsed)
                    {
                        switch (line_parsed[0])
                        {
                            // identifiers
                            case "p1":
                            case "p2":
                                break;

                            // choice request
                            case "request":
                                // todo
                                break;

#if DEBUG
                            default:
                                Console.WriteLine($@"Warning: ignored unrecognised message of type ""{msg_type}""");
                                return;
#endif
                        }
                    }
                    #endregion
                }
            }
        }
    }
}

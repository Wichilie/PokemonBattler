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

#if DEBUG
                        if (!processor(msg_line_parsed))
                        {
                            Console.WriteLine($@"Warning: ignored unrecognised line in message of type ""{msg_type}"": {msg_line}");
                        }
#else
                        processor(msg_line_parsed)
#endif
                    }

                    #region LINE_PROCESSORS
                    bool process_line_update(string[] line_parsed)
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
                                return true;

                            // turn data
                            case "split":
                                // todo
                                return true;
                            case "upkeep":
                                return true;
                            case "turn":
                                // todo
                                return true;

                            // turn actions
                            case "switch":
                                // todo
                                return true;
                            case "move":
                                // todo
                                return true;

                            // turn results
                            // todo

                            default:
                                return false;
                        }
                    }
                    bool process_line_sideupdate(string[] line_parsed)
                    {
                        switch (line_parsed[0])
                        {
                            // identifiers
                            case "p1":
                            case "p2":
                                return true;

                            // choice request
                            case "request":
                                // todo
                                return true;

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

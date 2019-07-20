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
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true
                        }
                    };

                    showdown.OutputDataReceived += ReadResponse;
                    if (!showdown.Start())
                    {
                        throw new Exception("Could not start Showdown instance.");
                    }
                    showdown.BeginOutputReadLine();

                    // initialise the battle
                    GiveCommand(">start {\"formatid\":\"gen7randombattle\", p1 {\"name\":\"Bot1\"}, p2 {\"name\":\"Bot2\"}}");
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
                // read a response from showdown
                private void ReadResponse(object o, DataReceivedEventArgs d)
                {
                    // todo
                }
            }
        }
    }
}

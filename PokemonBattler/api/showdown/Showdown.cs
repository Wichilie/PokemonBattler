using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace PokemonBattler
{
    namespace BattleAPI
    {
        class Showdown : IBattleAPI
        {
            private Process showdown;
            private List<string> response_buffer;

            public void InitSimulator()
            {
                // a new instance is generated every battle - no initialisation needs to be done
            }

            public void StartBattle()
            {
                Console.WriteLine("Starting new battle...");

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

                Console.WriteLine("Done");

                // initialise the battle
                GiveCommand(">start {\"formatid\":\"gen7randombattle\"}");
                GiveCommand(">player p1 {\"name\":\"Bot1\"}");
                GiveCommand(">player p2 {\"name\":\"Bot2\"}");
            }

            public BattleState GetState()
            {
                // todo
                return new BattleState();
            }
            public void DoTurn(TurnConfig tc)
            {
                // todo
            }

            // pass a command to showdown
            private void GiveCommand(string cmd)
            {
                response_buffer = new List<string>();
                showdown.StandardInput.WriteLine(cmd);
            }
            // read a response from showdown
            private void ReadResponse(object o, DataReceivedEventArgs d)
            {
                response_buffer.Add(d.Data);
            }
        }
    }
}

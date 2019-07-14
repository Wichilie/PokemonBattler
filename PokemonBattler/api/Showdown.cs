using System;
using System.Diagnostics;

namespace PokemonBattler
{
    namespace BattleAPI
    {
        class Showdown : IBattleAPI
        {
            Process showdown;

            public void InitSimulator()
            {
                // a new instance is generated every battle - no initialisation needs to be done
            }

            public void StartBattle()
            {
                // init the showdown process
                this.showdown = new Process();
                showdown.StartInfo.FileName = "cmd.exe";
                showdown.StartInfo.Arguments = "node Showdown.js";
                showdown.StartInfo.CreateNoWindow = true;
                showdown.StartInfo.UseShellExecute = false;
                showdown.StartInfo.RedirectStandardInput = true;
                showdown.StartInfo.RedirectStandardOutput = true;
                showdown.Start();
                showdown.BeginOutputReadLine();

                // init the battle
                showdown.StandardInput.WriteLine(">start {\"formatid\":\"gen7randombattle\"}");
                showdown.StandardInput.WriteLine(">player p1 {\"name\":\"Bot1\"}");
                showdown.StandardInput.WriteLine(">player p2 {\"name\":\"Bot2\"}");
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

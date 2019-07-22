using System;
using PokemonBattler.BattleAPI.Showdown;

class Program
{
    static void Main(string[] args)
    {
        // temporary debugging...
        Showdown sd = new Showdown();
        sd.StartBattle();

        Console.WriteLine(sd.GetState().ToString());
    }
}

using System.Threading;
using PokemonBattler.BattleAPI.Showdown;

class Program
{
    static void Main(string[] args)
    {
        // temporary debugging...
        Showdown sd = new Showdown();
        sd.StartBattle();
        Thread.Sleep(3000);
    }
}

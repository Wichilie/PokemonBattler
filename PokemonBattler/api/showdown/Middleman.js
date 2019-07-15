const ReadLine = require('readline');
const Sim      = require('../../simulation/showdown/.sim-dist/battle-stream');

battle = new Sim.BattleStream();

// redirect showdown output to API
(async () => { let rsp; 
			   while ((rsp = await battle.read())) 
			       console.log(rsp); })();

// redirect API input to showdown
const input = ReadLine.createInterface(process.stdin);
input.on('line', (cmd) => battle.write(cmd));

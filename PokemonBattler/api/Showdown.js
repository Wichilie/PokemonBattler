const Sim = require('../simulation/showdown/.sim-dist/battle-stream');
const ReadLine = require('readline');
stream = new Sim.BattleStream();

// redirect showdown output to console
(async () => {
	let output;
	while ((output = await stream.read())) {
		console.log(output);
	}
})();

// redirect input to showdown
const rl = ReadLine.createInterface({
    input: process.stdin,
    output: process.stdout
});
rl.on('line', (input) => {
    console.log(input);
});

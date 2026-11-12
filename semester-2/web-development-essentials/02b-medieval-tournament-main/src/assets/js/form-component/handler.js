import * as Helper from '../utils/helper.js';

const knight1select = document.querySelector('#knight1');
const knight2select = document.querySelector('#knight2');

export function startBattle(e) {
    e.preventDefault();

    const selectedKnight1 = Helper.getKnightData(knight1select.value);
    const selectedKnight2 = Helper.getKnightData(knight2select.value);

    if (selectedKnight1.id === selectedKnight2.id) {
        resultParagraph.textContent = "Knights cannot battle themselves!";
        return;
    }

    const knight1power = Helper.calculateKnightPower(selectedKnight1.id);
    const knight2power = Helper.calculateKnightPower(selectedKnight2.id);

    if (knight1power > knight2power) {
        Helper.showResult(`${selectedKnight1.name} wins with ${knight1power}:${knight2power}!`);
    } else if (knight2power > knight1power) {
        Helper.showResult(`${selectedKnight2.name} wins with ${knight2power}:${knight1power}`);
    } else {
        Helper.showResult("It's a draw!");
    }
}
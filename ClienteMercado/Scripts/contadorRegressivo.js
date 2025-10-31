//Contador Regressivo
var count = 6;

function countDown() {
    if ((count - 1) >= 0) {
        count = count - 1;
        $('#mensagem_segundos').html(count);
        setTimeout('countDown();', 1000);
    }
}
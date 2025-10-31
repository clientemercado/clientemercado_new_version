/* Formatação do CNPJ */
function FormataCNPJ(Campo, teclapres) {
    var tecla = teclapres.keyCode;
    var vr = new String(Campo.value);
    vr = vr.replace(".", "");
    vr = vr.replace(".", "");
    vr = vr.replace(".", "");
    vr = vr.replace("/", "");
    vr = vr.replace("-", "");

    tam = vr.length + 1;
    if (tecla != 9 && tecla != 8) {
        if (tam > 2 && tam < 6)
            Campo.value = vr.substr(0, 2) + '.' + vr.substr(2, tam);
        if (tam >= 6 && tam < 9)
            Campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, tam - 5);
        if (tam >= 9 && tam < 13)
            Campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, tam - 8);
        if (tam >= 13 && tam < 15)
            Campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12, tam - 12);
    }
}

/* Formatação do CPF */
function FormataCPF(Campo, teclapres) {
    var tecla = teclapres.keyCode;

    var vr = new String(Campo.value);
    vr = vr.replace(".", "");
    vr = vr.replace(".", "");
    vr = vr.replace("-", "");

    tam = vr.length + 1;

    if (tecla != 9 && tecla != 8) {
        if (tam > 3 && tam < 7)
            Campo.value = vr.substr(0, 3) + '.' + vr.substr(3, tam);
        if (tam >= 7 && tam < 10)
            Campo.value = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, tam - 6);
        if (tam >= 10 && tam < 12)
            Campo.value = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, 3) + '-' + vr.substr(9, tam - 9);
    }
}

/*Formatação CEP */
function MM_formtCep(e, src, mask) {
    if (window.event) { _TXT = e.keyCode; }
    else if (e.which) { _TXT = e.which; }
    if (_TXT > 47 && _TXT < 58) {
        var i = src.value.length;
        var saida = mask.substring(0, 1);
        var texto = mask.substring(i);

        if (texto.substring(0, 1) != saida) {
             src.value += texto.substring(0, 1);
        }

        return true;
    }
    else {
        if (_TXT != 8) {
            return false;
        } else {
             return true;
        }
    }
}

/* Limpar o Campos */
function limparCampoCNPJ() {
    document.getElementById('CNPJ_CPF_EMPRESA_USUARIO').value = '';
    document.getElementById('mensagemErroCnpj2').innerHTML = '';
    document.getElementById("btn_cadastro2").style.display = 'none';
    document.getElementById("btn_cadastro2_alter").style.display = 'none';
    document.getElementById('RAZAO_SOCIAL_EMPRESA').value = '';
    document.getElementById('RAZAO_SOCIAL_EMPRESA').style.backgroundColor = '#ffffff';
    document.getElementById("RAZAO_SOCIAL_EMPRESA").disabled = false;
    document.getElementById('NOME_FANTASIA_EMPRESA').value = '';
    document.getElementById('NOME_FANTASIA_EMPRESA').style.backgroundColor = '#ffffff';
    document.getElementById("NOME_FANTASIA_EMPRESA").disabled = false;
    document.getElementById('EMAIL1_EMPRESA').value = '';
    document.getElementById("EMAIL1_EMPRESA").disabled = false;
    document.getElementById('TELEFONE1_EMPRESA_USUARIO').value = '';
    document.getElementById("TELEFONE1_EMPRESA_USUARIO").disabled = false;
    document.getElementById('PAIS_EMPRESA_USUARIO').value = '';
    document.getElementById("PAIS_EMPRESA_USUARIO").disabled = false;
    document.getElementById('CEP_SEQUENCIAL_ENDERECO').value = '';
    document.getElementById("CEP_SEQUENCIAL_ENDERECO").disabled = false;
    document.getElementById('ENDERECO_EMPRESA_USUARIO').value = '';
    document.getElementById("ENDERECO_EMPRESA_USUARIO").disabled = false;
    document.getElementById('COMPLEMENTO_ENDERECO_EMPRESA_USUARIO').value = '';
    document.getElementById("COMPLEMENTO_ENDERECO_EMPRESA_USUARIO").disabled = false;
    document.getElementById('NOME_ESTADO_EMPRESA_USUARIO').value = '';
    document.getElementById("NOME_ESTADO_EMPRESA_USUARIO").disabled = false;
    document.getElementById('NOME_CIDADE_EMPRESA_USUARIO').value = '';
    document.getElementById("NOME_CIDADE_EMPRESA_USUARIO").disabled = false;
    document.getElementById('NOME_BAIRRO_EMPRESA_USUARIO').value = '';
    document.getElementById("NOME_BAIRRO_EMPRESA_USUARIO").disabled = false;
    document.getElementById('ID_EMPRESA').value = '';
    document.getElementById('ID_GRUPO_ATIVIDADES').value = 30;
    document.getElementById('CNPJ_CPF_EMPRESA_USUARIO').focus();
}

function limparCampoEmail(valor) {
    if (valor == 1) {
        document.getElementById('EMAIL1_EMPRESA').value = '';
        document.getElementById('EMAIL1_EMPRESA').focus();
    } else if (valor == 2){
        document.getElementById('EMAIL1_USUARIO').value = '';
        document.getElementById('EMAIL1_USUARIO').focus();
    }
}

function limparCampoCPF() {
    document.getElementById('CPF_USUARIO_EMPRESA').value = '';
    document.getElementById('mensagemErroCpf2').innerHTML = '';
    document.getElementById('NOME_USUARIO').value = '';
    document.getElementById('NOME_USUARIO').style.backgroundColor = '#ffffff';
    document.getElementById('NICK_NAME_USUARIO').value = '';
    document.getElementById('NICK_NAME_USUARIO').style.backgroundColor = '#ffffff';
    document.getElementById('CPF_USUARIO_EMPRESA').focus();
}

function limparCampoCEP() {
    document.getElementById('CEP_SEQUENCIAL_ENDERECO').value = '';
    document.getElementById('CEP_SEQUENCIAL_ENDERECO').focus();
}

function limparCampoQUANT(tipo) {
    if (tipo == 1) {
        document.getElementById('QUANTIDADE_ITEM_COTACAO_DIRECIONADA').value = '';
        document.getElementById('QUANTIDADE_ITEM_COTACAO_DIRECIONADA').focus();
    } else if (tipo == 2) {
        document.getElementById('QUANTIDADE_ITEM_COTACAO_AVULSA').value = '';
        document.getElementById('QUANTIDADE_ITEM_COTACAO_AVULSA').focus();
    }
}

//Setar Campos
function setarTipoLogin(tipoLogin, operacao) {
    if (operacao == 1) {
        document.getElementById('TIPO_LOGIN').value = tipoLogin;
        document.getElementById('LOGIN_EMPRESA_USUARIO').focus();
    } else if (operacao == 2){
        document.getElementById('EMAIL_ENVIO_SENHA').focus();
    }
}

function setarTipoPlano(tipoPlano, cor, div, valorMensal) {
    var corDesmarcada = '#CCCCCC';

    if (div == 'plano1') {
        document.getElementById(div).style.background = cor;
        document.getElementById('plano2').style.background = corDesmarcada;
        document.getElementById('plano3').style.background = corDesmarcada;
        document.getElementById('plano4').style.background = corDesmarcada;

        document.getElementById('descPlano1').style.display = 'block';
        document.getElementById('descPlano2').style.display = 'none';
        document.getElementById('descPlano3').style.display = 'none';
        document.getElementById('descPlano4').style.display = 'none';
        document.getElementById('area_botoes_assinatura2').style.display = 'none';
        document.getElementById('btn_gravar').style.display = 'block';
    } else if (div == 'plano2') {
        document.getElementById(div).style.background = cor;
        document.getElementById('plano1').style.background = corDesmarcada;
        document.getElementById('plano3').style.background = corDesmarcada;
        document.getElementById('plano4').style.background = corDesmarcada;
        document.getElementById('DESCRICAO_TIPO_CONTRATO_COTADA').value = 'Plano INDIVIDUAL USUÁRIO';

        document.getElementById('descPlano1').style.display = 'none';
        document.getElementById('descPlano2').style.display = 'block';
        document.getElementById('descPlano3').style.display = 'none';
        document.getElementById('descPlano4').style.display = 'none';
        document.getElementById('area_botoes_assinatura2').style.display = 'block';
        document.getElementById('btn_gravar').style.display = 'none';
    } else if (div == 'plano3') {
        document.getElementById(div).style.background = cor;
        document.getElementById('plano1').style.background = corDesmarcada;
        document.getElementById('plano2').style.background = corDesmarcada;
        document.getElementById('plano4').style.background = corDesmarcada;
        document.getElementById('DESCRICAO_TIPO_CONTRATO_COTADA').value = 'Plano IDEAL EMPRESA';

        document.getElementById('descPlano1').style.display = 'none';
        document.getElementById('descPlano2').style.display = 'none';
        document.getElementById('descPlano3').style.display = 'block';
        document.getElementById('descPlano4').style.display = 'none';
        document.getElementById('area_botoes_assinatura2').style.display = 'block';
        document.getElementById('btn_gravar').style.display = 'none';
    } else if (div == 'plano4') {
        document.getElementById(div).style.background = cor;
        document.getElementById('plano1').style.background = corDesmarcada;
        document.getElementById('plano2').style.background = corDesmarcada;
        document.getElementById('plano3').style.background = corDesmarcada;
        document.getElementById('DESCRICAO_TIPO_CONTRATO_COTADA').value = 'Plano MAIS EMPRESA';

        document.getElementById('descPlano1').style.display = 'none';
        document.getElementById('descPlano2').style.display = 'none';
        document.getElementById('descPlano3').style.display = 'none';
        document.getElementById('descPlano4').style.display = 'block';
        document.getElementById('area_botoes_assinatura2').style.display = 'block';
        document.getElementById('btn_gravar').style.display = 'none';
    }

    document.getElementById('ID_CODIGO_TIPO_CONTRATO_COTADA').value = tipoPlano;
    document.getElementById('VALOR_PLANO_CONTRATADO').value = valorMensal;
    //alert('ID_CODIGO_TIPO_CONTRATO_COTADA= ' + document.getElementById('ID_CODIGO_TIPO_CONTRATO_COTADA').value);
    //alert('VALOR_PLANO_CONTRATADO= ' + document.getElementById('VALOR_PLANO_CONTRATADO').value);
    //alert('DESCRICAO_TIPO_CONTRATO_COTADA= ' + document.getElementById('DESCRICAO_TIPO_CONTRATO_COTADA').value);
}

function setarResponsavelBoleto(responsavelBoleto) {
    document.getElementById('RESPONSAVEL_EMISSAO_BOLETO').value = responsavelBoleto;
    document.getElementById('LOGIN_EMPRESA_USUARIO').focus();
    //alert('RESPONSAVEL_EMISSAO_BOLETO= ' + document.getElementById('RESPONSAVEL_EMISSAO_BOLETO').value);
}

function setarTipoDesconto(tipoDesconto, operacao) {
    document.getElementById('APLICACAO_DO_DESCONTO').value = tipoDesconto;
    document.getElementById('VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_EXIBIR').focus();

    alert('APLICACAO_DO_DESCONTO= ' + document.getElementById('APLICACAO_DO_DESCONTO').value);
}

////Faz com que o campo só aceite a digitação de números
//function SomenteNumero(e) {
//    var tecla = (window.event) ? event.keyCode : e.which;
//    if ((tecla > 47 && tecla < 58))
//        return true;
//    else {
//        if (tecla == 8 || tecla == 0)
//            return true;
//        else
//            return false;
//    }
//}

//Formata número tipo moeda usando o evento onKeyDown
function Formata(campo, tammax, teclapres, decimal) {
    var tecla = teclapres.keyCode;
    vr = Limpar(campo.value, "0123456789");
    tam = vr.length;
    dec = decimal

    if (tam < tammax && tecla != 8) { tam = vr.length + 1; }

    if (tecla == 8)
    { tam = tam - 1; }

    if (tecla == 8 || tecla >= 48 && tecla <= 57 || tecla >= 96 && tecla <= 105) {

        if (tam <= dec)
        { campo.value = vr; }

        if ((tam > dec) && (tam <= 5)) {
            campo.value = vr.substr(0, tam - 2) + "," + vr.substr(tam - dec, tam);
        }
        if ((tam >= 6) && (tam <= 8)) {
            campo.value = vr.substr(0, tam - 5) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - dec, tam);
        }
        if ((tam >= 9) && (tam <= 11)) {
            campo.value = vr.substr(0, tam - 8) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - dec, tam);
        }
        if ((tam >= 12) && (tam <= 14)) {
            campo.value = vr.substr(0, tam - 11) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - dec, tam);
        }
        if ((tam >= 15) && (tam <= 17)) {
            campo.value = vr.substr(0, tam - 14) + "." + vr.substr(tam - 14, 3) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 2, tam);
        }
    }
}

//Faz parte da função Formata() acima
function Limpar(valor, validos) {
    // retira caracteres invalidos da string
    var result = "";
    var aux;
    for (var i = 0; i < valor.length; i++) {
        aux = validos.indexOf(valor.substring(i, i + 1));
        if (aux >= 0) {
            result += aux;
        }
    }
    return result;
}

//Checa quantidade de itens digitados para os campos do cartão de crédito
function checaQuantosDigitos(valor, campo, anoCorrente)
{
    /*
    OBS: Bloco abaixo comentado até que eu veja se é feita uma validação dos campos pelo GerenciaNet.
    */
    //if (valor != "") {
    //    //Verifica Número do cartão
    //    if (campo == 'DADOS_NUMERO_CARTAO') {
    //        if (parseInt(valor.length) < parseInt(16)) {
    //            alert('Atenção!\n\nO número do cartão deve ser composto de 16 dígitos.\nFavor digitar o número completo');
    //            document.getElementById('DADOS_NUMERO_CARTAO').focus();
    //        }
    //    }

    //    //Verifica mês do cartão
    //    if (campo == 'DADOS_MES_EXPIRACAO_CARTAO') {
    //        if ((parseInt(valor) <= parseInt(0)) || (parseInt(valor) > 12)) {
    //            alert('Atenção!\n\nMês inválido.\nFavor entrar com meses entre 1 e 12.');
    //            document.getElementById('DADOS_MES_EXPIRACAO_CARTAO').focus();
    //        }
    //    }

    //    //Verifica o ano de expiração
    //    if (campo == 'DADOS_ANO_EXPIRACAO_CARTAO') {
    //        if (parseInt(valor.length) < parseInt(4)) {
    //            alert('Atenção!\n\nO ano deve ser composto de 4 dígitos.\nFavor digitar o ano completo.');
    //            document.getElementById('DADOS_ANO_EXPIRACAO_CARTAO').focus();
    //        }

    //        if (parseInt(valor) < anoCorrente) {
    //            alert('Atenção!\n\nO ano de ' + valor + ' é inferior ao ano de ' + anoCorrente + ', sendo assim, seu cartão está vencido.\nFavor digitar um ano válido.');
    //            document.getElementById('DADOS_ANO_EXPIRACAO_CARTAO').focus();
    //        }
    //    }

    //    //Verifica o código de segurança
    //    if (campo == 'DADOS_CODIGO_SEGURANCA') {
    //        if (parseInt(valor.length) < parseInt(3)) {
    //            alert('Atenção!\nO código de segurança é composto de 3 dígitos.\nFavor digitar o código completo.');
    //            document.getElementById('DADOS_CODIGO_SEGURANCA').focus();
    //        }
    //    }
    //}
}

////Função que calcula o valor total do PRODUTO da COTAÇÃO, ao responder a mesma
//function CalculaValorTotalProdutosCotacao(campo, id) {
//    if ((campo.value != "") || (campo.value > 0)) {
//        var totalCalculo = 0;
//        var nomeCampoQuantidade = "inQuantidadeRealDoProduto_" + id;
//        var campoRecebeResultado = "inValorTotal_" + id;
//        var campoQuantidadeReal = new String(document.getElementById(nomeCampoQuantidade).value);

//        var multiplicando = new String(campo.value);

//        multiplicando = multiplicando.replace(".", "").replace(".", "").replace(",", ".");
//        multiplicador = campoQuantidadeReal.replace(".", "").replace(".", "").replace(",", ".");

//        totalCalculo = (parseFloat(multiplicador) * parseFloat(multiplicando));

//        document.getElementById(campoRecebeResultado).value = moeda(totalCalculo, 2, ",", ".");
//    }
//}

//Formata a saída dos valores monetários - PARTE 1
function moeda(valor, casas, separdor_decimal, separador_milhar) {
    var valor_total = parseInt(valor * (Math.pow(10, casas)));
    var inteiros = parseInt(parseInt(valor * (Math.pow(10, casas))) / parseFloat(Math.pow(10, casas)));
    var centavos = parseInt(parseInt(valor * (Math.pow(10, casas))) % parseFloat(Math.pow(10, casas)));


    if (centavos % 10 == 0 && centavos + "".length < 2) {
        centavos = centavos + "0";
    } else if (centavos < 10) {
        centavos = "0" + centavos;
    }

    var fmilhar = milhar(inteiros, separador_milhar);

    return fmilhar + "" + separdor_decimal + "" + centavos;
}

//Formata a saída dos valores monetários - PARTE 2
function milhar(inteiros, separador_milhar) {
    var milhares = parseInt(inteiros / 1000);

    if (milhares > 0) {
        var inteiros = inteiros % 1000;
        if (inteiros == 0) {
            inteiros = "000";
        } else if (inteiros < 10) {
            inteiros = "00" + inteiros;
        } else if (inteiros < 100) {
            inteiros = "0" + inteiros;
        }
        var retorno = milhar(milhares, separador_milhar) + "" + separador_milhar + "" + inteiros;
        return retorno;
    }
    else {
        return inteiros;
    }
}

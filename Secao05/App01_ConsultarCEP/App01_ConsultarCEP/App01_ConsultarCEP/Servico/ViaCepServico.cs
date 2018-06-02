using App01_ConsultarCEP.Servico.Modelo;
using Newtonsoft.Json;
using System;
using System.Net;
using Util;

namespace App01_ConsultarCEP.Servico
{
    public class ViaCepServico
    {
        private static string EnderecoUrl = "http://viacep.com.br/ws/{0}/json/";

        public static Processamento BuscarEnderecoViaCep(string cep, out Endereco endereco)
        {
            Processamento processamento = new Processamento();
            endereco = new Endereco();

            try
            {
                string url = string.Format(EnderecoUrl, cep);

                WebClient webClient = new WebClient();
                string resposta = webClient.DownloadString(url);
                endereco = JsonConvert.DeserializeObject<Endereco>(resposta);

                if (endereco.Cep == null)
                    endereco = null;
            }
            catch (Exception ex)
            {
                processamento = new Processamento(string.Format("0003 - Falha ao buscar o endereço. Detalhes: {0}", ex.Message), ex);
            }

            return processamento;
        }
    }
}

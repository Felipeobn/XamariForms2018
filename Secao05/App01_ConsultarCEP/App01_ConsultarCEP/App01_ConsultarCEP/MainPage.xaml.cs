using App01_ConsultarCEP.Servico;
using App01_ConsultarCEP.Servico.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Xamarin.Forms;

namespace App01_ConsultarCEP
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            cepButton.Clicked += CepButton_Clicked;
        }

        private void CepButton_Clicked(object sender, EventArgs e)
        {
            string cepFormatado = Util.Mascara.RemoverMascara(cepTextBox.Text);
            Processamento processamento = ValidarCep(cepFormatado);
            if (!processamento.Sucesso)
            {
                DisplayAlert("CEP Inválido", processamento.MensagemFormata, "OK");
                return;
            }

            Endereco endereco = new Endereco();
            processamento = ViaCepServico.BuscarEnderecoViaCep(cepFormatado, out endereco);
            if (!processamento.Sucesso)
            {
                DisplayAlert("CEP Inválido", processamento.MensagemFormata, "OK");
                return;
            }

            if (endereco == null)
            {
                DisplayAlert("CEP Não Encontrado", string.Format("O endereço não foi encontrado para o CEP informado: {0}", Util.Mascara.RenderizarMascara(cepFormatado, "##.###-###")), "OK");
                return;
            }

            cepLabel.Text = string.Format("Logradouro: {0}\nBairro: {1}\nCidade: {2}\nUF: {3}\nCEP: {4}\nIBGE: {5}", endereco.Logradouro, endereco.Bairro, endereco.Localidade, endereco.Uf, endereco.Cep, endereco.Ibge);
        }

        private Processamento ValidarCep(string cep)
        {
            int cepValidacao = 0;

            if (string.IsNullOrEmpty(cep))
                return new Processamento(2001, "CEP vázio");

            if (cep.Length != 8)
                return new Processamento(2002, "O CEP informado é invalido. O CEP deve possuir 8 caracteres");

            if (!int.TryParse(cep, out cepValidacao))
                return new Processamento(2003, "O CEP informado é invalido. Não pode possuir letras");

            return new Processamento();
        }
    }
}

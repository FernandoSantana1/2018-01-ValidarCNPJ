using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValidarCNPJ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool ValidarCNPJ(string cnpj)
        {
            // cnpj chega como String pois um CNPJ pode começar com zero 
            // Remover ". / - espaço" da string
            //se precisar remover outros caracteres incluir entre [ ] 
            cnpj = Regex.Replace(cnpj, "[ .,/-] +", "");
            //testar o comprimento da string eh 11(todo cnpj deve ter 14 posicoes)
            if (cnpj.Length != 14)  // .Length = tamanho
            {
                throw new ArgumentException("CNPJ deve ter 14 posicoes!");
            }
            //testar se todos os valores sao digitos 
            if (!Regex.IsMatch(cnpj, "[0-9]{14}")) //"isMatch = se corresponde a... //se dentro do CNPJ não houver numeros entre 0 a 9 com 14 digitos não será "Match"
            {
                throw new ArgumentException("CNPJ deve ter apenas numeros");
            }
            //testar os cnpj's validos 11111111111111......
            if (Regex.IsMatch(cnpj, @"([0-9])\1{13}"))
            {
                throw new ArgumentException("CNPJ invalido de acordo com a Receita Federal");
            }
            //Criar um array de numeros com base na string
            int[] iCNPJ = cnpj.Select(c => c - '0').ToArray();

            //////////Calcular o primeiro digito//////////////
            int soma = 0;
            for(int i = 0; i < iCNPJ.Length-2; i++)
            {
                if(i < 4)
                {
                    soma += iCNPJ[i] * (5 - i);
                }
                else
                {
                    soma += iCNPJ[i] * (13 - i);
                }
            }
            int digi1 = soma % 11;
            if (digi1 < 2)
            {
                digi1 = 0;
            }
            else
            {
                digi1 = 11 - (soma % 11);
            }
            /////////////Calcular o segundo digito//////////
            soma = 0;
            for (int i = 0; i < iCNPJ.Length-1; i++)
            {
                if(i < 5)
                {
                    soma += iCNPJ[i] * (6 - i);
                }
                else
                {
                    soma += iCNPJ[i] * (14 - i);
                }
            }
            int digi2 = soma % 11;
            if (digi2 < 2)
            {
                digi2 = 0;
            }
            else
            {
                digi2 = 11 - (soma % 11);
            }
            //testar os digitos verificadores calculados x informados
            if ((digi1 == iCNPJ[12]) && (digi2 == iCNPJ[13]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCNPJ(maskedTextBox1.Text))
                {
                    MessageBox.Show("CPF Valido.");
                }
                else
                {
                    MessageBox.Show("CPF Invalido.");
                }
            }
            catch (Exception ex) //catch = pegar
            {
                MessageBox.Show(ex.Message);
            }
            //ValidaCPF(maskedTextBox1.Text);
        }
    }
}

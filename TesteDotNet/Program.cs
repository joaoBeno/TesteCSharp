using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TesteDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seja bem vindo!!!");
            EfetuarOperação(true);
        }

        static int Soma(int a, int b)
        {
            try
            {
                return a + b;
            }
            catch (Exception e)
            {
                throw new ExcecaoDeSoma(e);
            }
        }

        static int Soma(params int[] numeros)
        {
            return numeros.Aggregate(0, Soma);
        }

        static int Subtracao(int a, int b)
        {
            try
            {
                return a - b;
            }
            catch (Exception e)
            {
                throw new ExcecaoDeSubtracao(e);
            }
        }

        static int Multiplicacao(int a, int b)
        {
            try
            {
                return a * b;
            }
            catch (Exception e)
            {
                throw new ExcecaoDeMultiplicacao(e);
            }
        }

        static int Divisao(int a, int b)
        {
            try
            {
                return a / b;
            }
            catch (Exception e)
            {
                throw new ExcecaoDeDivisao(e);
            }
        }

        static void Media(params int[] args)
        {
            Console.WriteLine("A média calculada foi: {0}", Divisao(Soma(args), args.Length));
        }
        
        static void MediaDosPares(params int[] args)
        {
            int[] numeros = args.Where(x => x % 2 == 0).ToArray();
            Console.WriteLine("A média calculada foi: {0}", Divisao(Soma(numeros), numeros.Length));
        }

        static void EfetuarOperação(bool ehPrimeiraExecucao = false)
        {
            if (ehPrimeiraExecucao)
                Console.WriteLine("As operações disponiveis são:" +
                                  "\n - Soma (soma ou +)" +
                                  "\n - Subtração (subtracao ou -)" +
                                  "\n - Multiplicação (multiplicacao ou *)" +
                                  "\n - Divisão (divisao ou /)" +
                                  "\n\nVocê pode digitar tanto \"2+2\", \"quanto 2;soma;2\", quanto \"2;+;2\"" +
                                  "\n\nMas se atente que você pode efetuar apenas uma operação de cada vez!" +
                                  "\n\nPara sair, digite \"sair\", e para exibir esta ajuda, digite \"ajuda\"");

            Console.WriteLine("Lendo arquivo de comandos...\n\n");

            var arq = new System.IO.StreamReader(@".\operacoes.txt");
            string linha;
            Dictionary<string, int> resultados = new Dictionary<string, int>();
            
            while((linha = arq.ReadLine()) != null)  
            {
                if (!linha.Equals(""))
                {
                    var chave = linha.Substring(0, linha.IndexOf(';'));
                    var cmdSplit = linha.Substring(linha.IndexOf(';') + 1).Split(';');
                    var comando = (cmdSplit.Length == 3) ? cmdSplit[1] + RemoverAcentos(cmdSplit[0].ToLower()) + cmdSplit[2] : "";

                    var resultado = (cmdSplit.Length > 3) ? Soma(cmdSplit.Skip(1).Select(x => Int32.Parse(x)).ToArray())
                                                               : OperacaoSemPontoVirgula(comando, true);
                    resultados.Add(chave, resultado ?? -1);
                }
            }

            foreach (KeyValuePair<string,int> chaveValor in resultados)
            {
                Console.WriteLine("Nome: {0}, Resultado: {1}", chaveValor.Key, chaveValor.Value);
            }
        }

        private static void ProcessaOperacao(string comando)
        {
            switch (comando.IndexOf(';'))
            {
                case -1: OperacaoSemPontoVirgula(comando); break;
                default: OperacaoComPontoVirgula(comando); break;
            }
        }

        private static int? OperacaoSemPontoVirgula(string comando, bool retornar = false)
        {
            
            float resultado = 0.0f;
            
            var rgx = new Regex(@"[+-/*]");
            if (!rgx.Match(comando).Success && comando.IndexOf("soma") < 0 && comando.IndexOf("subtracao") < 0
                && comando.IndexOf("multiplicacao") < 0 && comando.IndexOf("divisao") < 0)
            {
                throw new ExcecaoDeCalculo("Formato do comando não reconhecido", new Exception("Operação (Ex: soma ou +) não encontrado!"));
            }

            if (Regex.Match(comando, @"[a-z]{4,13}").Success)
            {
                comando = comando.Replace("soma", "+");
                comando = comando.Replace("subtracao", "-");
                comando = comando.Replace("multiplicacao", "*");
                comando = comando.Replace("divisao", "-");
            }
            
            int posicaoOperacao = rgx.Match(comando).Index;
            int operando = Int32.Parse(comando.Substring(0, posicaoOperacao));
            string operacao = comando.Substring(posicaoOperacao, 1);
            int operador = Int32.Parse(comando.Substring(posicaoOperacao + 1));

            switch (operacao)
            {
                case "+": resultado = Soma(operando, operador); break;
                case "-": resultado = Subtracao(operando, operador); break;
                case "/": resultado = Divisao(operando, operador); break;
                case "*": resultado = Multiplicacao(operando, operador); break;
            }

            if (retornar)
            {
                return (int) resultado;
            }
            Console.WriteLine("Resultado da operacao {0}: {1}\n---\n", comando, resultado);
            return null;
        }

        private static void OperacaoComPontoVirgula(string comando)
        {
            OperacaoSemPontoVirgula(comando.Replace(";",""));
        }

        private static bool SairOuContinuar()
        {
            Console.WriteLine("Pressione qualquer tecla para continuar ou ESC para sair");
            var keyInfo = Console.ReadKey();
            return keyInfo.Key == ConsoleKey.Escape;
        }

        private static void SeErro(Exception excecao)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(excecao.Message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t" + excecao.InnerException?.Message ?? "Erro desconhecido");
            Console.ResetColor();
        }

        private static string RemoverAcentos(string valor)
        {
            String normalizedString = valor.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        stringBuilder.Append(c);
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                        stringBuilder.Append('_');
                        break;
                }
            }
            string result = stringBuilder.ToString();
            return String.Join("_", result.Split(new char[] { '_' }
                , StringSplitOptions.RemoveEmptyEntries));
        }

        private class ExcecaoDeCalculo: Exception
        {
            public ExcecaoDeCalculo(string? message, Exception inner) : base(message, inner)
            {
            }
        }

        private class ExcecaoDeSoma : ExcecaoDeCalculo
        {
            public ExcecaoDeSoma(Exception inner) : base("Ocorreu um erro ao tentar somar os valores", inner) {}
        }

        private class ExcecaoDeSubtracao : ExcecaoDeCalculo
        {
            public ExcecaoDeSubtracao(Exception inner) : base("Ocorreu um erro ao tentar subtrair os valores", inner) {}
        }

        private class ExcecaoDeMultiplicacao : ExcecaoDeCalculo
        {
            public ExcecaoDeMultiplicacao(Exception inner) : base("Ocorreu um erro ao tentar multiplicar os valores", inner) {}
        }

        private class ExcecaoDeDivisao : ExcecaoDeCalculo
        {
            public ExcecaoDeDivisao(Exception inner) : base("Ocorreu um erro ao tentar dividir os valores", inner) {}
        }
    }
}
using System;

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

        static int Mutiplicacao(int a, int b)
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

            Console.WriteLine("Digite seu comando: ");
            string comando = Console.ReadLine() ?? "";

            if (comando.Equals("ajuda"))
            {
                EfetuarOperação(true);
            } else if (comando.Equals("sair"))
            {
                return;
            } else
            {
                ProcessaOperacao(comando);
                EfetuarOperação();
            }
        }

        private static void ProcessaOperacao(string comando)
        {
            // TODO: Executar o comando...
            Console.WriteLine("Resultado: {0}", comando);
        }

        private static void SeErro(Exception excecao)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(excecao.Message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t\t" + excecao.InnerException?.Message ?? "Erro desconhecido");
            Console.ResetColor();
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
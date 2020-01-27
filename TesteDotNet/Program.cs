using System;

namespace TesteDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seja bem vindo!!!");
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
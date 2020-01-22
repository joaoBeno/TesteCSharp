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
            return a + b;
        }

        static int Subtracao(int a, int b)
        {
            return a - b;
        }

        static int Mutiplicacao(int a, int b)
        {
            return a * b;
        }

        static int Divisao(int a, int b)
        {
            // Tratar divisão por zero?
            return a / b;
        }
    }
}
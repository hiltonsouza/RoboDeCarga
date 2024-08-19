using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Criação de um novo robô
        Robo meuRobo = new Robo();

        Console.Clear();

        // Movendo o robô da posição O até a posição N (parando antes de O e N)

        //meuRobo.Andar(1, (int)Direcao.Oeste); // Move o robô para antes de O
        meuRobo.Andar(8, (int)Direcao.Oeste); // Move o robô até antes de N
        meuRobo.PrintPosicaoRobo();


        // Coletar informações sobre o número de caixas
        bool verificaCaixa;
        var numeroDeCaixas = ColetarQuantidadeCaixas(out verificaCaixa);
        Console.WriteLine($"Número de caixas a serem carregadas: {numeroDeCaixas}");
        Console.WriteLine("Indo pegar informações sobre os tipos de caixa em T");
        meuRobo.Andar(4, (int)Direcao.Leste); // Move o robô até antes de N
        meuRobo.Andar(3, (int)Direcao.Norte);   

        if (verificaCaixa)
        {      
            //meuRobo.Andar(5, (int)Direcao.Leste); // Move o robô para o Leste em direção a T
            bool tipoCaixa;
            var tipo = TipoCaixa();

            if (tipo == "A")
            {
                meuRobo.Andar(5, (int)Direcao.Norte);
                meuRobo.Andar(3, (int)Direcao.Leste);

                Console.WriteLine("As caixas já estão comigo, e logo estarão em T");

                meuRobo.Andar(3, (int)Direcao.Oeste);
                meuRobo.Andar(5,(int)Direcao.Sul);

                Console.WriteLine($"As caixas do tipo A foram entregues!");
            }

            if(tipo == "B")
            {
                meuRobo.Andar(5, (int)Direcao.Norte);
                meuRobo.Andar(4, (int)Direcao.Oeste);

                Console.WriteLine("As caixas já estão comigo e logo estarão em T");

                meuRobo.Andar(4, (int)Direcao.Leste);
                meuRobo.Andar(5, (int)Direcao.Sul);

                Console.WriteLine($"As caixas do tipo B foram entregues!");
            }

        }

        // Mantém o console aberto até que o usuário pressione uma tecla
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public enum Direcao
    {
        T = 0, // Valor central entre Leste e Oeste
        Norte = 1,
        Sul = -1,
        Leste = -2,
        Oeste = 2
    }

    public class Robo
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private char[,] grid = new char[10, 10]
        {
            { ' ', 'A', ' ', ' ', ' ', ' ', ' ', ' ', 'B', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', 'T', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
            { 'O', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'N' } // N e O nas bordas
        };

        public Robo(int x = 1, int y = 9) // Começa na posição de 'O' uma casa à frente
        {
            X = x;
            Y = y;
        }

        public void Mover(int direcao)
        {
            if (direcao == (int)Direcao.Norte && Y > 1) // Verifica se não está no limite superior
                Y--;
            else if (direcao == (int)Direcao.Sul && Y < 8) // Verifica se não está no limite inferior
                Y++;
            else if (direcao == (int)Direcao.Leste && X > 1) // Verifica se não está no limite direito
                X--;
            else if (direcao == (int)Direcao.Oeste && X < 8) // Verifica se não está no limite esquerdo
                X++;

            PrintPosicaoRobo();
        }

        public void Andar(int qtdPassos, int direcao)
        {
            for (int i = 0; i < qtdPassos; i++)
            {
                Mover(direcao);
                Thread.Sleep(500); // Pausa para visualização
            }
        }

        public void AndarVerticalmente()
        {
            while (Y > 0)
            {
                if (grid[Y - 1, X] == 'A' || grid[Y - 1, X] == 'B')
                {
                    Mover((int)Direcao.Norte);
                }

                if (grid[Y, X] == 'A')
                { Mover((int)Direcao.Leste); }
                else if (grid[Y, X] == 'B')
                {
                    Mover((int)Direcao.Leste);
                }

                Thread.Sleep(500); // Pausa para visualização
            }

            while (Y < 8) // Voltar até Y = 8 (uma casa antes de T)
            {
                Mover((int)Direcao.Sul);
                Thread.Sleep(500); // Pausa para visualização
            }
        }
        public void PrintPosicaoRobo()
        {
            Console.Clear();

            // Reseta a posição fixa dos elementos no grid
            grid[6, 4] = 'T';
            grid[9, 0] = 'O';
            grid[9, 9] = 'N';
            grid[0, 1] = 'A';
            grid[0, 8] = 'B';

            grid[Y, X] = 'R'; // Marca a posição do robô

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }

            grid[Y, X] = ' '; // Limpa a posição antiga do robô para a próxima iteração
        }
    }

    public static int ColetarQuantidadeCaixas(out bool verificaCaixa)
    {
        Console.WriteLine("Oi, quantas caixas preciso carregar?");
        if (int.TryParse(Console.ReadLine(), out int numeroCaixas))
        {
            verificaCaixa = true;
            return numeroCaixas;
        }
        else
        {
            verificaCaixa = false;
            return 0;
        }
    }

    public static string TipoCaixa()
    {
        Console.WriteLine("Oi, qual tipo de caixa você precisa? A ou B?");
        var tipoCaixa = Console.ReadLine().ToUpper();

        if (tipoCaixa == "A" || tipoCaixa == "B")
        {
            //TipoCaixa = true;
            Console.WriteLine($"Legal! Vamos pegar caixas do tipo {tipoCaixa}");
        }
        else
        {
            //TipoCaixa = false;
            Console.WriteLine($"Acho que ainda não temos caixas do tipo {tipoCaixa} =( ");
        }
        return tipoCaixa;
    }

}

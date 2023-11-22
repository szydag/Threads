using System;
using System.Threading;

class TreasureHunt
{
    static int matrixSize = 5;
    static string[,] oceanMatrix = new string[matrixSize, matrixSize];
    static AutoResetEvent[] threadEvents = new AutoResetEvent[3];

    static (int, int) treasureLocation;

    static void Main()
    {
        Console.WriteLine("ŞAZİYEEEEEEEEEEEE");

        // Matrisi oluştur
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                oceanMatrix[i, j] = "water";
            }
        }

        treasureLocation = GetRandomTreasureLocation();
        oceanMatrix[treasureLocation.Item1, treasureLocation.Item2] = "treasure";

        // Thread olaylarını oluştur
        for (int i = 0; i < threadEvents.Length; i++)
        {
            threadEvents[i] = new AutoResetEvent(false);
        }

        // Thread'leri başlat
        Thread[] threads = new Thread[3];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(new ParameterizedThreadStart(TreasureHunter));
            threads[i].Start(i);
        }

        // Hazine bulunana kadar beklet
        WaitHandle.WaitAll(threadEvents);

        // Thread'leri bitir
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
    }

    static void TreasureHunter(object id)
    {
        int threadId = (int)id;
        int startRow = threadId * (matrixSize / 3);
        int endRow = (threadId + 1) * (matrixSize / 3);

        for (int i = startRow; i < endRow; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if ((i, j).Equals(treasureLocation))
                {
                    Console.WriteLine($"Thread {threadId}: Treasure found at ({i}, {j})");
                    threadEvents[threadId].Set();
                    return;
                }
            }
        }
    }

    static (int, int) GetRandomTreasureLocation()
    {
        Random random = new Random();
        return (random.Next(0, matrixSize), random.Next(0, matrixSize));
    }
}

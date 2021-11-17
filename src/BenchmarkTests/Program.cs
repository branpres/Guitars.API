using BenchmarkDotNet.Running;
using BenchmarkTests;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<TestHarness>();
        Console.ReadKey();
    }
}
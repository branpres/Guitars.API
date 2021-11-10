namespace Application.UnitTests.Features.Guitars;

public static class GuitarsTestsHelper
{
    public static List<Guitar> GetGuitars()
    {
        var j45 = new Guitar(GuitarType.Acoustic, 6, "Gibson", "J-45") { Id = 1 };
        AddStrings(j45);

        var stratocaster = new Guitar(GuitarType.Electric, 6, "Fender", "Stratocaster") { Id = 2 };
        AddStrings(stratocaster);

        var lesPaul = new Guitar(GuitarType.Electric, 6, "Gibson", "Les Paul") { Id = 3 };
        AddStrings(lesPaul);

        return new List<Guitar> { j45, stratocaster, lesPaul };
    }

    private static void AddStrings(Guitar guitar)
    {
        guitar.String(1, "010", "E");
        guitar.String(2, "013", "B");
        guitar.String(3, "017", "G");
        guitar.String(4, "DY26", "D");
        guitar.String(5, "DY36", "A");
        guitar.String(6, "DY46", "E");
    }
}
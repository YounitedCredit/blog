using System.Diagnostics.CodeAnalysis;
using AutoBogus;

namespace DataGenerator.DataBuilders
{
    public interface IFaker
    {
        T BuildFake<T>();
    }

    [ExcludeFromCodeCoverage]
    public class Faker : IFaker
    {
        T IFaker.BuildFake<T>() => AutoFaker.Generate<T>();
    }
}

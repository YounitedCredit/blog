using AutoBogus;

namespace DataGenerator
{
    public interface IPurchaseFaker
    {
        public Purchase BuildFake();
    }

    public class PurchaseFaker : IPurchaseFaker
    {
        public Purchase BuildFake() => AutoFaker.Generate<Purchase>();
    }
}

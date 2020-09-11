using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public static class SampleData
    {
        public static IReadOnlyCollection<Guid> MockCustomerIds = new List<Guid>{ MockCustomerId1, MockCustomerId2, MockCustomerId3};
        public static Guid MockCustomerId1 => new Guid("FF194DC2-935D-4F66-86E9-5F3AA20A6C95");
        public static Guid MockCustomerId2 => new Guid("9E22C803-3755-43B6-9F28-CA76FCFCC783");
        public static Guid MockCustomerId3 => new Guid("984F1CCE-44DF-4EDD-BBED-118CC54412F9");
    }
}

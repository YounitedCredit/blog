namespace DependencyInjectionTest.Tests
{
    using DependencyInjectionTest.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NFluent;
    using NSubstitute;
    using System;
    using Xunit;

public class StartupTest
{
    private ServiceProvider _serviceProvider;

    public StartupTest()
    {
        _serviceProvider = null;
    }

    [Fact]
    public void Test_Dependency_injection()
    {
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        var startup = new Startup(configuration);            
        var serviceProvider = services.BuildServiceProvider();

        startup.ConfigureServices(services);

        CheckResolvedObject<ILogger, Logger>();
        CheckResolvedObject<IRepository, Repository>(obj => Check.That(obj.IsSimulation).IsTrue());
    }

    private InterfaceType CheckResolvedObject<InterfaceType, ClassType>()
        where InterfaceType : class
        where ClassType : class
    {
        var createdObj = _serviceProvider.GetService<InterfaceType>();
        Check.That(createdObj).IsNotNull();
        Check.That(createdObj).IsInstanceOf<ClassType>();

        return createdObj;
    }

    private InterfaceType CheckResolvedObject<InterfaceType, ClassType>(Action<InterfaceType> DoFurtherChecks)
        where InterfaceType : class
        where ClassType : class
    {
        var createdObj = CheckResolvedObject<InterfaceType, ClassType>();

        DoFurtherChecks(createdObj);

        return createdObj;
    }
}
}

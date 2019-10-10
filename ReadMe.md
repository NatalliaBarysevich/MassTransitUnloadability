# Demo for "MassTransit can't be used with assembly unloadability in .NET Core"

# Solution description 
1) ConsumerLib.csproj contains classes to be loaded in domain throught collectable assembly load context.
In code it will be named as "extern assembly"
2) MassTransitUnloadability.csproj contains logic with loading and unloading extern assemblies and use extern types in IHost
3) MassTransitUnloadabilityTest.csproj has one test class "HostRunnerTest" demonstrates masstransit influence to unloadability of extern assemblies from domain.
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Hangfire.DynamicJobs")]
[assembly: AssemblyDescription("Dynamic recurring jobs for Hangfire to support multiple code bases with single storage")]

[assembly: InternalsVisibleTo("Hangfire.DynamicJobs.Tests")]

// Allow the generation of mocks for internal types
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
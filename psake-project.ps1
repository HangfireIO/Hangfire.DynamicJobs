Framework 4.5.1
Include "packages\Hangfire.Build.0.2.6\tools\psake-common.ps1"

Task Default -Depends Collect
Task CI -Depends Pack

Task RestoreCore -Depends Clean {
    Exec { dotnet restore }
}

Task CompileCore -Depends RestoreCore {
    Exec { dotnet build -c Release }
}

Task Test -Depends CompileCore -Description "Run unit and integration tests." {
    Exec { dotnet test -c Release "tests\Hangfire.DynamicJobs.Tests\Hangfire.DynamicJobs.Tests.csproj" }
}

Task Collect -Depends Test -Description "Copy all artifacts to the build folder." {
    Collect-Assembly "Hangfire.DynamicJobs" "net451"
    Collect-Assembly "Hangfire.DynamicJobs" "netstandard1.3"
    Collect-Assembly "Hangfire.DynamicJobs" "netstandard2.0"
    Collect-File "LICENSE"
    Collect-File "LICENSE_STANDARD"
    Collect-File "LICENSE_ROYALTYFREE"
}

Task Pack -Depends Collect -Description "Create NuGet packages and archive files." {
    $version = Get-PackageVersion

    Create-Archive "Hangfire.DynamicJobs-$version"
    Create-Package "Hangfire.DynamicJobs" $version
}
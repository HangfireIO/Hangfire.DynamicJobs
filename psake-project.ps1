Include "packages\Hangfire.Build.0.4.2\tools\psake-common.ps1"

Task Default -Depends Pack

Task Test -Depends Compile -Description "Run unit and integration tests." {
    Exec { dotnet test -c release --no-build "tests\Hangfire.DynamicJobs.Tests" }
}

Task Collect -Depends Test -Description "Copy all artifacts to the build folder." {
    Collect-Assembly "Hangfire.DynamicJobs" "net451"
    Collect-Assembly "Hangfire.DynamicJobs" "netstandard1.3"
    Collect-Assembly "Hangfire.DynamicJobs" "netstandard2.0"
    Collect-File "LICENSE"
    Collect-File "LICENSE_STANDARD"
    Collect-File "LICENSE_ROYALTYFREE"
    Collect-File "COPYING.LESSER"
    Collect-File "COPYING"
    Collect-File "README.md"
}

Task Pack -Depends Collect -Description "Create NuGet packages and archive files." {
    $version = Get-PackageVersion

    Create-Package "Hangfire.DynamicJobs" $version
    Create-Archive "Hangfire.DynamicJobs-$version"
}

Task Sign -Depends Pack -Description "Sign artifacts." {
    $version = Get-PackageVersion

    Sign-ArchiveContents "Hangfire.DynamicJobs-$version" "hangfire"
}
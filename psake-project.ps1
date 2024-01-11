Framework 4.5.1
Include "packages\Hangfire.Build.0.2.6\tools\psake-common.ps1"

Task Default -Depends Collect
Task CI -Depends Pack

Task Build -Depends Clean -Description "Restore all the packages and build the whole solution." {
    Exec { dotnet build -c Release }
}

Task Test -Depends Build -Description "Run unit and integration tests." {
    Exec { dotnet test -c Release "tests\Hangfire.DynamicJobs.Tests\Hangfire.DynamicJobs.Tests.csproj" }
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
}

Task Pack -Depends Collect -Description "Create NuGet packages and archive files." {
    $version = Get-PackageVersion

    Create-Archive "Hangfire.DynamicJobs-$version"
    Create-Package2 "Hangfire.DynamicJobs" $version
}

function Create-Package2($project, $version) {
    Write-Host "Creating NuGet package for '$project'..." -ForegroundColor "Green"

    Create-Directory $temp_dir
    Copy-Files "$nuspec_dir\$project.nuspec" $temp_dir

    $commit = (git rev-parse HEAD)

    Try {
        Write-Host "Patching version with '$version'..." -ForegroundColor "DarkGray"
        Replace-Content "$nuspec_dir\$project.nuspec" '%version%' $version
        Write-Host "Patching commit hash with '$commit'..." -ForegroundColor "DarkGray"
        Replace-Content "$nuspec_dir\$project.nuspec" '%commit%' $commit
        Exec { .$nuget pack "$nuspec_dir\$project.nuspec" -OutputDirectory "$build_dir" -BasePath "$build_dir" -Version "$version" }
    }
    Finally {
        Move-Files "$temp_dir\$project.nuspec" $nuspec_dir
    }
}
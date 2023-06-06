# Hangfire.DynamicJobs

The [`Hangfire.DynamicJobs`](https://www.nuget.org/packages/Hangfire.DynamicJobs) package provides dynamic recurring jobs for Hangfire to support multiple code bases with single storage. It does so by wrapping the original job with the `DynamicJob` type available in this package, so no reference to the assembly that contains the original job type is required neither for the recurring job manager nor for the Dashboard UI – only `Hangfire.DynamicJobs` package should be referenced. The original job type will be deserialized and performed as usual during the invocation process.

Hangfire 1.8.0 or later is required for this package. It doesn't use any special storage methods, so it's possible to use any of them.

## Configuration

The `UseDynamicJobs` configuration method should be called during the initialization process. This method will add a custom job renderer for the Dashboard UI to make dynamic jobs more transparent, a special job filter provider implementation that knows how to deal with serialized filters that can be passed when creating a dynamic job.

For newer applications you can call the `UseDynamicJobs` method when configuring Hangfire with the `AddHangfire` method:

```csharp
services.AddHangfire(confiuration => configuration.
    .UseDynamicJobs()
    .UseXXX()); // other configuration methods
```

For older applications, please place the call where `GlobalConfiguration` class is used:

```csharp
GlobalConfiguration.Configuration
    .UseDynamicJobs()
    .UseXXX(); // other configuration methods
```

## Usage

This package adds the `AddOrUpdateDynamic` method overloads for the `IRecurringJobManager` interface that you can use to create dynamic jobs. Everything you need is to pass the arguments as previously when creating a recurring job. Please note that there are no overloads available for the `RecurringJob` static class, so an instance of the `IRecurringJobManager` service is required.

```csharp
IRecurringJobManager manager = new RecurringJobManager(); // or obtain instance using dependency injection

manager.AddOrUpdateDynamic<INewsletterService>("monthly-newsletter", x => x.SendMonthly(), Cron.Monthly());
```

### Passing filters and options

Dynamic jobs don't have access to extension filters applied to the original type or method. However, it is possible to pass them dynamically by using the `Filters` property of the `DynamicRecurringJobOptions` class (that inherits the `RecurringJobOptions`) as shown below. In this case, all the given filters will be serialized with the job, and the job filter provider registered with the `UseDynamicJobs` method call will be used to activate them. Please note that not all filters can support serialization at this stage, if you found any problem, please report this by creating a [GitHub issue](https://github.com/HangfireIO/Hangfire/issues).

You can also use the `DynamicRecurringJobOptions` class to pass other recurring job options as previously.

```csharp
IRecurringJobManager manager = new RecurringJobManager(); // or obtain instance using dependency injection

manager.AddOrUpdateDynamic<INewsletterService>(
    "monthly-newsletter",
    x => x.SendMonthly(),
    Cron.Monthly(),
    new DynamicRecurringJobOptions()
    {
        Filters = new [] { new MyFilterAttribute("newsletter") },
        TimeZone = TimeZoneInfo.Local
    });
```

## Questions? Problems?

If you've discovered a bug, please report it to the [Hangfire GitHub Issues](https://github.com/HangfireIO/Hangfire/issues). Detailed reports with stack traces, and actual and expected behaviors are welcome.

## Building the sources

To build a solution and get assembly files, just run the following command. All build artifacts, including `*.pdb` files will be placed into the `build` folder. **Before proposing a pull request, please use this command to ensure everything is ok.** Btw, you can execute this command from the Package Manager Console window.

```
build
```

To build NuGet packages as well as an archive file, use the `pack` command as shown below. You can find the result files in the `build` folder.

```
build pack
```

To see the full list of available commands, pass the `-docs` switch:

```
build -docs
```

Hangfire uses [psake](https://github.com/psake/psake) build automation tool. All psake tasks and functions are defined in `psake-project`.ps1`.

## License

Copyright &copy; 2023 Hangfire OÜ. Please see the [`LICENSE`](https://github.com/HangfireIO/Hangfire.DynamicJobs/blob/main/LICENSE) file for details.

## Legal

By submitting a Pull Request, you disavow any rights or claims to any changes submitted to the Hangfire project and assign the copyright of those changes to Hangfire OÜ.

If you cannot or do not want to reassign those rights (your employment contract with your employer may not allow this), you should not submit a PR. Open an issue and someone else can do the work.

This is a legal way of saying "If you submit a PR to us, that code becomes ours". 99.9% of the time that's what you intend anyways; we hope it doesn't scare you away from contributing.

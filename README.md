# Hangfire.DynamicJobs

Dynamic recurring jobs for Hangfire to support multiple code bases with single storage.

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

Copyright &copy; 2023 Hangfire OÜ. Please see the [`LICENSE`](https://github.com/HangfireIO/Hangfire.DynamicJobs/blob/main/LICENSE) file for EULA.

## Legal

By submitting a Pull Request, you disavow any rights or claims to any changes submitted to the Hangfire project and assign the copyright of those changes to Hangfire OÜ.

If you cannot or do not want to reassign those rights (your employment contract with your employer may not allow this), you should not submit a PR. Open an issue and someone else can do the work.

This is a legal way of saying "If you submit a PR to us, that code becomes ours". 99.9% of the time that's what you intend anyways; we hope it doesn't scare you away from contributing.
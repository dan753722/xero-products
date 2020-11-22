tap into the Products project and in your command line run:
```
dotnet run
```

Then go to https://localhost:5001/swagger/index.html to play with the APIs interactively.

A few unit tests are also added as part of the refactoring effort in the Tests project folder.

The whole api project has been refactored into a Command / Query fashion.

Products API controller's dependencies are initialised by autofac via DI.

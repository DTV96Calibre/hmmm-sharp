# Building
From the root of the repo, do:

`dotnet build ./path/to/CSharpProject_you_want_to_build`

## Test bench
Pressing **F5** will build and run the test bench in VS Code.

Alternatively build and run the bench with: 

`dotnet build ./Bench/Bench.csproj`

`dotnet run --project ./Bench/Bench.csproj`

## Class library (.DLL)
Build the Hmmm project for a portable DLL for use in other projects.
Targets netstandard2.0 as of this writing.

`dotnet build ./Hmmm/Hmmm.csproj`

# Notes
Supposedly __halt__ is equivalent to a __jump__ to -1, but this cannot be represented in a __jump__ 
instruction nor in the unsigned byte used for addressing. It is assumed that either the assembler in the official
Hmmm simulator aliases `JumpN -1` to `Halt` or this is just a documentation mistake.

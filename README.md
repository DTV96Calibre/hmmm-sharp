# Building
From the root of the repo, do:
`dotnet build ./path/to/project/you/want/to/build`

## Test bench
`dotnet build ./Bench/Bench.csproj`

Run the bench with 

`dotnet run --project ./Bench/Bench.csproj`

## Class library (.DLL)
`dotnet build ./Hmmm/Hmmm.csproj`

# Notes
Supposedly __halt__ is equivalent to a __jump__ to -1, but this cannot be represented in a __jump__ 
instruction nor in the unsigned byte used for addressing. It is assumed that either the assembler in the official
Hmmm simulator aliases `JumpN -1` to `Halt` or this is just a documentation mistake.
REM @echo off

SET SolutionDir=%1
SET ProjectDir=%2

SET BaseDir=%SolutionDir%..\
SET OutputDir=%BaseDir%NuGet\
SET NuGet=%SolutionDir%.nuget\NuGet.exe

cd %ProjectDir%

if not exist %OutputDir%\NUL mkdir %OutputDir%

pushd %ProjectDir%
%NuGet% pack -Build -Properties Configuration=Release;OutputPath=bin\Release -OutputDirectory %OutputDir%
popd

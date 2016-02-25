@echo off
set packages=Scoper Scoper.Autofac Scoper.Ninject

for %%a in (%packages%) do (
    cd %%a
    .\NugetPackCmd.bat
    cd ..
)
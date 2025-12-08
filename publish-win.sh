#!/usr/bin/sh
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:DebugType=None /p:PublishTrimmed=true /p:AssemblyName="legacytui"

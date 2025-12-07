#!/usr/bin/sh
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:DebugType=None /p:PublishTrimmed=true /p:AssemblyName="legacytui"

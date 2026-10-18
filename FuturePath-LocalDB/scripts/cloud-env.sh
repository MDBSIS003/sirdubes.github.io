#!/usr/bin/env bash
# Source this file in the isolated Linux cloud workspace before .NET commands.
export DOTNET_ROOT=${DOTNET_ROOT:-/workspace/tools/dotnet}
export PATH="$DOTNET_ROOT:$PATH"
export DOTNET_CLI_HOME=/workspace/tools/dotnet-home
export NUGET_PACKAGES=/workspace/tools/nuget
export XDG_DATA_HOME=/workspace/.xdg-data
export XDG_RUNTIME_DIR=/workspace/.runtime
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

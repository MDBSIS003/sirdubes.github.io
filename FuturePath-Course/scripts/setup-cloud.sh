#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
source scripts/cloud-env.sh
mkdir -p "$DOTNET_CLI_HOME" "$NUGET_PACKAGES" "$XDG_DATA_HOME" "$XDG_RUNTIME_DIR"
if [[ ! -x "$DOTNET_ROOT/dotnet" ]]; then
  sdk_version=10.0.402
  sdk_temp=$(mktemp -d /tmp/futurepath-sdk.XXXXXX)
  trap 'rm -rf "$sdk_temp"' EXIT
  curl --fail --silent --show-error --location --retry 2 \
    https://builds.dotnet.microsoft.com/dotnet/release-metadata/10.0/releases.json -o "$sdk_temp/releases.json"
  python3 - "$sdk_temp" "$sdk_version" <<'PY'
import json, pathlib, sys
root = pathlib.Path(sys.argv[1])
for release in json.loads((root/'releases.json').read_text())['releases']:
    for sdk in release.get('sdks', []):
        if sdk['version'] == sys.argv[2]:
            for file in sdk['files']:
                if file['rid'] == 'linux-x64' and file['name'].endswith('.tar.gz'):
                    if not file['url'].startswith('https://builds.dotnet.microsoft.com/'):
                        raise SystemExit('Unexpected SDK download host')
                    (root/'url').write_text(file['url'])
                    (root/'checksum').write_text(file['hash'] + '  sdk.tar.gz\n')
                    raise SystemExit(0)
raise SystemExit('Pinned SDK not found in official release metadata')
PY
  curl --fail --silent --show-error --location --retry 2 "$(cat "$sdk_temp/url")" -o "$sdk_temp/sdk.tar.gz"
  (cd "$sdk_temp" && sha512sum --check checksum)
  mkdir -p "$DOTNET_ROOT"
  tar -xzf "$sdk_temp/sdk.tar.gz" -C "$DOTNET_ROOT"
fi
dotnet --version
dotnet restore FuturePath.Development.sln --locked-mode
dotnet build FuturePath.Development.sln -c Release --no-restore
dotnet test tests/FuturePath.Tests -c Release --no-build --logger 'trx;LogFileName=futurepath.trx' --results-directory TestResults

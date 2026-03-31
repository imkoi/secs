#!/usr/bin/env sh
set -eu

RESULTS_DIR="./artifacts/test-results"
mkdir -p "$RESULTS_DIR"

dotnet test ./Secs.sln \
  --collect:"XPlat Code Coverage" \
  --logger "trx;LogFileName=tests.trx" \
  --results-directory "$RESULTS_DIR"

echo "Test results: $RESULTS_DIR/tests.trx"
echo "Coverage files: $RESULTS_DIR/**/coverage.cobertura.xml"

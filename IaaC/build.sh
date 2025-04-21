#!/bin/bash
set -e

# Move to the repo root (assuming this script is in ./IaaC)
cd "$(dirname "$0")/.."

CONFIG="Release"
OUTPUT_DIR="./build"  # This will ensure the build folder is in the root

echo "🔨 Building solution in $(pwd) ..."
dotnet build Quantum.DataFlow.sln --configuration $CONFIG
 
echo "📦 Packing..."
dotnet pack Quantum.DataFlow.sln --configuration $CONFIG --output $OUTPUT_DIR

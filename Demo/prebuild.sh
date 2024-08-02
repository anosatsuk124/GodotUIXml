#!/usr/env/bin bash

TARGET_DIR="$(realpath ./UI)"
OUT_DIR="./SourcesGenerated"

PROJECT_PATH="$(realpath ../)"

if [ ! -d "${OUT_DIR}" ]; then
	mkdir "${OUT_DIR}"
fi

OUT_DIR="$(realpath ${OUT_DIR})"

pushd "${OUT_DIR}"

# Remove all generated files

for f in $(find ${OUT_DIR} -name '*.generated.cs'); do
	rm $f
done

# Build the project

for f in $(find ${TARGET_DIR} -name '*.xml'); do
	echo "Processing $f file..."
	${PROJECT_PATH}/bin/Debug/net8.0/GodotUIXml "$f"
done

popd

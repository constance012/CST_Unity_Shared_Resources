#!/bin/bash

# === Get the directory of this script ===
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

# === Prompt for project name ===
read -p "Enter the project name: " PROJECT_NAME

# === Configuration ===
TARGET_FOLDER="$SCRIPT_DIR/../$PROJECT_NAME/Assets"
LINK_NAME="0_CstSharedResources"
LINK_TARGET="$SCRIPT_DIR/$LINK_NAME"

# === Change to the target folder ===
if [ ! -d "$TARGET_FOLDER" ]; then
  echo "[Error] Target folder does not exist: $TARGET_FOLDER"
  read -n 1 -s -r -p "Press any key to exit..."
  echo
  exit 1
fi
cd "$TARGET_FOLDER" || exit 1

# === Remove old symlink if it exists ===
if [ -L "$LINK_NAME" ] || [ -d "$LINK_NAME" ]; then
  echo "Removing old symbolic link: $LINK_NAME"
  rm -rf "$LINK_NAME"
fi

# === Create new symbolic link ===
ln -s "$LINK_TARGET" "$LINK_NAME"
if [ $? -eq 0 ]; then
  echo "[Success] Symbolic link created: $LINK_NAME -> $LINK_TARGET"
else
  echo "[Error] Failed to create symbolic link."
fi

# === Pause before exit ===
read -n 1 -s -r -p "Press any key to exit..."
echo

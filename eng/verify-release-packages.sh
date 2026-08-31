#!/usr/bin/env bash
#
set -euo pipefail

readonly PACKAGE_IDS=(GeminiDotnet GeminiDotnet.Extensions.AI)

if [ "$#" -ne 2 ]; then
  echo "usage: ${0##*/} <version> <package-directory>" >&2
  exit 2
fi

readonly version="$1"
readonly directory="$2"

expected="$(
  for id in "${PACKAGE_IDS[@]}"; do
    echo "$id.$version.nupkg"
    echo "$id.$version.snupkg"
  done | sort
)"

packed="$(
  find "$directory" -maxdepth 1 -type f \( -name '*.nupkg' -o -name '*.snupkg' \) |
    sed 's#.*/##' | sort
)"

if [ "$expected" != "$packed" ]; then
  echo "the packed files do not match version $version:" >&2
  diff --unified=0 --label expected --label packed \
    <(echo "$expected") <(echo "$packed") >&2 || true
  exit 1
fi

for id in "${PACKAGE_IDS[@]}"; do
  nuspec="$(unzip -p "$directory/$id.$version.nupkg" "$id.nuspec")"
  repository="$(grep -o '<repository [^>]*>' <<<"$nuspec" || true)"

  if ! grep -q 'url="https://' <<<"$repository" ||
    ! grep -q 'commit="[0-9a-f]\{40\}"' <<<"$repository"; then
    echo "$id.$version.nupkg is not source-linked, its repository metadata is" \
      "\"$repository\"" >&2
    exit 1
  fi
done

echo "$version: verified $(echo "$packed" | wc -l | tr -d ' ') packages in $directory"

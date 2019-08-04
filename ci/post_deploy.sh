#!/usr/bin/env bash

set -e

git add -A .
git stash
git checkout -b gh-pages -t origin/gh-pages
git submodule update --recursive --remote --rebase --init
git add -A .
git commit -m "Updating submodules..." 
git push origin gh-pages --force
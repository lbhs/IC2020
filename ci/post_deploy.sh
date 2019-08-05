#!/usr/bin/env bash

set -e

git add -A .
git stash
git fetch origin gh-pages
git checkout -t -b origin/gh-pages
git submodule update --recursive --remote --merge --init
git add -A .
git commit -m "Updating submodules..." 
git push origin gh-pages --force
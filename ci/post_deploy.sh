#!/usr/bin/env bash

set -e

echo "Starting the post-deployment update to gh-pages."
echo "Using git version: $(git --version)"

git add -A .
git stash
git fetch origin gh-pages
git checkout -t -b origin/gh-pages
git submodule update --recursive --remote --merge --init
git add -A .
git commit -m "Updating submodules..." 
git push origin gh-pages --force
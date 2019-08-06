#!/usr/bin/env bash

set -e

echo "Starting the post-deployment update to gh-pages."
echo "Using git version: $(git --version)"
echo "Having remotes: $(git remote -v show)"
echo "Having branches: $(git branch --list)"

# Stash the existing changes
git add -A .
git stash

# Setup the repo for fetching normally
echo "Fetching origin:"
git fetch origin

# Change to the gh-pages branch and update the submodules
echo "Checking out gh-pages branch"
git checkout -b gh-pages -t origin/gh-pages

echo "Before updating submodules"
git submodule update --recursive --remote --merge --init
git add -A .

echo "Before commit: $(git status)"
git commit -m "Updating submodules..." 
git push origin gh-pages --force

echo "Completed updating the gh-pages submodules."
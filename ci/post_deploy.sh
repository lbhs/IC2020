#!/usr/bin/env bash

set -e

echo "Starting the post-deployment update to gh-pages."
echo "Using git version: $(git --version)"

# Stash the existing changes
git add -A .
git stash

# Setup the repo for fetching normally
git config --replace-all remote.origin.fetch +refs/head/*:refs/remotes/origin/*
git fetch

# Change to the gh-pages branch and update the submodules
git checkout -t -b origin/gh-pages
git submodule update --recursive --remote --merge --init
git add -A .
git commit -m "Updating submodules..." 
git push origin gh-pages --force

echo "Completed updating the gh-pages submodules."
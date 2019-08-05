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

echo "Checking out gh-pages branch"
cd ..
git clone git@github.com:lbhs/IC2020 tmp-ic2020
cd tmp-ic2020

# Change to the gh-pages branch and update the submodules
echo "Before checkout"
git checkout -t -b origin/gh-pages
echo "Before updating submodules"
git submodule update --recursive --remote --merge --init
git add -A .
echo "Before commit: $(git status)"
git commit -m "Updating submodules..." 
git push origin gh-pages --force

echo "Completed updating the gh-pages submodules."
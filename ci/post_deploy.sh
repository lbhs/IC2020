#!/usr/bin/env bash

set -e

echo "Starting the post-deployment update to gh-pages: $(pwd -P)"

# Setup the cert
openssl aes-256-cbc -K ${encrypted_d4473614101c_key} -iv ${encrypted_d4473614101c_iv} -in ci/config -out config.tar -d
tar xvf config.tar
mv lbhs_github_cert ~/.ssh/ 
chmod 400 ~/.ssh/lbhs_github_cert

# Clone the repo
cd ..
GIT_SSH_COMMAND="ssh -i ~/.ssh/lbhs_github_cert" git clone git@github.com:lbhs/IC2020.git --branch gh-pages ghpages
cd ghpages

echo "Before updating submodules"
git submodule update --recursive --remote --merge --init
git add -A .

echo "Before commit: $(git status)"
git commit -m "Updating submodules..."
GIT_SSH_COMMAND="ssh -i ~/.ssh/lbhs_github_cert" git push origin gh-pages --force

echo "Completed updating the gh-pages submodules."
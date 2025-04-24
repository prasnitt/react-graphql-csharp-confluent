#!/bin/bash
set -e  # Ex

cd git/prasnitt/react-graphql-csharp-confluent
git reset --hard
git pull origin main
cd server/GraphQLDemo/ChatHub
dotnet build -c Release
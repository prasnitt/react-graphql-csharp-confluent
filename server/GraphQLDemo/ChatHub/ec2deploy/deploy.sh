#!/bin/bash
set -e  # Ex


cd /home/ec2-user/react-graphql-csharp-confluent
git reset --hard
git pull origin main
cd server/GraphQLDemo/ChatHub
dotnet build -c Release -o out

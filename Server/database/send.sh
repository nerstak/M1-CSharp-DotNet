#!/bin/sh
echo "Restarting container...";
docker restart pg-docker-csharp;

echo "Sending files...";
for f in scripts/*; 
do 
	docker cp $f pg-docker-csharp:/home/; 
done

echo "Setting up database...";
docker exec -w /home pg-docker-csharp ./run.sh;
docker-compose stop 
docker-compose rm
docker rmi job
cd D:\gitnew\dapr-server\Services\JobService\bin\Release\net6.0\publish
docker build -t job .
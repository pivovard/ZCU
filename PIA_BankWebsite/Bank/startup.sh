#!/bin/bash

echo "+++++++++++++++++++++++++"
echo "Starting SQL server"
echo "+++++++++++++++++++++++++"

docker pull mcr.microsoft.com/mssql/server:2017-latest

docker run --rm -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Bank_666' --name 'sql1' -p 1433:1433 -v sql1data:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2017-latest

docker exec -it sql1 mkdir /var/opt/mssql/backup

docker cp BankContext.sql sql1:/var/opt/mssql/backup

docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Bank_666' -i /var/opt/mssql/backup/BankContext.sql

echo "+++++++++++++++++++++++++"
echo "SQL server running"
echo "+++++++++++++++++++++++++"


echo "+++++++++++++++++++++++++"
echo "Starting bank application"
echo "+++++++++++++++++++++++++"

docker build -t bank  .
docker run -it --rm -p "80:80" -p "433:433" --link sql1 bank
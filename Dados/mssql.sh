#!/bin/sh

PASSWORD="V3ry\$3cur3Pa\$\$w0rd"

# Run server
docker volume create sqlstorage > /dev/null
docker run                                                 \
    -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$PASSWORD"    \
    --name sql --hostname sql                              \
    --mount source=sqlstorage,target=/var/opt/mssql        \
    -p 1433:1433 -d --rm                                   \
    mcr.microsoft.com/mssql/server:2022-latest > /dev/null
trap 'trap - EXIT; docker stop "sql"; exit' EXIT TERM INT HUP

DOCKER_ARGS="/opt/mssql-tools18/bin/sqlcmd -No -S localhost -U SA -P $PASSWORD"

# Wait for MSSQL server to start
while :; do
    if ! docker exec -i "sql" $DOCKER_ARGS -q "SELECT 1" </dev/null 2>&1 | grep -q "Login failed"; then
        break
    fi
done

# Run interactive session or file
[ "$#" -eq 1 ] && docker cp "$1" sql:/file.sql >/dev/null && EXTRA_ARGS="-i file.sql"
docker exec -it "sql" $DOCKER_ARGS $EXTRA_ARGS

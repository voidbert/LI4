#!/bin/sh

# Copyright 2025 Ana Cerqueira, Humberto Gomes, João Torres, José Lopes, José Matos
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#    http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

PASSWORD="V3ry\$3cur3Pa\$\$w0rd"

# Run server
[ "$#" -eq 2 ] && MOUNT_ARGS="-v $2:/mnt"

docker volume create sqlstorage > /dev/null
docker run                                                 \
    --user root                                            \
    -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$PASSWORD"    \
    --name sql --hostname sql                              \
    --mount source=sqlstorage,target=/var/opt/mssql        \
    $MOUNT_ARGS                                            \
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
[ "$#" -ge 1 ] && docker cp "$1" sql:/file.sql >/dev/null && EXTRA_ARGS="-i file.sql"
docker exec -it "sql" $DOCKER_ARGS $EXTRA_ARGS

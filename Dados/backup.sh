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

if [ "$#" -lt 1 ]; then
    echo "Usage: ./backup.sh <output_directory>"
    exit 1
fi

FILENAME="$(date +"%Y-%m-%d_%H-%M-%S").bak"
SQLFILE="$(mktemp)"
printf "BACKUP DATABASE WeaponsRUs TO DISK = '/mnt/$FILENAME' WITH FORMAT;" > "$SQLFILE"

./mssql.sh "$SQLFILE" "$1"
rm "$SQLFILE"

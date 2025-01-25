#!/bin/sh

if [ "$#" -lt 1 ]; then
    echo "Usage: ./backup.sh <output_directory>"
    exit 1
fi

FILENAME="$(date +"%Y-%m-%d_%H-%M-%S").bak"
SQLFILE="$(mktemp)"
printf "BACKUP DATABASE WeaponsRUs TO DISK = '/mnt/$FILENAME' WITH FORMAT;" > "$SQLFILE"

./mssql.sh "$SQLFILE" "$1"
rm "$SQLFILE"

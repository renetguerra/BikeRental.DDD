#!/bin/bash
set -e

echo "Esperando a que SQL Server esté disponible..."

until /opt/mssql-tools/bin/sqlcmd -S db -U sa -P 'Adm1nP@ssw0rd!' -Q "SELECT 1" > /dev/null 2>&1; do
  echo "SQL Server no está listo todavía... esperando 2 segundos."
  sleep 2
done

echo "SQL Server está listo."

if [ -f /scripts/BikeRental_data.sql ]; then
  echo "Ejecutando script de inicialización de base de datos..."
  /opt/mssql-tools/bin/sqlcmd -S db -U sa -P 'Adm1nP@ssw0rd!' -i /scripts/BikeRental_data.sql
else
  echo "Archivo /scripts/BikeRental_data.sql no encontrado. Saltando inicialización."
fi

echo "Iniciando aplicación .NET..."
exec dotnet BikeRental.DDD.API.dll

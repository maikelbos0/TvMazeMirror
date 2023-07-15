# TvMazeMirror

Een mirror van de shows beschikbaar via de api van TVmaze.com. Deze mirror haalt automatisch alle shows op waarvan de première na 1 januari 2014. Het is tevens mogelijk shows toe te voegen, te bewerken en te verwijderen.

# Development

Dit project maakt na een debug-build automatisch de database aan op de locatie die in de appsettings.Development.json is gedefinieerd in AppSettings::DatabaseConnectionString. Als deze connection string een geldige server- en databasenaam bevat is het project direct te starten.

Er wordt tevens een idempotent .sql script gegenereerd voor deployment. Dit script is te vinden in ./src/TvMazeMirror/Database/MigrateDatabase.sql


Database migration
dotnet ef migrations add "Init" --project src/Infrastructure --startup-project src/API --output-dir Persistence/Migrations
dotnet ef migrations remove --project src/Infrastructure --startup-project src/API
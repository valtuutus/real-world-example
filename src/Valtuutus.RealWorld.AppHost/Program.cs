var builder = DistributedApplication.CreateBuilder(args);


var db = "valtuutus";

var pgusername = builder.AddParameter("pgusername", secret: true);
var pgpassword = builder.AddParameter("pgpassword", secret: true);
var pg = builder.AddPostgres("postgres", pgusername, pgpassword, port: 5432)
    .WithDataVolume("pg_data")
    .WithInitBindMount("db_bootstrap")
    .WithEnvironment("POSTGRES_DB", db)
    .WithArgs("-c", "wal_level=logical")
    .WithAnnotation(new ContainerLifetimeAnnotation { Lifetime = ContainerLifetime.Persistent });

var valtuutus = pg.AddDatabase(db);


var api = builder.AddProject<Projects.Valtuutus_RealWorld_Api>("api")
    .WithEndpoint("http", endpoint => { endpoint.IsProxied = false; })
    .WithReference(valtuutus)
    .WaitFor(valtuutus);

builder.Build().Run();
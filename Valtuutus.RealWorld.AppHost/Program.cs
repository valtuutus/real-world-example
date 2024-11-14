var builder = DistributedApplication.CreateBuilder(args);


var db = "valtuutus";
var pg = builder.AddPostgres("postgres")
    .WithDataVolume("pg_data")
    .WithInitBindMount("db_bootstrap")
    .WithEnvironment("POSTGRES_DB", db)
    .WithAnnotation(new ContainerLifetimeAnnotation { Lifetime = ContainerLifetime.Persistent })
    .AddDatabase(db);

builder.AddProject<Projects.Valtuutus_RealWorld_Api>("api")
    .WithReference(pg)
    .WaitFor(pg);

builder.Build().Run();
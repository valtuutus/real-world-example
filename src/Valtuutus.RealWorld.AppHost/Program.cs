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

builder.AddContainer("debezium", "debezium/server", "3.0.0.Final")
    .WithVolume("debezium_data", "/debezium/data")
    .WithAnnotation(new ContainerLifetimeAnnotation { Lifetime = ContainerLifetime.Persistent })
    .WithEnvironment("DEBEZIUM_SOURCE_CONNECTOR_CLASS", "io.debezium.connector.postgresql.PostgresConnector")
    .WithEnvironment("DEBEZIUM_SOURCE_OFFSET_STORAGE_FILE_FILENAME","data/offsets.dat")
    .WithEnvironment("DEBEZIUM_SOURCE_OFFSET_FLUSH_INTERVAL_MS","0")
    .WithEnvironment("DEBEZIUM_SOURCE_DATABASE_HOSTNAME","host.docker.internal")
    .WithEnvironment("DEBEZIUM_SOURCE_DATABASE_PORT","5432")
    .WithEnvironment("DEBEZIUM_SOURCE_DATABASE_USER","postgres")
    .WithEnvironment("DEBEZIUM_SOURCE_DATABASE_PASSWORD","postgres")
    .WithEnvironment("DEBEZIUM_SOURCE_DATABASE_DBNAME","valtuutus")
    .WithEnvironment("DEBEZIUM_SOURCE_TABLE_EXCLUDE_LIST", "public.attributes,public.relation_tuples,public.transactions")
    .WithEnvironment("DEBEZIUM_SOURCE_TOPIC_PREFIX","valtuutus")
    .WithEnvironment("DEBEZIUM_SOURCE_PLUGIN_NAME","pgoutput")
    .WithEnvironment("DEBEZIUM_SINK_TYPE","rabbitmq")
    .WithEnvironment("DEBEZIUM_SINK_RABBITMQ_CONNECTION_HOST","host.docker.internal")
    .WithEnvironment("DEBEZIUM_SINK_RABBITMQ_CONNECTION_PORT","5672")
    .WithEnvironment("DEBEZIUM_SINK_RABBITMQ_CONNECTION_USERNAME","guest")
    .WithEnvironment("DEBEZIUM_SINK_RABBITMQ_CONNECTION_PASSWORD","guest")
    .WithEnvironment("DEBEZIUM_SINK_RABBITMQ_EXCHANGE", "amq.topic")
    .WithEnvironment("DEBEZIUM_SINK_RABBITMQ_ROUTINGKEY", "valtuutus.cdc")
    .WithEnvironment("DEBEZIUM_FORMAT_KEY", "json")
    .WithEnvironment("DEBEZIUM_FORMAT_VALUE", "json")
    .WithEnvironment("QUARKUS_LOG_CONSOLE_JSON", "FALSE")
    .WaitFor(pg);


var amqp = builder.AddContainer("amqp", "cloudamqp/lavinmq")
    .WithEndpoint(5672, 5672, "amqp")
    .WithEndpoint(15672, 15672)
    .WithAnnotation(new ContainerLifetimeAnnotation { Lifetime = ContainerLifetime.Persistent });
builder.AddProject<Projects.Valtuutus_RealWorld_Api>("api")
    .WithReference(valtuutus)
    .WaitFor(amqp)
    .WaitFor(valtuutus);

builder.Build().Run();
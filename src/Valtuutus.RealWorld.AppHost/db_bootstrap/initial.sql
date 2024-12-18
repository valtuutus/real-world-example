

-- Create "attributes" table
CREATE TABLE "public"."attributes"
(
    "id"          bigint                 NOT NULL GENERATED ALWAYS AS IDENTITY,
    "entity_type" character varying(256) NOT NULL,
    "entity_id"   character varying(64)  NOT NULL,
    "attribute"   character varying(64)  NOT NULL,
    "value"       jsonb                  NOT NULL,
    created_tx_id char(26)               NOT NULL,
    deleted_tx_id char(26),
    PRIMARY KEY ("id")
);
-- Create index "idx_attributes" to table: "attributes"
CREATE INDEX "idx_attributes" ON "public"."attributes" ("entity_type", "entity_id", "attribute");
-- Create "relation_tuples" table
CREATE TABLE "public"."relation_tuples"
(
    "id"               bigint                 NOT NULL GENERATED ALWAYS AS IDENTITY,
    "entity_type"      character varying(256) NOT NULL,
    "entity_id"        character varying(64)  NOT NULL,
    "relation"         character varying(64)  NOT NULL,
    "subject_type"     character varying(256) NOT NULL,
    "subject_id"       character varying(64)  NOT NULL,
    "subject_relation" character varying(64)  NOT NULL,
    created_tx_id      char(26)               NOT NULL,
    deleted_tx_id      char(26),
    PRIMARY KEY ("id")
);
-- Create index "idx_tuples_entity_relation" to table: "relation_tuples"
CREATE INDEX "idx_tuples_entity_relation" ON "public"."relation_tuples" ("entity_type", "relation");
-- Create index "idx_tuples_subject_entities" to table: "relation_tuples"
CREATE INDEX "idx_tuples_subject_entities" ON "public"."relation_tuples" ("entity_type", "relation", "subject_type", "subject_id");
-- Create index "idx_tuples_user" to table: "relation_tuples"
CREATE INDEX "idx_tuples_user" ON "public"."relation_tuples" ("entity_type", "entity_id", "relation", "subject_id");
-- Create index "idx_tuples_userset" to table: "relation_tuples"
CREATE INDEX "idx_tuples_userset" ON "public"."relation_tuples" ("entity_type", "entity_id", "relation", "subject_type",
                                                                 "subject_relation");


CREATE TABLE "public"."transactions"
(
    "id"         char(26)    NOT NULL,
    "created_at" timestamptz NOT NULL,
    PRIMARY KEY ("id")
);
CREATE INDEX "idx_created_at" ON public.transactions USING btree (created_at);

CREATE UNIQUE INDEX "unique_attributes" on public.attributes (entity_type, entity_id, attribute) where deleted_tx_id is null;

CREATE TABLE "Workspaces" (
                              "Id" uuid NOT NULL,
                              "Name" character varying(100) NOT NULL,
                              "Public" boolean NOT NULL,
                              CONSTRAINT "PK_Workspaces" PRIMARY KEY ("Id")
);


CREATE TABLE "Projects" (
                            "Id" uuid NOT NULL,
                            "WorkspaceId" uuid NOT NULL,
                            "Name" character varying(100) NOT NULL,
                            CONSTRAINT "PK_Projects" PRIMARY KEY ("Id"),
                            CONSTRAINT "FK_Projects_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES "Workspaces" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Teams" (
                         "Id" uuid NOT NULL,
                         "WorkspaceId" uuid NOT NULL,
                         "Name" character varying(100) NOT NULL,
                         CONSTRAINT "PK_Teams" PRIMARY KEY ("Id"),
                         CONSTRAINT "FK_Teams_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES "Workspaces" ("Id") ON DELETE CASCADE
);


CREATE TABLE "ProjectStatuses" (
                                   "Id" uuid NOT NULL,
                                   "ProjectId" uuid NOT NULL,
                                   "Type" integer NOT NULL,
                                   "Name" character varying(100) NOT NULL,
                                   CONSTRAINT "PK_ProjectStatuses" PRIMARY KEY ("Id"),
                                   CONSTRAINT "FK_ProjectStatuses_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);


CREATE TABLE "ProjectTeamAssignees" (
                                        "ProjectId" uuid NOT NULL,
                                        "TeamId" uuid NOT NULL,
                                        "Type" integer NOT NULL,
                                        CONSTRAINT "PK_ProjectTeamAssignees" PRIMARY KEY ("ProjectId", "TeamId"),
                                        CONSTRAINT "FK_ProjectTeamAssignees_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE,
                                        CONSTRAINT "FK_ProjectTeamAssignees_Teams_TeamId" FOREIGN KEY ("TeamId") REFERENCES "Teams" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Users" (
                         "Id" uuid NOT NULL,
                         "Name" character varying(100) NOT NULL,
                         "Email" character varying(256) NOT NULL,
                         "PasswordHash" character varying(256) NOT NULL,
                         CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "UserTeams" (
    "UserId" uuid NOT NULL,
    "TeamId" uuid NOT NULL,
    CONSTRAINT "PK_UserTeams" PRIMARY KEY ("UserId", "TeamId"),
    CONSTRAINT "FK_UsersTeams_Teams" FOREIGN KEY ("TeamId") REFERENCES "Teams" ("Id"),
    CONSTRAINT "FK_UsersTeams_Users" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id")
);


CREATE TABLE "Tasks" (
                         "Id" uuid NOT NULL,
                         "ProjectStatusId" uuid NOT NULL,
                         "ProjectId" uuid NOT NULL,
                         "Name" character varying(100) NOT NULL,
                         CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id"),
                         CONSTRAINT "FK_Tasks_ProjectStatuses_ProjectStatusId" FOREIGN KEY ("ProjectStatusId") REFERENCES "ProjectStatuses" ("Id") ON DELETE CASCADE,
                         CONSTRAINT "FK_Tasks_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);


CREATE TABLE "ProjectUserAssignees" (
                                        "ProjectId" uuid NOT NULL,
                                        "UserId" uuid NOT NULL,
                                        "Type" integer NOT NULL,
                                        CONSTRAINT "PK_ProjectUserAssignees" PRIMARY KEY ("ProjectId", "UserId"),
                                        CONSTRAINT "FK_ProjectUserAssignees_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE,
                                        CONSTRAINT "FK_ProjectUserAssignees_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "WorkspaceAssignees" (
                                      "WorkspaceId" uuid NOT NULL,
                                      "UserId" uuid NOT NULL,
                                      "Type" integer NOT NULL,
                                      CONSTRAINT "PK_WorkspaceAssignees" PRIMARY KEY ("WorkspaceId", "UserId"),
                                      CONSTRAINT "FK_WorkspaceAssignees_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                                      CONSTRAINT "FK_WorkspaceAssignees_Workspaces_WorkspaceId" FOREIGN KEY ("WorkspaceId") REFERENCES "Workspaces" ("Id") ON DELETE CASCADE
);


CREATE TABLE "TaskAssignees" (
                                 "TaskId" uuid NOT NULL,
                                 "UserId" uuid NOT NULL,
                                 CONSTRAINT "PK_TaskAssignees" PRIMARY KEY ("TaskId", "UserId"),
                                 CONSTRAINT "FK_TaskAssignees_Tasks_TaskId" FOREIGN KEY ("TaskId") REFERENCES "Tasks" ("Id") ON DELETE CASCADE,
                                 CONSTRAINT "FK_TaskAssignees_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");


CREATE INDEX "IX_Projects_WorkspaceId" ON "Projects" ("WorkspaceId");


CREATE INDEX "IX_ProjectStatuses_ProjectId" ON "ProjectStatuses" ("ProjectId");


CREATE INDEX "IX_ProjectTeamAssignees_TeamId" ON "ProjectTeamAssignees" ("TeamId");


CREATE INDEX "IX_ProjectUserAssignees_UserId" ON "ProjectUserAssignees" ("UserId");


CREATE INDEX "IX_TaskAssignees_UserId" ON "TaskAssignees" ("UserId");


CREATE INDEX "IX_Tasks_ProjectId" ON "Tasks" ("ProjectId");


CREATE INDEX "IX_Tasks_ProjectStatusId" ON "Tasks" ("ProjectStatusId");


CREATE INDEX "IX_Teams_WorkspaceId" ON "Teams" ("WorkspaceId");


CREATE INDEX "IX_UserTeams_TeamId" ON "UserTeams" ("TeamId");

CREATE INDEX "IX_UserTeams_UserId" ON "UserTeams" ("UserId");

CREATE INDEX "IX_UserTeams_UserId_TeamId" ON "UserTeams" ("UserId", "TeamId");


CREATE INDEX "IX_WorkspaceAssignees_UserId" ON "WorkspaceAssignees" ("UserId");



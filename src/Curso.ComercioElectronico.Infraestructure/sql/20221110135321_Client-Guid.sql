BEGIN TRANSACTION;



CREATE TABLE "ef_temp_Ordenes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Ordenes" PRIMARY KEY,
    "ClienteId" TEXT NOT NULL,
    "Estado" INTEGER NOT NULL,
    "Fecha" TEXT NOT NULL,
    "FechaAnulacion" TEXT NULL,
    "Observaciones" TEXT NULL,
    "Total" TEXT NOT NULL,
    CONSTRAINT "FK_Ordenes_Clientes_ClienteId" FOREIGN KEY ("ClienteId") REFERENCES "Clientes" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Ordenes" ("Id", "ClienteId", "Estado", "Fecha", "FechaAnulacion", "Observaciones", "Total")
SELECT "Id", "ClienteId", "Estado", "Fecha", "FechaAnulacion", "Observaciones", "Total"
FROM "Ordenes";

CREATE TABLE "ef_temp_Clientes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Clientes" PRIMARY KEY,
    "Nombres" TEXT NOT NULL
);

INSERT INTO "ef_temp_Clientes" ("Id", "Nombres")
SELECT "Id", "Nombres"
FROM "Clientes";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Ordenes";

ALTER TABLE "ef_temp_Ordenes" RENAME TO "Ordenes";

DROP TABLE "Clientes";

ALTER TABLE "ef_temp_Clientes" RENAME TO "Clientes";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Ordenes_ClienteId" ON "Ordenes" ("ClienteId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221110135321_Client-Guid', '6.0.10');

COMMIT;
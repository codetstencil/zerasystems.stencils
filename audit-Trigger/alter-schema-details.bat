sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [MoreInfoLink] nvarchar(4000) NULL; "
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [GettingStartedLink] nvarchar(4000) NULL; "
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [ReleaseNotesLink] nvarchar(4000) NULL; "
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [CategoryId] int NULL DEFAULT(0);"
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [Category] nvarchar(4000) NULL; "
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [Is3rdParty] bit NOT NULL DEFAULT(0);"
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [AutomaticallyUpdate] bit NOT NULL DEFAULT(0);"
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [Identifier] uniqueidentifier NULL DEFAULT('00000000-0000-0000-0000-000000000000');"
sqlcompact -d "Data Source=%1" -q "ALTER TABLE [StencilDetails] ADD [License] int NULL DEFAULT(1);"
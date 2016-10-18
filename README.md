# DynamicModelMigrator
C# library to migrate a database table to match a given Type

`Install-Package DynamicModelMigrator`

##Basic Usage##
`DynamicModelMigrator.Migrate<SomeClass>(someConnectionString)`


Table names default to the name of the class you provide, you may specify a table name by providing it to the call to Migrate

`DynamicModelMigrator.Migrate<SomeClass>(someConnectionString, "MyObnoxiouslyLongAndAmbiguousTableName")`

##Defaulting variable length fields##

`[StringLength(6)]`

DynamicModelMigrator does *not* default string fields to max length, because frankly, that's stupid. You will need to provide a length for your variable length fields or they will default to a length of 1.




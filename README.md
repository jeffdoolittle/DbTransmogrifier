DbTransmogrifier
================

**trans·mog·ri·fy** */transˈmägrəˌfī/*

Verb: Transform, esp. in a surprising or magical manner.

Synonyms: transform - alter - change - transmute - metamorphose

Description
-----------

The DbTransmogrifier provides simple, convention based database migrations for .NET.  It currently supports Microsoft SQL Server and SQL Server express.  It would be fairly trivial to extend it to support Oracle, SQL CE, Firebird, MySql, Postgres or any other RDBMS (like MS Access).


Discovering Migrations
----------------------

DbTransmogrifier currently supports one simple convention for discovering migrations.  It looks for classes that implement an interface called "IMigration" decorated with a MigrationAttribute.  See "Defining Migrations in Your Assembly" below for more details.

You do not have to reference the DbTransmogrifier assembly from your project in order to process migrations.  Currently DbTransmogrifier comes with a single, simple convention for discovering and applying your migrations.  Simply place the DbTransmogrifier executable in the same directory as the assembly which contains your migrations.


Defining Migrations in Your Assembly
------------------------------------

### Migration Attribute

Create a **MigrationAttribute** class in your migration assembly.  This attribute should have a **Version** integer property on it which DbTransmogrifier will use to determine the version of each migration.  See the *SampleMigrations* project for a detailed example.


### IMigration Interface

DbTransmogrifier looks for an **IMigration** interface with two methods each returning an IEnumerable&lt;string&gt;.  One method should be called *Up()* and the other *Down()*.  See the SampleMigrations project for a detailed example.

Possible Plans for the Future
-----------------------------

* Support other RDBMS's (Oracle, SQL CE, Firebird, MySql, Postgres, etc.)
* Allow for alternative migration discovery conventions (file system based migrations, alternative assembly scanning options, etc.)

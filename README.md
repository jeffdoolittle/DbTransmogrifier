DbTransmogrifier
================

**trans·mog·ri·fy** */transˈmägrəˌfī/*

Verb: Transform, esp. in a surprising or magical manner.

Synonyms: transform - alter - change - transmute - metamorphose

Description
-----------

The DbTransmogrifier provides simple, convention based database migrations for .NET.  The following RDBMS's are supported:

* Microsoft SQL Server
* Microsoft SQL Server Express
* PostgreSql

It would be fairly trivial to extend it to support Oracle, SQL CE, Firebird, MySql or any other RDBMS (like MS Access).

Discovering Migrations
----------------------

DbTransmogrifier currently supports one simple convention for discovering migrations.  It looks for classes that implement an interface called "IMigration" decorated with a MigrationAttribute.  See [Defining Migrations in Your Assembly](#defining-migrations-in-your-assembly) for more details.

You do not have to reference the DbTransmogrifier assembly from your project in order to process migrations.  Currently DbTransmogrifier comes with a single, simple convention for discovering and applying your migrations.  Simply place the DbTransmogrifier executable in the same directory as the assembly which contains your migrations.


Defining Migrations in Your Assembly
------------------------------------

### Migration Attribute

Create a **MigrationAttribute** class in your migration assembly.  This attribute should have a **Version** integer property on it which DbTransmogrifier will use to determine the version of each migration.  For example:

```
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class MigrationAttribute : Attribute
{
	public MigrationAttribute(int version)
	{
		Version = version;
	}

	public int Version { get; private set; }
}
```

See the *SampleMigrations* project for a detailed example.

### IMigration Interface

DbTransmogrifier looks for an **IMigration** interface with the following definition:

```
public interface IMigration
{
	IEnumerable<string> Up();
	IEnumerable<string> Down();
}
```

Any class implementing the ```IMigration``` interface, and decorated with the ```MigrationAttribute``` will be processed as a migration by the DBTransmogrifier. See the *SampleMigrations* project for a detailed example.

Command line options
------------------------------------

DBTransmogrifier supports the following command line options:

### Database level commands

* ```--init``` :: Creates the target database if it does not exist. Creates the "SchemaVersion" table if it does not exist.
* ```--tear-down``` :: Deletes all database Tables, Constraints, Views, Functions and Stored Procedures. Resets "SchemaVersion" table to version 0 (zero).  Basically restores the databse to it's initialized state.
* ```--drop``` :: Drops the database. No warnings, no redo, no cancel.  Be careful! You've been warned.

### Migration commands

* ```--current-version``` :: Displays the current schema version number.
* ```--up-to-latest``` :: Applies all up migrations after the current version up to the maximum version available.
* ```--up-to={version}``` :: Applies all up migrations after the current version up to the version specified.
* ```--down-to={version}``` :: Applies all down migrations from the current version down to the version specified.

### Other commands

* ```--help``` :: Displays command line help. Basically just a dump of available command line options to help jog your memory if you forget them.

Possible Plans for the Future
-----------------------------

* Improve build process
* Support other RDBMS's (Oracle, SQL CE, Firebird, MySql, etc.)
* Allow for alternative migration discovery conventions (file system based migrations, alternative assembly scanning options, etc.)
* Create a NuGet installation package
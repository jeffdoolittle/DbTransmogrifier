# DbTransmogrifier

**trans·mog·ri·fy** */transˈmägrəˌfī/*

Verb: Transform, esp. in a surprising or magical manner.

Synonyms: transform - alter - change - transmute - metamorphose

## Description

The DbTransmogrifier provides simple, convention based database migrations for .NET.  The following RDBMS's are supported:

* Microsoft SQL Server
* Microsoft SQL Server Express
* PostgreSql

It would be fairly trivial to extend it to support Oracle, SQL CE, Firebird, MySql or any other RDBMS (like MS Access).

DbTransmogrifier is licensed under the [MIT license](./license).

DbTransmogrifier is also available as a [NuGet package](http://nuget.org/packages/DbTransmogrifier/).

## Discovering Migrations

DbTransmogrifier currently supports one simple convention for discovering migrations.  It looks for classes that implement an interface called "IMigration" decorated with a MigrationAttribute.  See [Defining Migrations in Your Assembly](#defining-migrations-in-your-assembly) for more details.

You do not have to reference the DbTransmogrifier assembly from your project in order to process migrations.  Currently DbTransmogrifier comes with a single, simple convention for discovering and applying your migrations.  Simply place the DbTransmogrifier executable in the same directory as the assembly which contains your migrations.

## Defining Migrations in Your Assembly

### Migration Interface and Attribute

DbTransmogrifier is designed so you do not have to take a dependency on the DbTransmogrifier assembly in your own migration definition assembly. DbTransmogrifier will scan your assembly to discover migrations that match particular signatures. The simplest way to wire things up is to create a `Migration.cs` file in assembly where you will build your migrations and add the following code to the file:

```csharp
    public interface IMigration
    {
        IEnumerable<string> Up();
        IEnumerable<string> Down();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
    }

    public abstract class Migration : IMigration
    {
        public abstract IEnumerable<string> Up();

        public virtual IEnumerable<string> Down()
        {
            yield break;
        }
    }
```

DbTransmogrifier looks in your assembly for any classes that implement the `IMigration` interface and are decorated with the `MigrationAttribute`

See the *SampleMigrations* project for a detailed example.

## Command line options

DbTransmogrifier supports the following command line options:

### Database level commands

* ```--init``` :: Creates the target database if it does not exist. Creates the "SchemaVersion" table if it does not exist.
* ```--tear-down``` :: Deletes all database Tables, Constraints, Views, Functions and Stored Procedures. Resets "SchemaVersion" table to version 0 (zero).  Basically restores the database to its initialized state.
* ```--drop``` :: Drops the database. No warnings, no redo, no cancel.  Be careful! You've been warned.

### Migration commands

* ```--current-version``` :: Displays the current schema version number.
* ```--up-to-latest``` :: Applies all up migrations after the current version up to the maximum version available.
* ```--up-to={version}``` :: Applies all up migrations after the current version up to the version specified.
* ```--down-to={version}``` :: Applies all down migrations from the current version down to the version specified.

### Other commands

* ```--help``` :: Displays command line help. Basically just a dump of available command line options to help jog your memory if you forget them.

## Advanced Options

DbTransmogrifier allows for the injection of ```IDbConnection``` and ```IDbTransaction``` so you can create migrations that query your database. You can choose between constructor or setter injection.  See the *SampleMigrations* project for example implementations.

This functionality is implemented by the ```DefaultMigrationBuilder```. If you want to create your own custom implementation of ```IMigrationBuilder``` you'll have to do your own dependency injection.  See the next section for configuration options.

## Configuration

### Default Configuration

By default, DbTransmogrifer will use your app.config file to load up the following settings:

* Database Provider
* Master Connection String
* Target Connection String

Example:

```xml
  <appSettings>
    <add key="ProviderInvariantName" value="System.Data.SqlClient"/>
  </appSettings>
  <connectionStrings>
    <clear/>
      <add name="Target" connectionString="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=YOUR_TARGET_DATABASE_NAME;Data Source=.\SQLEXPRESS"/>
      <add name="Master" connectionString="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=.\SQLEXPRESS"/>
  </connectionStrings>
```

### Overriding the Default Configuration

In order to override the defaults, you'll need to reference the DbTransmogrifier assembly in a project of your own creation (for example, a Console app).  The ```MigrationConfigurer``` class provides the following extensibility points for you to provide your own configuration options:

* ```ProviderNameSource```: A function that returns the name of the database provider you will be using (```System.Data.SqlClient``` is the default).
* ```MasterConnectionStringSource```: A function that returns a valid connection to your database server. This connection should *not* reference your target database (the value from app.config is the default).
* ```TargetConnectionStringSource```: A function that returns as valid connection to your target database (the value from app.config is the default).
* ```MigrationSourceFactory```: A function that returns an implementation of ```IMigrationSource``` (```DefaultMigrationTypeSource``` is the default).
* ```MigrationBuilderFactory```: A function that processes the results returned from your ```IMigrationTypeSource``` and can build each migration, injecting any necessary dependencies into them (```DefaultMigrationBuilder``` is the default).

Example:

```csharp
    class Program
    {
        static void Main(string[] args)
        {
            MigrationConfigurer.ProviderNameSource = () => "System.Data.SqlClient";
            MigrationConfigurer.MasterConnectionStringSource = () => "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=.";
            MigrationConfigurer.TargetConnectionStringSource = () => "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=YOUR_TARGET_DATABASE_NAME;Data Source=.";

            var transmogrifier = MigrationConfigurer
               .Configure()
               .WithDefaultMigrationBuilderFactory()
               .WithDefaultMigrationSourceFactory()
               .BuildTransmogrifier();

            var processor = new Processor(transmogrifier, args);
            processor.Process();
        }
    }
```

## Possible Plans for the Future

* Support other RDBMS's (Oracle, SQL CE, Firebird, MySql, etc.)
* Allow for alternative migration discovery conventions (file system based migrations, alternative assembly scanning options, etc.)
* Add support for dependency injection so migrations can have their dependencies supplied to them by a container

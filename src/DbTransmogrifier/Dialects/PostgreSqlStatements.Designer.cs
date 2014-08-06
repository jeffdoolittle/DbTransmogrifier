﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbTransmogrifier.Dialects {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class PostgreSqlStatements {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PostgreSqlStatements() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DbTransmogrifier.Dialects.PostgreSqlStatements", typeof(PostgreSqlStatements).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE DATABASE &quot;{0}&quot;;.
        /// </summary>
        internal static string CreateDatabase {
            get {
                return ResourceManager.GetString("CreateDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE &quot;SchemaVersion&quot; (&quot;Version&quot; bigint NOT NULL PRIMARY KEY);
        ///INSERT INTO &quot;SchemaVersion&quot; VALUES(0);.
        /// </summary>
        internal static string CreateSchemaVersionTable {
            get {
                return ResourceManager.GetString("CreateSchemaVersionTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select max(&quot;Version&quot;) from &quot;SchemaVersion&quot;;.
        /// </summary>
        internal static string CurrentVersion {
            get {
                return ResourceManager.GetString("CurrentVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT cast(count(*) as bit) FROM pg_database WHERE datname = @0;.
        /// </summary>
        internal static string DatabaseExists {
            get {
                return ResourceManager.GetString("DatabaseExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM &quot;SchemaVersion&quot; WHERE &quot;Version&quot; = @0;.
        /// </summary>
        internal static string DeleteSchemaVersion {
            get {
                return ResourceManager.GetString("DeleteSchemaVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    pg_terminate_backend (pg_stat_activity.pid)
        ///FROM
        ///    pg_stat_activity
        ///WHERE
        ///    pg_stat_activity.datname = &quot;{0}&quot;;.
        /// </summary>
        internal static string DropAllConnections {
            get {
                return ResourceManager.GetString("DropAllConnections", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP DATABASE &quot;{0}&quot;;.
        /// </summary>
        internal static string DropDatabase {
            get {
                return ResourceManager.GetString("DropDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE &quot;SchemaVersion&quot;;.
        /// </summary>
        internal static string DropSchemaVersionTable {
            get {
                return ResourceManager.GetString("DropSchemaVersionTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO &quot;SchemaVersion&quot; VALUES (@0);.
        /// </summary>
        internal static string InsertSchemaVersion {
            get {
                return ResourceManager.GetString("InsertSchemaVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT cast(count(*) as bit) FROM information_schema.tables WHERE table_name = &apos;SchemaVersion&apos;;.
        /// </summary>
        internal static string SchemaVersionTableExists {
            get {
                return ResourceManager.GetString("SchemaVersionTableExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP SCHEMA public CASCADE;
        ///CREATE SCHEMA public;
        ///CREATE TABLE &quot;SchemaVersion&quot; (&quot;Version&quot; bigint NOT NULL PRIMARY KEY);
        ///INSERT INTO &quot;SchemaVersion&quot; VALUES(0);.
        /// </summary>
        internal static string TearDown {
            get {
                return ResourceManager.GetString("TearDown", resourceCulture);
            }
        }
    }
}

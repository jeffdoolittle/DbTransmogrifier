﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
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
    internal class MsSqlStatements {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MsSqlStatements() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DbTransmogrifier.Dialects.MsSqlStatements", typeof(MsSqlStatements).Assembly);
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
        ///   Looks up a localized string similar to CREATE DATABASE {0};.
        /// </summary>
        internal static string CreateDatabase {
            get {
                return ResourceManager.GetString("CreateDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [SchemaVersion] ( [Version] bigint NOT NULL PRIMARY KEY );
        ///INSERT INTO [SchemaVersion] VALUES(0);.
        /// </summary>
        internal static string CreateSchemaVersionTable {
            get {
                return ResourceManager.GetString("CreateSchemaVersionTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select max([Version]) from [SchemaVersion];.
        /// </summary>
        internal static string CurrentVersion {
            get {
                return ResourceManager.GetString("CurrentVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT cast(count(*) as bit) FROM sys.databases WHERE [name] = @0;.
        /// </summary>
        internal static string DatabaseExists {
            get {
                return ResourceManager.GetString("DatabaseExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [SchemaVersion] WHERE [Version] = @0;.
        /// </summary>
        internal static string DeleteSchemaVersion {
            get {
                return ResourceManager.GetString("DeleteSchemaVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP DATABASE {0};.
        /// </summary>
        internal static string DropDatabase {
            get {
                return ResourceManager.GetString("DropDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE [SchemaVersion];.
        /// </summary>
        internal static string DropSchemaVersionTable {
            get {
                return ResourceManager.GetString("DropSchemaVersionTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO [SchemaVersion] VALUES (@0);.
        /// </summary>
        internal static string InsertSchemaVersion {
            get {
                return ResourceManager.GetString("InsertSchemaVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT cast(count(*) as bit) FROM sys.tables WHERE [name] = &apos;SchemaVersion&apos;;.
        /// </summary>
        internal static string SchemaVersionTableExists {
            get {
                return ResourceManager.GetString("SchemaVersionTableExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @name VARCHAR(128)
        ///DECLARE @constraint VARCHAR(254)
        ///DECLARE @SqL VARCHAR(254)
        ///
        ////* Drop all non-system stored procs */
        ///
        ///SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = &apos;P&apos; AND category = 0 ORDER BY [name])
        ///
        ///WHILE @name is not null
        ///BEGIN
        ///SELECT @SqL = &apos;DROP PROCEDURE [dbo].[&apos; + RTRIM(@name) +&apos;]&apos;
        ///EXEC (@SqL)
        ///PRINT &apos;Dropped Procedure: &apos; + @name
        ///SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = &apos;P&apos; AND category = 0 AND [name] &gt; @name ORDER BY [name])
        ///END        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TearDown {
            get {
                return ResourceManager.GetString("TearDown", resourceCulture);
            }
        }
    }
}

﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Config;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.History;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Data.Entity.Migrations.Sql;
    using System.Data.Entity.Resources;
    using System.Data.Entity.Utilities;
    using System.Reflection;

    /// <summary>
    ///     Configuration relating to the use of migrations for a given model.
    ///     You will typically create a configuration class that derives
    ///     from <see cref="DbMigrationsConfiguration{TContext}" /> rather than
    ///     using this class.
    /// </summary>
    public class DbMigrationsConfiguration
    {
        public const string DefaultMigrationsDirectory = "Migrations";

        private readonly Dictionary<string, MigrationSqlGenerator> _sqlGenerators
            = new Dictionary<string, MigrationSqlGenerator>();

        private MigrationCodeGenerator _codeGenerator;
        private Type _contextType;
        private Assembly _migrationsAssembly;
        private EdmModelDiffer _modelDiffer = new EdmModelDiffer();
        private DbConnectionInfo _connectionInfo;
        private string _migrationsDirectory = DefaultMigrationsDirectory;
        private readonly Lazy<IDbDependencyResolver> _resolver;
        private IHistoryContextFactory _historyContextFactory;
        private string _contextKey;

        /// <summary>
        ///     Initializes a new instance of the DbMigrationsConfiguration class.
        /// </summary>
        public DbMigrationsConfiguration()
            : this(new Lazy<IDbDependencyResolver>(() => DbConfiguration.DependencyResolver))
        {
            CodeGenerator = new CSharpMigrationCodeGenerator();
            ContextKey = GetType().ToString();
        }

        internal DbMigrationsConfiguration(Lazy<IDbDependencyResolver> resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        ///     Gets or sets a value indicating if automatic migrations can be used when migration the database.
        /// </summary>
        public bool AutomaticMigrationsEnabled { get; set; }

        public string ContextKey
        {
            get { return _contextKey; }
            set
            {
                Check.NotEmpty(value, "value");

                _contextKey = value.RestrictTo(HistoryContext.ContextKeyMaxLength);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating if data loss is acceptable during automatic migration.
        ///     If set to false an exception will be thrown if data loss may occur as part of an automatic migration.
        /// </summary>
        public bool AutomaticMigrationDataLossAllowed { get; set; }

        /// <summary>
        ///     Adds a new SQL generator to be used for a given database provider.
        /// </summary>
        /// <param name="providerInvariantName"> Name of the database provider to set the SQL generator for. </param>
        /// <param name="migrationSqlGenerator"> The SQL generator to be used. </param>
        public void SetSqlGenerator(string providerInvariantName, MigrationSqlGenerator migrationSqlGenerator)
        {
            Check.NotEmpty(providerInvariantName, "providerInvariantName");
            Check.NotNull(migrationSqlGenerator, "migrationSqlGenerator");

            _sqlGenerators[providerInvariantName] = migrationSqlGenerator;
        }

        /// <summary>
        ///     Gets the SQL generator that is set to be used with a given database provider.
        /// </summary>
        /// <param name="providerInvariantName"> Name of the database provider to get the SQL generator for. </param>
        /// <returns> The SQL generator that is set for the database provider. </returns>
        public MigrationSqlGenerator GetSqlGenerator(string providerInvariantName)
        {
            Check.NotEmpty(providerInvariantName, "providerInvariantName");

            MigrationSqlGenerator migrationSqlGenerator;

            if (!_sqlGenerators.TryGetValue(providerInvariantName, out migrationSqlGenerator))
            {
                migrationSqlGenerator
                    = _resolver.Value.GetService<MigrationSqlGenerator>(providerInvariantName);

                if (migrationSqlGenerator == null)
                {
                    throw Error.NoSqlGeneratorForProvider(providerInvariantName);
                }
            }

            return migrationSqlGenerator;
        }

        /// <summary>
        ///     Gets or sets the derived DbContext representing the model to be migrated.
        /// </summary>
        public Type ContextType
        {
            get { return _contextType; }
            set
            {
                Check.NotNull(value, "value");

                if (!typeof(DbContext).IsAssignableFrom(value))
                {
                    throw new ArgumentException(Strings.DbMigrationsConfiguration_ContextType(value.Name));
                }

                _contextType = value;

                DbConfigurationManager.Instance.EnsureLoadedForContext(_contextType);
            }
        }

        /// <summary>
        ///     Gets or sets the namespace used for code-based migrations.
        /// </summary>
        public string MigrationsNamespace { get; set; }

        /// <summary>
        ///     Gets or sets the sub-directory that code-based migrations are stored in.
        /// </summary>
        public string MigrationsDirectory
        {
            get { return _migrationsDirectory; }
            set
            {
                Check.NotEmpty(value, "value");

                _migrationsDirectory = value;
            }
        }

        /// <summary>
        ///     Gets or sets the code generator to be used when scaffolding migrations.
        /// </summary>
        public MigrationCodeGenerator CodeGenerator
        {
            get { return _codeGenerator; }
            set
            {
                Check.NotNull(value, "value");

                _codeGenerator = value;
            }
        }

        public IHistoryContextFactory HistoryContextFactory
        {
            get
            {
                return _historyContextFactory
                       ?? _resolver.Value.GetService<IHistoryContextFactory>(GetType())
                       ?? _resolver.Value.GetService<IHistoryContextFactory>();
            }
            set { _historyContextFactory = value; }
        }

        /// <summary>
        ///     Gets or sets the assembly containing code-based migrations.
        /// </summary>
        public Assembly MigrationsAssembly
        {
            get { return _migrationsAssembly; }
            set
            {
                Check.NotNull(value, "value");

                _migrationsAssembly = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value to override the connection of the database to be migrated.
        /// </summary>
        public DbConnectionInfo TargetDatabase
        {
            get { return _connectionInfo; }
            set
            {
                Check.NotNull(value, "value");

                _connectionInfo = value;
            }
        }

        /// <summary>
        ///     Gets or sets the timeout value used for the individual commands within a
        ///     migration. A null value indicates that the default value of the underlying
        ///     provider will be used.
        /// </summary>
        public int? CommandTimeout { get; set; }

        internal virtual void OnSeed(DbContext context)
        {
            DebugCheck.NotNull(context);
        }

        internal EdmModelDiffer ModelDiffer
        {
            get { return _modelDiffer; }
            set
            {
                DebugCheck.NotNull(value);

                _modelDiffer = value;
            }
        }
    }
}

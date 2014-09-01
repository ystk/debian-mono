﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ConnectionFactoryConfig
{
    using System.Data.Entity.Utilities;
    using System.ServiceProcess;
    using System.Xml.Linq;
    using EnvDTE;
    using Microsoft.Win32;

    /// <summary>
    ///     Handles detection of SQL Server Express or LocalDB on the current machine and adding entries
    ///     to .config files of a Visual Studio project to configure the default connection factory acordingly.
    /// </summary>
    public class ConnectionFactoryConfigurator
    {
        /// <summary>
        ///     Causes detection of SQL Server Express and/or LocalDB on the current machine and modifies
        ///     all "app.config" and "web.config" items in the given project to have an "defaultConnectionFactory"
        ///     entry with an appropriate base connection string. If the .config file already contains a
        ///     "defaultConnectionFactory" entry then no change is made.
        /// </summary>
        /// <remarks>
        ///     This code is usually invoked on installation of the Entity Framework nuget package into a project.
        ///     If SQL Express (2005, 2008, or 2012) is found, then SQL Express will be configured.
        ///     Otherwise, if a particular version of LocalDB is found, then the connection string will use that version. If
        ///     multiple versions are found then an attempt to use the highest version will be made. If no version
        ///     of SQL Express or LocalDB is found, then LocalDB v11.0 (SQL Server 2012) will be used.
        /// </remarks>
        /// <param name="project"> The Visual Studio project to configure. </param>
        [CLSCompliant(false)]
        public ConnectionFactoryConfigurator(Project project)
        {
            Check.NotNull(project, "project");

            using (
                var detector = new SqlServerDetector(
                    Registry.LocalMachine, new ServiceControllerProxy(new ServiceController("MSSQL$SQLEXPRESS"))))
            {
                var factorySpecification = detector.BuildConnectionFactorySpecification();
                var manipulator = new ConfigFileManipulator();
                var processor = new ConfigFileProcessor();

                new ConfigFileFinder().FindConfigFiles(
                    project.ProjectItems,
                    i => processor.ProcessConfigFile(
                        i, new Func<XDocument, bool>[]
                               {
                                   c => manipulator.AddOrUpdateConfigSection(c, GetType().Assembly.GetName().Version),
                                   c => manipulator.AddConnectionFactoryToConfig(c, factorySpecification)
                               }));
            }
        }
    }
}

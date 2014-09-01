﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration.Functions
{
    using System.Data.Entity.Migrations;
    using Xunit;

    public class ModificationFunctionsConfigurationTTests
    {
        [Fact]
        public void InsertFunction_when_config_action_should_call_method_on_internal_configuration()
        {
            var modificationFunctionsConfiguration = new ModificationFunctionsConfiguration<Order>();

            ModificationFunctionConfiguration<Order> configuration = null;

            modificationFunctionsConfiguration.InsertFunction(c => { configuration = c; });

            Assert.Same(
                configuration.Configuration,
                modificationFunctionsConfiguration.Configuration.InsertModificationFunctionConfiguration);
        }

        [Fact]
        public void UpdateFunction_when_config_action_should_call_method_on_internal_configuration()
        {
            var modificationFunctionsConfiguration = new ModificationFunctionsConfiguration<Order>();

            ModificationFunctionConfiguration<Order> configuration = null;

            modificationFunctionsConfiguration.UpdateFunction(c => { configuration = c; });

            Assert.Same(
                configuration.Configuration,
                modificationFunctionsConfiguration.Configuration.UpdateModificationFunctionConfiguration);
        }

        [Fact]
        public void DeleteFunction_when_config_action_should_call_method_on_internal_configuration()
        {
            var modificationFunctionsConfiguration = new ModificationFunctionsConfiguration<Order>();

            ModificationFunctionConfiguration<Order> configuration = null;

            modificationFunctionsConfiguration.DeleteFunction(c => { configuration = c; });

            Assert.Same(
                configuration.Configuration,
                modificationFunctionsConfiguration.Configuration.DeleteModificationFunctionConfiguration);
        }
    }
}

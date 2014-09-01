﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Mapping
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Utilities;

    /// <summary>
    ///     Represents a mapping from a model function import to a store composable or non-composable function.
    /// </summary>
    internal abstract class FunctionImportMapping
    {
        internal FunctionImportMapping(EdmFunction functionImport, EdmFunction targetFunction)
        {
            DebugCheck.NotNull(functionImport);
            DebugCheck.NotNull(targetFunction);

            FunctionImport = functionImport;
            TargetFunction = targetFunction;
        }

        /// <summary>
        ///     Gets model function (or source of the mapping)
        /// </summary>
        internal readonly EdmFunction FunctionImport;

        /// <summary>
        ///     Gets store function (or target of the mapping)
        /// </summary>
        internal readonly EdmFunction TargetFunction;
    }
}

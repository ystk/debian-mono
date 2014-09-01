﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.


#if !NET40

namespace System.Data.Entity.Core.Objects.DataClasses
{
    using System.Data.Entity.Utilities;
    using System.Threading;
    using System.Threading.Tasks;

    public static class IRelatedEndExtensions
    {
        /// <summary>
        ///     An asynchronous version of Load, which
        ///     loads the related entity or entities into the related end using the default merge option.
        /// </summary>
        /// <returns> A task representing the asynchronous operation. </returns>
        public static Task LoadAsync(this IRelatedEnd relatedEnd)
        {
            Check.NotNull(relatedEnd, "relatedEnd");

            return relatedEnd.LoadAsync(CancellationToken.None);
        }

        /// <summary>
        ///     An asynchronous version of Load, which
        ///     loads the related entity or entities into the related end using the specified merge option.
        /// </summary>
        /// <param name="mergeOption"> Merge option to use for loaded entity or entities. </param>
        /// <returns> A task representing the asynchronous operation. </returns>
        public static Task LoadAsync(this IRelatedEnd relatedEnd, MergeOption mergeOption)
        {
            Check.NotNull(relatedEnd, "relatedEnd");

            return relatedEnd.LoadAsync(mergeOption, CancellationToken.None);
        }
    }
}

#endif

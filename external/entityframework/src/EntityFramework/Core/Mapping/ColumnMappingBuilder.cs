// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Mapping
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Utilities;

    internal class ColumnMappingBuilder
    {
        private EdmProperty _columnProperty;
        private readonly IList<EdmProperty> _propertyPath;
        private StorageScalarPropertyMapping _scalarPropertyMapping;

        public ColumnMappingBuilder(EdmProperty columnProperty, IList<EdmProperty> propertyPath)
        {
            DebugCheck.NotNull(columnProperty);
            DebugCheck.NotNull(propertyPath);

            _columnProperty = columnProperty;
            _propertyPath = propertyPath;
        }

        public IList<EdmProperty> PropertyPath
        {
            get { return _propertyPath; }
        }

        public EdmProperty ColumnProperty
        {
            get { return _columnProperty; }
            set
            {
                DebugCheck.NotNull(value);

                _columnProperty = value;

                if (_scalarPropertyMapping != null)
                {
                    _scalarPropertyMapping.ColumnProperty = _columnProperty;
                }
            }
        }

        public void SetTarget(StorageScalarPropertyMapping scalarPropertyMapping)
        {
            _scalarPropertyMapping = scalarPropertyMapping;
        }
    }
}

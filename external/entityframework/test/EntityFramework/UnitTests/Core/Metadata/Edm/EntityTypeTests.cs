﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Metadata.Edm
{
    using System.Linq;
    using Moq;
    using Xunit;

    public class EntityTypeTests
    {
        [Fact]
        public void Properties_collection_is_live_until_entity_goes_readonly()
        {
            var entityType = new EntityType();

            Assert.False(entityType.IsReadOnly);
            Assert.NotSame(entityType.Properties, entityType.Properties);

            entityType.SetReadOnly();

            Assert.Same(entityType.Properties, entityType.Properties);
        }

        [Fact]
        public void Can_add_and_remove_foreign_key_builders()
        {
            var entityType = new EntityType();
            var mockForeignKeyBuilder = new Mock<ForeignKeyBuilder>();

            entityType.AddForeignKey(mockForeignKeyBuilder.Object);

            Assert.Same(mockForeignKeyBuilder.Object, entityType.ForeignKeyBuilders.Single());

            mockForeignKeyBuilder.Verify(fk => fk.SetOwner(entityType));

            entityType.RemoveForeignKey(mockForeignKeyBuilder.Object);

            mockForeignKeyBuilder.Verify(fk => fk.SetOwner(null));
        }

        [Fact]
        public void Can_get_list_of_declared_key_properties()
        {
            var entityType = new EntityType();

            Assert.Empty(entityType.DeclaredKeyProperties);

            var property = EdmProperty.Primitive("P", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String));

            entityType.AddKeyMember(property);

            Assert.Equal(1, entityType.DeclaredKeyProperties.Count);

            entityType.RemoveMember(property);

            var baseType = new EntityType();
            baseType.AddKeyMember(property);

            entityType.BaseType = baseType;

            Assert.Empty(entityType.DeclaredKeyProperties);
            Assert.Equal(1, entityType.KeyMembers.Count);
        }

        [Fact]
        public void Can_get_list_of_declared_navigation_properties()
        {
            var entityType = new EntityType();

            Assert.Empty(entityType.DeclaredNavigationProperties);

            var property = new NavigationProperty("N", TypeUsage.Create(new EntityType()));

            entityType.AddMember(property);

            Assert.Equal(1, entityType.DeclaredNavigationProperties.Count);

            entityType.RemoveMember(property);

            var baseType = new EntityType();
            baseType.AddMember(property);

            entityType.BaseType = baseType;

            Assert.Empty(entityType.DeclaredNavigationProperties);
            Assert.Equal(1, entityType.Members.Count);
        }

        [Fact]
        public void Can_get_list_of_declared_properties()
        {
            var entityType = new EntityType();

            Assert.Empty(entityType.DeclaredProperties);

            var property = EdmProperty.Primitive("P", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String));

            entityType.AddMember(property);

            Assert.Equal(1, entityType.DeclaredProperties.Count);

            entityType.RemoveMember(property);

            var baseType = new EntityType();
            baseType.AddMember(property);

            entityType.BaseType = baseType;

            Assert.Empty(entityType.DeclaredProperties);
            Assert.Equal(1, entityType.Members.Count);
        }

        [Fact]
        public void Can_get_list_of_declared_members()
        {
            var entityType = new EntityType();

            Assert.Empty(entityType.DeclaredMembers);

            var property1 = new NavigationProperty("N", TypeUsage.Create(new EntityType()));
            var property2 = EdmProperty.Primitive("P", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String));

            entityType.AddMember(property1);
            entityType.AddMember(property2);

            Assert.Equal(2, entityType.DeclaredMembers.Count);

            entityType.RemoveMember(property1);
            entityType.RemoveMember(property2);

            var baseType = new EntityType();
            baseType.AddMember(property1);
            baseType.AddMember(property2);

            entityType.BaseType = baseType;

            Assert.Empty(entityType.DeclaredMembers);
            Assert.Equal(2, entityType.Members.Count);
        }

        [Fact]
        public void Properties_list_should_be_live_on_reread()
        {
            var entityType = new EntityType();

            Assert.Empty(entityType.Properties);

            var property = EdmProperty.Primitive("P", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String));

            entityType.AddMember(property);

            Assert.Equal(1, entityType.Properties.Count);
        }
    }
}

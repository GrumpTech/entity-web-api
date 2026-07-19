using EntityWebApi.Core.Attributes;
using EntityWebApi.Core.Enums;
using EntityWebApi.Dtos.Internal.Dtos;
using EntityWebApi.Dtos.Internal.Extensions;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace EntityWebApi.Dtos.Internal.Extensions
{
    public static class TypeBuilderExtensions
    {
        public static Type CreateType(this TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo()!.AsType();
        }

        public static IEnumerable<PropertyBuilder> AddProperties(this TypeBuilder typeBuilder, IEnumerable<PropertyConfiguration> properties)
        {
            return properties.Select(typeBuilder.AddProperty).ToList();
        }

        public static PropertyBuilder AddProperty(this TypeBuilder typeBuilder, PropertyConfiguration property)
        {
            var name = property.Name;
            var propertyType = property.Type;
            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, propertyType, null);
            var fieldBuilder = typeBuilder.AddPrivateField($"<{name}>_backing_field", propertyType);
            var access = property.Access;
            typeBuilder.AddGetter(propertyBuilder, fieldBuilder, GetMethodAttributes(access.HasFlag(PropertyAccess.Read)));
            var writeMethodAttributes = GetMethodAttributes(access.HasFlag(PropertyAccess.Write));
            if (property.Optional)
            {
                typeBuilder.AddPatchSetter(propertyBuilder, fieldBuilder, writeMethodAttributes);
            }
            else
            {
                typeBuilder.AddSetter(propertyBuilder, fieldBuilder, writeMethodAttributes);
            }
            return propertyBuilder.AddAttributes(property.Attributes ?? Array.Empty<CustomAttributeData>());
        }

        public static FieldBuilder AddPrivateField(this TypeBuilder typeBuilder, string name, Type type)
        {
            var fieldBuilder = typeBuilder.DefineField(name, type, FieldAttributes.Private);
            var constructorInfo = typeof(DebuggerBrowsableAttribute).GetConstructor(new[] { typeof(DebuggerBrowsableState) });
            fieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructorInfo!, new object[] { DebuggerBrowsableState.Never }));
            return fieldBuilder;
        }


        private static MethodAttributes GetMethodAttributes(bool isPublic = true)
        {
            return (isPublic ? MethodAttributes.Public : MethodAttributes.Private) | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
        }

        private static PropertyBuilder AddGetter(this TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder, MethodAttributes methodAttributes)
        {
            var getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyBuilder.Name}", methodAttributes, propertyBuilder.PropertyType, null);
            var getMethodGenerator = getMethodBuilder.GetILGenerator();
            getMethodGenerator.Emit(OpCodes.Ldarg_0);
            getMethodGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getMethodGenerator.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getMethodBuilder);
            return propertyBuilder;
        }

        private static PropertyBuilder AddSetter(this TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder, MethodAttributes methodAttributes)
        {
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyBuilder.Name}", methodAttributes, typeof(void), new Type[] { propertyBuilder.PropertyType });
            var setMethodGenerator = setMethodBuilder.GetILGenerator();
            setMethodGenerator.Emit(OpCodes.Ldarg_0);
            setMethodGenerator.Emit(OpCodes.Ldarg_1);
            setMethodGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            setMethodGenerator.Emit(OpCodes.Ret);
            propertyBuilder.SetSetMethod(setMethodBuilder);
            return propertyBuilder;
        }

        private static PropertyBuilder AddPatchSetter(this TypeBuilder typeBuilder, PropertyBuilder propertyBuilder, FieldBuilder fieldBuilder, MethodAttributes methodAttributes)
        {
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyBuilder.Name}", methodAttributes, typeof(void), new Type[] { propertyBuilder.PropertyType });
            var hasValueFieldBuilder = typeBuilder.AddPrivateField($"<{propertyBuilder.Name}>_has_value_field", typeof(bool));
            AddHasValueFieldAttribute(propertyBuilder, hasValueFieldBuilder.Name);
            var setMethodGenerator = setMethodBuilder.GetILGenerator();
            setMethodGenerator.Emit(OpCodes.Ldarg_0);
            setMethodGenerator.Emit(OpCodes.Ldarg_1);
            setMethodGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            setMethodGenerator.Emit(OpCodes.Ldarg_0);
            setMethodGenerator.Emit(OpCodes.Ldc_I4_1);
            setMethodGenerator.Emit(OpCodes.Stfld, hasValueFieldBuilder);
            setMethodGenerator.Emit(OpCodes.Ret);
            propertyBuilder.SetSetMethod(setMethodBuilder);
            return propertyBuilder;
        }

        private static void AddHasValueFieldAttribute(PropertyBuilder propertyBuilder, string fieldName)
        {
            var constructorInfo = typeof(HasValueFieldAttribute).GetConstructor(new Type[] { typeof(string) });
            propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructorInfo!, new object[] { fieldName }));
        }
    }
}
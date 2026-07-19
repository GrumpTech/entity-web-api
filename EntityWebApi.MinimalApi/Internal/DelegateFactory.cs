using EntityWebApi.MinimalApi.Internal.BuilderExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace EntityWebApi.MinimalApi.Internal
{
    public class DelegateFactory
    {
        private readonly static Lazy<ModuleBuilder> _lazyModuleBuilder = new(InitModuleBuilder);
        private static ModuleBuilder ModuleBuilder => _lazyModuleBuilder.Value;
        private static readonly Dictionary<int, Type> _delegateTypes = new()
        {
            { 1, typeof(Func<,>) },
            { 2, typeof(Func<,,>) },
            { 3, typeof(Func<,,,>) },
            { 4, typeof(Func<,,,,>) },
            { 5, typeof(Func<,,,,,>) },
            { 6, typeof(Func<,,,,,,>) }
        };
        private const string parameterNameHandler = "<eh>";

        public Delegate CreateDelegate(MethodInfo method, IEnumerable<CustomAttributeData>? attributes = null)
        {
            var methodName = method.Name;
            if (method?.DeclaringType == null || method.IsStatic)
            {
                throw new ArgumentException($"EndpointHandlers: Method {methodName} is null, static, or has no containing class");
            }
            var type = method.DeclaringType;
            if (type.IsInterface || (type.IsGenericType && !type.IsConstructedGenericType))
            {
                throw new ArgumentException($"EndpointHandlers: Expected {type.FullName} to be non-generic class");
            }
            var parameters = method.GetParameters();
            var typeBuilder = ModuleBuilder.DefineType($"{type}Extension");

            var parameterTypes = parameters.Select(p => p.ParameterType).Append(type).ToArray();
            var methodBuilder = typeBuilder.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Static,
                method.ReturnType, parameterTypes);
            methodBuilder.AddAttributes(attributes ?? Array.Empty<CustomAttributeData>());
            methodBuilder.AddParameters(parameters);
            var parameterBuilder = methodBuilder.AddParameter(parameterNameHandler, ParameterAttributes.In, parameterTypes.Length);
            AddFromServicesAttribute(parameterBuilder);

            var methodGenerator = methodBuilder.GetILGenerator();
            methodGenerator.Emit(OpCodes.Ldarg_S, parameterBuilder.Position - 1);
            for (int i = 0, l = parameters.Length; i < l; i++)
            {
                methodGenerator.Emit(OpCodes.Ldarg_S, i);
            }
            methodGenerator.Emit(OpCodes.Callvirt, method);
            methodGenerator.Emit(OpCodes.Ret);

            var delegateType = GetDelegateType(parameterTypes, method.ReturnType) ??
                throw new InvalidOperationException($"EndpointHandlers: maximum number of supported parameters '{parameters.Length}' exceeded");
            return Delegate.CreateDelegate(delegateType, null, typeBuilder.CreateType().GetMethod(methodName)!);
        }


        private static Type? GetDelegateType(Type[] parameters, Type resultType)
        {
            return _delegateTypes.TryGetValue(parameters.Length, out var type) ?
                type.MakeGenericType(parameters.Append(resultType).ToArray()) : null;
        }

        private static void AddFromServicesAttribute(ParameterBuilder parameterBuilder)
        {
            var constructorInfo = typeof(FromServicesAttribute).GetConstructor(Array.Empty<Type>());
            var attributeBuilder = new CustomAttributeBuilder(constructorInfo!, Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attributeBuilder);
        }

        private static ModuleBuilder InitModuleBuilder()
        {
            var assemblyName = "EndpointHandlers.Types.Dynamic";
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            return assemblyBuilder.DefineDynamicModule(assemblyName);
        }
    }
}

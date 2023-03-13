using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class AssemblyInfoProviders {

	public const string UNITY_MAIN_ASSEMBLY_NAME = "Assembly-CSharp";
	
	public static Assembly GetMainUnityAssembly {
		get {
			try {
				return Assembly.Load(UNITY_MAIN_ASSEMBLY_NAME);
			}
			catch (Exception e) {
				return GetAllAssemblies.First();
			}
		}
	}

	public static IEnumerable<Assembly> GetAllAssemblies => AppDomain.CurrentDomain.GetAssemblies();
	
	public static IEnumerable<Assembly> GetRelevantAssemblies {
		get {
			var assemblies = GetMainUnityAssembly.GetReferencedAssemblies().Select(Assembly.Load)
			                                     .Append(GetMainUnityAssembly).ToList();

			var myAsm = typeof(AssemblyInfoProviders).Assembly;

			if (!assemblies.Contains(myAsm)) {
				assemblies.Add(myAsm);
			}
			
			return assemblies;
		}
	}

	public static IEnumerable<Type> GetTypes(IEnumerable<Assembly> fromAssemblies)
		=> fromAssemblies.SelectMany(asm => asm.GetTypes(), (_, type) => type);

	public static IEnumerable<Type> GetTypesWithAttribute(IEnumerable<Assembly> fromAssemblies, Type attributeType) {
		return GetTypes(fromAssemblies).Where(type => Attribute.IsDefined(type, attributeType));
	}

	public static IEnumerable<MethodInfo> GetMethodsWithAttribute(IEnumerable<Assembly> fromAssemblies, Type attributeType, BindingFlags methodsFlags = BindingFlags.Default) {
		return GetTypes(fromAssemblies).SelectMany(type => type.GetMethods(methodsFlags), (_, methodInfo) => methodInfo).Where(method => Attribute.IsDefined(method, attributeType));
	}
}
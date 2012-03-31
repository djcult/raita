using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Reflection;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Raita.StepDefinitionGenerator
{
    static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetFeatureTypes(this Assembly assembly)
        {
            var types = assembly.GetTypes();
            return types
                .Where(t => t.GetCustomAttributes(typeof(GeneratedCodeAttribute), false)
                    .Cast<GeneratedCodeAttribute>()
                    .Any(a => a.Tool == "TechTalk.SpecFlow")
                );
        }

        public static IEnumerable<MethodInfo> GetTestMethods(this Type type)
        {
            var methods = type.GetMethods();
            return methods.Where(m => m.GetCustomAttributes(typeof(TestAttribute), false)
                                          .Any());
        }

        public static IEnumerable<string> GetTestRunnerArguments(this MethodBase method)
        {
            var testRunnerMethodNames = new[] { "Given", "When", "Then", "And", "But" };

            var instructions = method.GetInstructions();

            return instructions
                .Where(i =>
                           {
                               var operand = i.Operand as MethodBase;
                               return operand != null && testRunnerMethodNames.Contains(operand.Name);
                           })
                .Select(instruction => instruction.Previous.Operand as string);
        }

        public static IEnumerable<Type> GetBindingTypes(this Assembly assembly)
        {
            var types = assembly.GetTypes();
            return types.Where(t => t
                .GetCustomAttributes(typeof(BindingAttribute), false)
                .Any());
        }

        public static IEnumerable<StepDefinitionBaseAttribute> GetStepDefinitionAttributes(this Type type)
        {
            return type.GetMethods()
                .SelectMany(m => m.GetCustomAttributes(typeof(StepDefinitionBaseAttribute), false))
                .Cast<StepDefinitionBaseAttribute>();
        }
    }
}
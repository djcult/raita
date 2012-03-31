using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Raita.StepDefinitionGenerator
{
    public class StepDefinitionsFactory
    {
        public static IEnumerable<StepDefinition> CreateFromAssembly(Assembly assembly)
        {
            var featureSteps = assembly.GetFeatureTypes()
                .SelectMany(t => t.GetTestMethods())
                .SelectMany(m => ReflectionExtensions.GetTestRunnerArguments(m));

            var bindingSteps = assembly.GetBindingTypes()
                .SelectMany(t => t.GetStepDefinitionAttributes())
                .Select(a => a.Regex);

            var stepDefs = 
                    (from bindingStep in bindingSteps
                    let regex = new Regex(bindingStep)
                    let matchingFeatureSteps = featureSteps.Where(s => regex.IsMatch(s))
                    select new StepDefinition(bindingStep, matchingFeatureSteps.ToArray()))
                .ToList();

            stepDefs.AddRange(
                featureSteps
                    .Except(stepDefs.SelectMany(s => s.Children))
                    .Select(s => new StepDefinition(s))
                );
            
            return stepDefs;
        }
    }
}
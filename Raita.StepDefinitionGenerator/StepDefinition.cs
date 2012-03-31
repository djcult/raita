using System.Collections.Generic;
using System.Text;

namespace Raita.StepDefinitionGenerator
{
    public class StepDefinition
    {
        public string Parent { get; private set; }
        public IEnumerable<string> Children { get; private set; }

        public StepDefinition(string foo)
        {
            Parent = foo;
            Children = new string[0];
        }

        public StepDefinition(string foo, params string[] bars)
        {
            Parent = foo;
            Children = bars;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Parent + "\n");
            foreach (var child in Children)
                sb.Append("\t" + child + "\n");
            return sb.ToString();
        }
    }
}
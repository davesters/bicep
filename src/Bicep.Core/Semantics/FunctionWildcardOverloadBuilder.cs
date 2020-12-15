// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

namespace Bicep.Core.Semantics
{
    public class FunctionWildcardOverloadBuilder : FunctionOverloadBuilder
    {
        private Regex WildcardRegex { get; }

        public FunctionWildcardOverloadBuilder(string name, Regex wildcardRegex) : base(name)
        {
            this.WildcardRegex = wildcardRegex;
        }

        public override FunctionOverload Build()
        {
            return new FunctionWildcardOverload(
                this.Name,
                this.WildcardRegex,
                this.ReturnTypeBuilder,
                this.ReturnType,
                this.MinimumArgumentCount,
                this.MaximumArgumentCount,
                this.FixedParameters.ToImmutable(),
                this.VariableParameter,
                this.Flags);
        }
    }
}
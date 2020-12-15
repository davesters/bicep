// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class FixedFunctionParameter
    {
        public FixedFunctionParameter(string name, TypeSymbol type, bool required)
        {
            this.Name = name;
            this.Type = type;
            this.Required = required;
        }

        public string Name { get; }

        public TypeSymbol Type { get; }

        public bool Required { get; }
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class FixedFunctionParameter
    {
        public FixedFunctionParameter(string name, TypeSymbol type, bool required, string? description)
        {
            this.Name = name;
            this.Type = type;
            this.Required = required;
            this.Description = description;
        }

        public string Name { get; }

        public string? Description { get; }

        public TypeSymbol Type { get; }

        public bool Required { get; }
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class VariableFunctionParameter
    {
        public VariableFunctionParameter(string namePrefix, TypeSymbol type, string? description)
        {
            this.NamePrefix = namePrefix;
            this.Type = type;
            this.Description = description;
        }

        public string NamePrefix { get; }

        public string? Description { get; }

        public TypeSymbol Type { get; }

        public string GetName(int index) => $"{this.NamePrefix}{index}";
    }
}
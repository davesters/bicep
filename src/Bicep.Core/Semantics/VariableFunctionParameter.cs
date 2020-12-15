// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class VariableFunctionParameter
    {
        public VariableFunctionParameter(string namePrefix, TypeSymbol type)
        {
            this.NamePrefix = namePrefix;
            this.Type = type;
        }

        public string NamePrefix { get; }

        public TypeSymbol Type { get; }

        public string GetName(int index) => $"{this.NamePrefix}{index}";
    }
}
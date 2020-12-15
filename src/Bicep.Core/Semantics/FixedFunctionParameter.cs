// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class FixedFunctionParameter
    {
        public FixedFunctionParameter(string name, TypeSymbol type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; }

        public TypeSymbol Type { get; }
    }
}

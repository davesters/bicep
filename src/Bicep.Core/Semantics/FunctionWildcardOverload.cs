// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class FunctionWildcardOverload : FunctionOverload
    {
        public FunctionWildcardOverload(string name, Regex wildcardRegex, ReturnTypeBuilderDelegate returnTypeBuilder, TypeSymbol returnType, int minimumArgumentCount, int? maximumArgumentCount, IEnumerable<FixedFunctionParameter> fixedArgumentTypes, VariableFunctionParameter? variableArgumentType, FunctionFlags flags = FunctionFlags.Default)
            : base(name, returnTypeBuilder, returnType, minimumArgumentCount, maximumArgumentCount, fixedArgumentTypes, variableArgumentType, flags)
        {
            WildcardRegex = wildcardRegex;
        }

        public Regex WildcardRegex { get; }
    }
}

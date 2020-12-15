// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public class FunctionOverloadBuilder
    {
        public FunctionOverloadBuilder(string name)
        {
            this.Name = name;
            this.ReturnType = LanguageConstants.Any;
            this.FixedParameters = ImmutableArray.CreateBuilder<FixedFunctionParameter>();
            this.ReturnTypeBuilder = args => LanguageConstants.Any;
            this.VariableParameter = null;
        }

        protected string Name { get; }

        protected string? Description { get; private set; }

        protected TypeSymbol ReturnType { get; private set; }

        protected ImmutableArray<FixedFunctionParameter>.Builder FixedParameters { get; }

        protected int MinimumArgumentCount { get; private set; }

        protected int? MaximumArgumentCount { get; private set; }

        protected VariableFunctionParameter? VariableParameter { get; private set; }

        protected FunctionOverload.ReturnTypeBuilderDelegate ReturnTypeBuilder { get; private set; }

        protected FunctionFlags Flags { get; private set; }

        public virtual FunctionOverload Build() =>
            new FunctionOverload(
                this.Name,
                this.ReturnTypeBuilder,
                this.ReturnType,
                this.MinimumArgumentCount,
                this.MaximumArgumentCount,
                this.FixedParameters.ToImmutable(),
                this.VariableParameter,
                this.Flags);

        public FunctionOverloadBuilder WithDescription(string description)
        {
            this.Description = description;

            return this;
        }

        public FunctionOverloadBuilder WithReturnType(TypeSymbol returnType)
        {
            this.ReturnType = returnType;
            this.ReturnTypeBuilder = args => returnType;

            return this;
        }

        public FunctionOverloadBuilder WithDynamicReturnType(FunctionOverload.ReturnTypeBuilderDelegate returnTypeBuilder)
        {
            this.ReturnType = returnTypeBuilder(Enumerable.Empty<FunctionArgumentSyntax>());
            this.ReturnTypeBuilder = returnTypeBuilder;

            return this;
        }

        public FunctionOverloadBuilder WithFixedParameters(params TypeSymbol[] parameterTypes) => 
            this.WithOptionalFixedParameters(parameterTypes.Length, parameterTypes);

        public FunctionOverloadBuilder WithOptionalFixedParameters(int minimumArgumentCount, params TypeSymbol[] parameterTypes)
        {
            this.FixedParameters.Clear();
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                this.FixedParameters.Add(CreateFixedParameter(parameterTypes[i], i, required: i < minimumArgumentCount));
            }
            
            this.MinimumArgumentCount = minimumArgumentCount;
            this.MaximumArgumentCount = parameterTypes.Length;

            return this;
        }

        public FunctionOverloadBuilder WithFixedParametersAndOptionalVariableParameters(TypeSymbol variableArgumentType, params TypeSymbol[] fixedArgumentTypes)
        {
            this.WithFixedParameters(fixedArgumentTypes);
            this.MaximumArgumentCount = null;
            this.VariableParameter = CreateVariableParameter(variableArgumentType);

            return this;
        }

        public FunctionOverloadBuilder WithVariableParameters(int minimumArgumentCount, TypeSymbol parameterType)
        {
            this.FixedParameters.Clear();
            for (int i = 0; i < minimumArgumentCount; i++)
            {
                this.FixedParameters.Add(CreateFixedParameter(parameterType, i, required: i < minimumArgumentCount));
            }

            this.MinimumArgumentCount = minimumArgumentCount;
            this.MaximumArgumentCount = null;
            this.VariableParameter = CreateVariableParameter(parameterType);

            return this;
        }

        public FunctionOverloadBuilder WithRequiredParameter(string name, TypeSymbol type, string description)
        {
            this.FixedParameters.Add(new FixedFunctionParameter(name, type, required: true, description));
            return this;
        }

        public FunctionOverloadBuilder WithOptionalParameter(string name, TypeSymbol type, string description)
        {
            this.FixedParameters.Add(new FixedFunctionParameter(name, type, required: false, description));
            return this;
        }

        public FunctionOverloadBuilder WithRequiredVariableParameter(string namePrefix, TypeSymbol type, string description)
        {
            // TODO:
            this.VariableParameter = new VariableFunctionParameter(namePrefix, type, description);
        }

        public FunctionOverloadBuilder WithOptionalVariableParameter(string namePrefix, TypeSymbol type, string description)
        {
            // TODO:
            this.VariableParameter = new VariableFunctionParameter(namePrefix, type, description);
        }

        public FunctionOverloadBuilder WithFlags(FunctionFlags flags)
        {
            this.Flags = flags;

            return this;
        }

        protected virtual void Validate()
        {
            // required must precede optional
            // can't have optional with varargs
        }

        protected VariableFunctionParameter CreateVariableParameter(TypeSymbol type) => new VariableFunctionParameter("vararg", type, description: null);

        protected FixedFunctionParameter CreateFixedParameter(TypeSymbol type, int index, bool required) => new FixedFunctionParameter($"arg{index}", type, required, description: null);
    }
}

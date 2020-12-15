// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics.Namespaces
{
    public class SystemNamespaceSymbol : NamespaceSymbol
    {
        private static readonly ImmutableArray<FunctionOverload> SystemOverloads = new[]
        {
            new FunctionOverloadBuilder("any")
                .WithReturnType(LanguageConstants.Any)
                .WithDescription("Converts the specified value to the `any` type.")
                .WithRequiredParameter("any", LanguageConstants.Any, "The value to convert to `any` type")
                .Build(),

            new FunctionOverloadBuilder("concat")
                .WithReturnType(LanguageConstants.Array)
                .WithDescription("Combines multiple arrays and returns the concatenated array.")
                .WithRequiredVariableParameter("arg", LanguageConstants.Array, "The array for concatenation")
                .Build(),

            new FunctionOverloadBuilder("concat")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Combines multiple string, integer, or boolean values and returns them as a concatenated string.")
                .WithRequiredVariableParameter("arg", UnionType.Create(LanguageConstants.String, LanguageConstants.Int, LanguageConstants.Bool), "The string, int, or boolean value for concatenation")
                .Build(),

            new FunctionOverloadBuilder("format")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Creates a formatted string from input values.")
                .WithRequiredParameter("formatString", LanguageConstants.String, "The composite format string.")
                .WithOptionalVariableParameter("arg", LanguageConstants.Any, "The value to include in the formatted string.")
                .Build(),

            new FunctionOverloadBuilder("base64")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Returns the base64 representation of the input string.")
                .WithRequiredParameter("inputString", LanguageConstants.String, "The value to return as a base64 representation.")
                .Build(),

            new FunctionOverloadBuilder("padLeft")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Returns a right-aligned string by adding characters to the left until reaching the total specified length.")
                .WithRequiredParameter("valueToPad", UnionType.Create(LanguageConstants.String, LanguageConstants.Int), "The value to right-align.")
                .WithRequiredParameter("totalLength", LanguageConstants.Int, "The total number of characters in the returned string.")
                .WithOptionalParameter("paddingCharacter", LanguageConstants.String, "The character to use for left-padding until the total length is reached. The default value is a space.")
                .Build(),

            new FunctionOverloadBuilder("replace")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Returns a new string with all instances of one string replaced by another string.")
                .WithRequiredParameter("originalString", LanguageConstants.String, "The original string.")
                .WithRequiredParameter("oldString", LanguageConstants.String, "The string to be removed from the original string.")
                .WithRequiredParameter("newString", LanguageConstants.String, "The string to add in place of the removed string.")
                .Build(),

            new FunctionOverloadBuilder("toLower")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Converts the specified string to lower case.")
                .WithRequiredParameter("stringToChange", LanguageConstants.String, "The value to convert to lower case.")
                .Build(),

            new FunctionOverloadBuilder("toUpper")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Converts the specified string to upper case.")
                .WithRequiredParameter("stringToChange", LanguageConstants.String, "The value to convert to upper case.")
                .Build(),

            new FunctionOverloadBuilder("length")
                .WithReturnType(LanguageConstants.Int)
                .WithDescription("Returns the number of characters in a string, elements in an array, or root-level properties in an object.")
                .WithRequiredParameter("arg", UnionType.Create(LanguageConstants.String, LanguageConstants.Object, LanguageConstants.Array), "The array to use for getting the number of elements, the string to use for getting the number of characters, or the object to use for getting the number of root-level properties.")
                .Build(),

            new FunctionOverloadBuilder("split")
                .WithReturnType(LanguageConstants.Array)
                .WithDescription("Returns an array of strings that contains the substrings of the input string that are delimited by the specified delimiters.")
                .WithRequiredParameter("inputString", LanguageConstants.String, "The string to split.")
                .WithRequiredParameter("delimiter", UnionType.Create(LanguageConstants.String, LanguageConstants.Array), "The delimiter to use for splitting the string.")
                .Build(),

            new FunctionOverloadBuilder("string")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Converts the specified value to a string.")
                .WithRequiredParameter("valueToConvert", LanguageConstants.Any, "The value to convert to string. Any type of value can be converted, including objects and arrays.")
                .Build(),

            new FunctionOverloadBuilder("int")
                .WithReturnType(LanguageConstants.Int)
                .WithDescription("Converts the specified value to an integer.")
                .WithRequiredParameter("valueToConvert", UnionType.Create(LanguageConstants.String, LanguageConstants.Int), "The value to convert to an integer.")
                .Build(),

            new FunctionOverloadBuilder("uniqueString")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Creates a deterministic hash string based on the values provided as parameters.")
                .WithRequiredVariableParameter("arg", LanguageConstants.String, "The value used in the hash function to create a unique string.")
                .Build(),

            new FunctionOverloadBuilder("guid")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Creates a value in the format of a globally unique identifier based on the values provided as parameters.")
                .WithRequiredVariableParameter("arg", LanguageConstants.String, "The value used in the hash function to create the GUID.")
                .Build(),

            new FunctionOverloadBuilder("trim")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Removes all leading and trailing white-space characters from the specified string.")
                .WithRequiredParameter("stringToTrim", LanguageConstants.String, "The value to trim.")
                .Build(),

            new FunctionOverloadBuilder("uri")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Creates an absolute URI by combining the baseUri and the relativeUri string.")
                .WithRequiredParameter("baseUri", LanguageConstants.String, "The base uri string.")
                .WithRequiredParameter("relativeUri", LanguageConstants.String, "The relative uri string to add to the base uri string.")
                .Build(),

            // TODO: Docs deviation
            new FunctionOverloadBuilder("substring")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Returns a substring that starts at the specified character position and contains the specified number of characters.")
                .WithRequiredParameter("stringToParse", LanguageConstants.String, "The original string from which the substring is extracted.")
                .WithRequiredParameter("startIndex", LanguageConstants.Int, "The zero-based starting character position for the substring.")
                .WithOptionalParameter("length", LanguageConstants.Int, "The number of characters for the substring. Must refer to a location within the string. Must be zero or greater.")
                .Build(),

            new FunctionOverloadBuilder("take")
                .WithReturnType(LanguageConstants.Array)
                .WithDescription("Returns an array with the specified number of elements from the start of the array.")
                .WithRequiredParameter("originalValue", LanguageConstants.Array, "The array to take the elements from.")
                .WithRequiredParameter("numberToTake", LanguageConstants.Int, "The number of elements to take. If this value is 0 or less, an empty array is returned. If it is larger than the length of the given array, all the elements in the array are returned.")
                .Build(),

            new FunctionOverloadBuilder("take")
                .WithReturnType(LanguageConstants.String)
                .WithDescription("Returns a string with the specified number of characters from the start of the string.")
                .WithRequiredParameter("originalValue", LanguageConstants.String, "The string to take the elements from.")
                .WithRequiredParameter("numberToTake", LanguageConstants.Int, "The number of characters to take. If this value is 0 or less, an empty string is returned. If it is larger than the length of the given string, all the characters are returned.")
                .Build(),

            // TODO: Resume here
            new FunctionOverloadBuilder("skip")
                .WithReturnType(LanguageConstants.Array)
                .WithFixedParameters(LanguageConstants.Array, LanguageConstants.Int)
                .Build(),

            new FunctionOverloadBuilder("skip")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String, LanguageConstants.Int)
                .Build(),

            new FunctionOverloadBuilder("empty")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(UnionType.Create(LanguageConstants.Null, LanguageConstants.Object, LanguageConstants.Array, LanguageConstants.String))
                .Build(),

            new FunctionOverloadBuilder("contains")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(LanguageConstants.Object, LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("contains")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(LanguageConstants.Array, LanguageConstants.Any)
                .Build(),

            new FunctionOverloadBuilder("contains")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(LanguageConstants.String, LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("intersection")
                .WithReturnType(LanguageConstants.Object)
                .WithVariableParameters(2, LanguageConstants.Object)
                .Build(),

            new FunctionOverloadBuilder("intersection")
                .WithReturnType(LanguageConstants.Array)
                .WithVariableParameters(2, LanguageConstants.Array)
                .Build(),

            new FunctionOverloadBuilder("union")
                .WithReturnType(LanguageConstants.Object)
                .WithVariableParameters(2, LanguageConstants.Object)
                .Build(),

            new FunctionOverloadBuilder("union")
                .WithReturnType(LanguageConstants.Array)
                .WithVariableParameters(2, LanguageConstants.Array)
                .Build(),

            new FunctionOverloadBuilder("first")
                .WithReturnType(LanguageConstants.Any)
                .WithFixedParameters(LanguageConstants.Array)
                .Build(),

            new FunctionOverloadBuilder("first")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("last")
                .WithReturnType(LanguageConstants.Any)
                .WithFixedParameters(LanguageConstants.Array)
                .Build(),

            new FunctionOverloadBuilder("last")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("indexOf")
                .WithReturnType(LanguageConstants.Int)
                .WithFixedParameters(LanguageConstants.String, LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("lastIndexOf")
                .WithReturnType(LanguageConstants.Int)
                .WithFixedParameters(LanguageConstants.String, LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("startsWith")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(LanguageConstants.String, LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("endsWith")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(LanguageConstants.String, LanguageConstants.String)
                .Build(),

            // TODO: Needs to support number type as well
            new FunctionOverloadBuilder("min")
                .WithReturnType(LanguageConstants.Int)
                .WithVariableParameters(1, LanguageConstants.Int)
                .Build(),

            new FunctionOverloadBuilder("min")
                .WithReturnType(LanguageConstants.Int)
                .WithFixedParameters(LanguageConstants.Array)
                .Build(),

            // TODO: Needs to support number type as well
            new FunctionOverloadBuilder("max")
                .WithReturnType(LanguageConstants.Int)
                .WithVariableParameters(1, LanguageConstants.Int)
                .Build(),

            new FunctionOverloadBuilder("max")
                .WithReturnType(LanguageConstants.Int)
                .WithFixedParameters(LanguageConstants.Array)
                .Build(),

            new FunctionOverloadBuilder("range")
                .WithReturnType(LanguageConstants.Array)
                .WithFixedParameters(LanguageConstants.Int, LanguageConstants.Int)
                .Build(),

            new FunctionOverloadBuilder("base64ToString")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("base64ToJson")
                .WithReturnType(LanguageConstants.Any)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("uriComponentToString")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("uriComponent")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("dataUriToString")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("dataUri")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters(LanguageConstants.Any)
                .Build(),

            new FunctionOverloadBuilder("array")
                .WithReturnType(LanguageConstants.Array)
                .WithFixedParameters(LanguageConstants.Any)
                .Build(),

            new FunctionOverloadBuilder("coalesce")
                .WithReturnType(LanguageConstants.Any)
                .WithVariableParameters(1, LanguageConstants.Any)
                .Build(),

            // TODO: Requires number type
            //new FunctionOverloadBuilder("float")
            //    .WithReturnType(LanguageConstants.Number)
            //    .WithFixedParameters(LanguageConstants.Any)
            //    .Build(),

            new FunctionOverloadBuilder("bool")
                .WithReturnType(LanguageConstants.Bool)
                .WithFixedParameters(LanguageConstants.Any)
                .Build(),

            new FunctionOverloadBuilder("json")
                .WithReturnType(LanguageConstants.Any)
                .WithFixedParameters(LanguageConstants.String)
                .Build(),

            new FunctionOverloadBuilder("dateTimeAdd")
                .WithReturnType(LanguageConstants.String)
                .WithOptionalFixedParameters(2, LanguageConstants.String, LanguageConstants.String, LanguageConstants.String)
                .Build(),

            // newGuid and utcNow are only allowed in parameter default values
            new FunctionOverloadBuilder("utcNow")
                .WithReturnType(LanguageConstants.String)
                .WithOptionalFixedParameters(0, LanguageConstants.String)
                .WithFlags(FunctionFlags.ParamDefaultsOnly)
                .Build(),

            new FunctionOverloadBuilder("newGuid")
                .WithReturnType(LanguageConstants.String)
                .WithFixedParameters()
                .WithFlags(FunctionFlags.ParamDefaultsOnly)
                .Build(),
        }.ToImmutableArray();

        // TODO: Add copyIndex here when we support loops.
        private static readonly ImmutableArray<BannedFunction> BannedFunctions = new[]
        {
            /*
             * The true(), false(), and null() functions are not included in this list because
             * we parse true, false and null as keywords in the lexer, so they can't be used as functions anyway.
             */

            new BannedFunction("variables", builder => builder.VariablesFunctionNotSupported()),
            new BannedFunction("parameters", builder => builder.ParametersFunctionNotSupported()),
            new BannedFunction("if", builder => builder.IfFunctionNotSupported()),
            new BannedFunction("createArray", builder => builder.CreateArrayFunctionNotSupported()),
            new BannedFunction("createObject", builder => builder.CreateObjectFunctionNotSupported()),

            BannedFunction.CreateForOperator("add", "+"),
            BannedFunction.CreateForOperator("sub", "-"),
            BannedFunction.CreateForOperator("mul", "*"),
            BannedFunction.CreateForOperator("div", "/"),
            BannedFunction.CreateForOperator("mod", "%"),
            BannedFunction.CreateForOperator("less", "<"),
            BannedFunction.CreateForOperator("lessOrEquals", "<="),
            BannedFunction.CreateForOperator("greater", ">"),
            BannedFunction.CreateForOperator("greaterOrEquals", ">="),
            BannedFunction.CreateForOperator("equals", "=="),
            BannedFunction.CreateForOperator("not", "!"),
            BannedFunction.CreateForOperator("and", "&&"),
            BannedFunction.CreateForOperator("or", "||")
        }.ToImmutableArray();

        public SystemNamespaceSymbol() : base("sys", SystemOverloads, BannedFunctions)
        {
        }
    }
}
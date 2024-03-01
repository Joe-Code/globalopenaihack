using Microsoft.SemanticKernel;
using System.ComponentModel;

public class DefinitionPlugin
{
    [KernelFunction()]
    public async Task<string> GetDefinitions(Kernel kernel, KernelArguments arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        string randomWords = arguments["randomWords"] != null ? Convert.ToString(arguments["randomWords"])! : string.Empty;
        string functionDefinition = $"Define the following word(s): {randomWords}";

        // add function to the kernel
        KernelFunction defineFunction = kernel.CreateFunctionFromPrompt(functionDefinition);

        // call OpenAI API using the function and provided input
        FunctionResult result = await defineFunction.InvokeAsync(kernel, arguments);

        return result.GetValue<string>()!;
    }
}

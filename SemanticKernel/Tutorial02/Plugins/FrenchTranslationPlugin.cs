using Microsoft.SemanticKernel;
using System.ComponentModel;

public class FrenchTranslationPlugin
{
    /// <summary>
    /// Given a word or phrase, translate it into French
    /// </summary>
    /// <param name="input">The word or phrase to translate</param>
    /// <returns>The translated word or phrase</returns>
    [KernelFunction(), Description("Given a word or phrase, translate it into French")]
    public async Task<string> TranslateAsync(Kernel kernel, string input)
    {
        string functionDefinition = $"Translate the following into French: {input}";

        // add function to the kernel
        KernelFunction toFrenchFunction = kernel.CreateFunctionFromPrompt(functionDefinition);

        // call OpenAI API using the function and provided input
        FunctionResult result = await toFrenchFunction.InvokeAsync(kernel, new(input));

        return result.GetValue<string>()!;
    }
}

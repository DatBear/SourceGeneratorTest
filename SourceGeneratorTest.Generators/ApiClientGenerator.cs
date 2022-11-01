using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Xml.Linq;
using static SourceGeneratorTest.Generators.GeneratedApiClientSyntaxHelpers;

namespace SourceGeneratorTest.Generators;

[Generator]
public class ApiClientGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ApiClientSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (ApiClientSyntaxReceiver)context.SyntaxReceiver;

        var apiClientMethods = receiver.GetGeneratedApiClientMethods();

        foreach (var apiClientEntry in apiClientMethods)
        {
            var classGenerator = new GeneratedApiClientClassGenerator(apiClientEntry.Key, apiClientEntry.Value);
            var client = classGenerator.Generate();
            context.AddSource($"{classGenerator.Name}.g.cs", SourceText.From(client, Encoding.UTF8));
        }
    }


    class ApiClientSyntaxReceiver : ISyntaxReceiver
    {
        private List<ClassDeclarationSyntax> GeneratedApiClientClasses { get; } = new();
        private List<MethodDeclarationSyntax> ValidControllerMethods { get; set; } = new();

        public Dictionary<ClassDeclarationSyntax, List<MethodDeclarationSyntax>> GetGeneratedApiClientMethods()
        {
            var dict = GeneratedApiClientClasses.ToDictionary(x => x, x => new List<MethodDeclarationSyntax>());
            
            foreach (var apiClient in GeneratedApiClientClasses)
            {
                var entry = dict[apiClient];
                var eAttr = GetAttributeSyntax(apiClient, GeneratedApiClientAttributeName);
                var eNamespace = GetGeneratedApiClientControllerNamespace(eAttr);
                foreach (var method in ValidControllerMethods)
                {
                    var mNamespace = GetNamespace(method);
                    if (eNamespace == mNamespace)
                    {
                        entry.Add(method);
                    }
                }
            }

            return dict;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds)
            {
                var generatedApiClientAttr = GetAttributeSyntax(cds, GeneratedApiClientAttributeName);

                if (generatedApiClientAttr != null)
                {
                    var controllerNamespace = GetGeneratedApiClientControllerNamespace(generatedApiClientAttr);

                    GeneratedApiClientClasses.Add(cds);
                }
            }

            if (syntaxNode is MethodDeclarationSyntax mds)
            {
                var mNamespace = GetNamespace(mds);
                var httpAttr = GetAttributeSyntax(mds, ValidHttpMethodAttributeNames);
                var responseTypeAttr = GetAttributeSyntax(mds, ResponseTypeAttributeName);

                if (httpAttr != null && responseTypeAttr != null)
                {
                    ValidControllerMethods.Add(mds);
                }
            }
        }
        
    }
}
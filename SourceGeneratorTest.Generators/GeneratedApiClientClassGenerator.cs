using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static SourceGeneratorTest.Generators.GeneratedApiClientSyntaxHelpers;

namespace SourceGeneratorTest.Generators;

public class GeneratedApiClientClassGenerator
{
    private readonly ClassDeclarationSyntax _apiClientClass;
    private readonly List<MethodDeclarationSyntax> _methods;

    public string Name { get; private set; }

    public GeneratedApiClientClassGenerator(ClassDeclarationSyntax apiClientClass, List<MethodDeclarationSyntax> methods)
    {
        _apiClientClass = apiClientClass;
        _methods = methods;
    }


    public string Generate()
    {
        Name = _apiClientClass.Identifier.ValueText;

        var sb = new StringBuilder();
        var acAttribute = GetAttributeSyntax(_apiClientClass, GeneratedApiClientAttributeName);
        var acNamespace = GetNamespace(_apiClientClass);
        
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using SourceGeneratorTest.Data.Model;");//todo figure out dynamically somehow?
        sb.AppendLine();

        sb.AppendLine($"namespace {acNamespace};");
        sb.AppendLine();
        sb.AppendLine($"public partial class {_apiClientClass.Identifier.Value}");
        sb.AppendLine("{");
        foreach (var method in _methods)
        {
            var info = new EndpointMethodInfo(method);
            sb.Tabs(1).AppendLine($"public async Task<{info.StatusMessageResponseType}> {info.Name}({string.Join(", ", info.Arguments)}){{");
            sb.Tabs(2).AppendLine($"return await SendRequest{(info.HasResponseType ? $"<{info.ResponseType}>" : string.Empty)}($\"{info.PathTemplate}\", HttpMethod.{info.HttpMethod}, {info.BodyArgument ?? "null"});");
            sb.Tabs(1).AppendLine("}");
            sb.Tabs(1).AppendLine();
        }
        sb.AppendLine("}");
        sb.AppendLine();

        return sb.ToString();
    }
}
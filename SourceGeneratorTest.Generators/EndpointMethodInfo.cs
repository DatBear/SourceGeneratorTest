using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static SourceGeneratorTest.Generators.GeneratedApiClientSyntaxHelpers;

namespace SourceGeneratorTest.Generators;

public class EndpointMethodInfo
{
    private readonly MethodDeclarationSyntax _mds;

    public string Name { get; set; }
    public string PathTemplate { get; set; }
    public string HttpMethod { get; set; }
    public bool HasResponseType { get; private set; }
    public string StatusMessageResponseType { get; set; }
    public string ResponseType { get; set; }
    public List<string> Arguments { get; set; } = new();
    public string? BodyArgument { get; set; }

    public EndpointMethodInfo(MethodDeclarationSyntax mds)
    {
        _mds = mds;

        var cds = ParentsUntil<ClassDeclarationSyntax>(mds);
        var cRouteAttr = GetAttributeSyntax(cds, RouteAttributeName);
        var cRouteTemplate = GetLiteralValue(cRouteAttr).Trim('/').Replace("[controller]", GetControllerName(mds));

        var mHttpAttr = GetAttributeSyntax(mds, ValidHttpMethodAttributeNames);
        var mRouteAttr = GetAttributeSyntax(mds, RouteAttributeName);
        var mRouteTemplate = (GetLiteralValue(mHttpAttr) ?? GetLiteralValue(mRouteAttr))?.Trim('/');
        var mResponseTypeAttr = GetAttributeSyntax(mds, ResponseTypeAttributeName);

        Name = _mds.Identifier.ValueText;
        PathTemplate = $"{cRouteTemplate}/{mRouteTemplate}";
        HttpMethod = mHttpAttr?.Name?.ToString()?.Replace("Http", string.Empty) ?? "Get";
        ResponseType = GetResponseType(mResponseTypeAttr);
        HasResponseType = ResponseType != "void";
        StatusMessageResponseType = ResponseType == "void" ? "StatusMessage" : $"StatusMessage<{ResponseType.Trim(' ')}>";
        Arguments = mds.ParameterList.Parameters.Select(x => x != null ? $"{x.Type?.ToString()} {x.Identifier.ValueText}" : null).Where(x => x != null).ToList();
        BodyArgument = mds.ParameterList.Parameters.FirstOrDefault(x => x.AttributeLists.SelectMany(x => x.Attributes).Any(attr => attr.Name.ToString() == "FromBody"))?.Identifier.ValueText;
    }
}
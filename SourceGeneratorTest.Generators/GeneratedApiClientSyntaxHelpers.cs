using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace SourceGeneratorTest.Generators;

public static class GeneratedApiClientSyntaxHelpers
{
    internal const string GeneratedApiClientAttributeName = "GeneratedApiClient";

    internal static readonly List<string> ValidHttpMethodAttributeNames = new()
    {
        //todo add delete,patch,put
        "HttpGet",
        "HttpPost",
    };

    internal const string ResponseTypeAttributeName = "ProducesResponseType";
    internal const string ControllerSuffix = "Controller";
    internal const string RouteAttributeName = "Route";

    internal static AttributeSyntax? GetAttributeSyntax(SyntaxNode sn, string name)
    {
        return GetAttributeSyntax(sn, new List<string> { name });
    }

    internal static AttributeSyntax? GetAttributeSyntax(SyntaxNode sn, IEnumerable<string> names)
    {
        var attributeList = sn is ClassDeclarationSyntax cds ? cds.AttributeLists : sn is MethodDeclarationSyntax mds ? mds.AttributeLists : new SyntaxList<AttributeListSyntax>();
        if (!attributeList.Any()) return null;
        var attrs = attributeList.SelectMany(x => x.Attributes);
        return attrs.FirstOrDefault(x => names.Contains(x.Name.ToString()));
    }

    internal static string? GetLiteralValue(AttributeSyntax attrs, int? index = null)
    {
        var expr = attrs?.ArgumentList?.Arguments.FirstOrDefault(x => x is { Expression: LiteralExpressionSyntax })?.Expression as LiteralExpressionSyntax;
        return expr?.Token.ValueText;
    }

    //internal static AttributeSyntax? GetAttributeSyntax(ClassDeclarationSyntax cds, string name)
    //{
    //    var attrList = cds.AttributeLists.FirstOrDefault(x => x.Attributes.Any(x => x.Name.ToString() == name));
    //    return attrList?.Attributes.FirstOrDefault(x => x.Name.ToString() == name);
    //}

    //internal static AttributeSyntax? GetAttributeSyntax(MethodDeclarationSyntax mds, IEnumerable<string> names)
    //{
    //    var attrList = mds.AttributeLists.FirstOrDefault(x => x.Attributes.Any(x => names.Contains(x.Name.ToString())));
    //    return attrList?.Attributes.FirstOrDefault(x => names.Contains(x.Name.ToString()));
    //}

    //internal static AttributeSyntax? GetAttributeSyntax(MethodDeclarationSyntax mds, string name)
    //{
    //    return GetAttributeSyntax(mds, new List<string> { name });
    //}

    internal static string GetGeneratedApiClientControllerNamespace(AttributeSyntax attr)
    {
        var arg = attr.ArgumentList.Arguments.FirstOrDefault();
        if (arg != null)
        {
            if (arg.Expression is LiteralExpressionSyntax les)
            {
                return les.Token.ValueText;
            }
            throw new Exception($"Only string literals are allowed in {GeneratedApiClientAttributeName}'s ControllerNamespace property.");
        }
        return null;
    }

    internal static string GetNamespace(SyntaxNode cds)
    {
        var cNamespace = ParentsUntil<NamespaceDeclarationSyntax>(cds)?.Name?.ToString();
        cNamespace ??= ParentsUntil<FileScopedNamespaceDeclarationSyntax>(cds)?.Name?.ToString();
        return cNamespace;
    }

    internal static string? GetClassName(MethodDeclarationSyntax mds)
    {
        var cds = ParentsUntil<ClassDeclarationSyntax>(mds);
        return cds?.Identifier.ValueText;
    }

    internal static string? GetControllerName(MethodDeclarationSyntax mds)
    {
        return GetClassName(mds)?.Replace(ControllerSuffix, string.Empty);
    }

    internal static string GetResponseType(AttributeSyntax attr)
    {
        var arg = attr.ArgumentList.Arguments.FirstOrDefault(x => x is { Expression: TypeOfExpressionSyntax })?.Expression as TypeOfExpressionSyntax;
        if (arg == null) return null;
        return arg.Type.ToString();
    }

    internal static T? ParentsUntil<T>(SyntaxNode node) where T : SyntaxNode
    {
        if (node is T correctType) return correctType;
        if (node != null && node.Parent != null) return ParentsUntil<T>(node.Parent);
        return default;
    }

}
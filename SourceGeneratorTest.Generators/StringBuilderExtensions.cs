using System.Text;

namespace SourceGeneratorTest.Generators;

public static class StringBuilderExtensions
{
    public static StringBuilder Tabs(this StringBuilder sb, int tabs)
    {
        sb.Append(new string('\t', tabs));
        return sb;
    }
}
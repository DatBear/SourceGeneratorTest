namespace SourceGeneratorTest;

public class GeneratedApiClientAttribute : Attribute
{
    public readonly string ControllerNamespace;

    public GeneratedApiClientAttribute(string controllerNamespace)
    {
        ControllerNamespace = controllerNamespace;
    }
}
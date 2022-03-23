using Client.ServerSymbol;
using IApplicationService;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace ApiSourceGenerator
{
    [Generator]
    public class ApiDocumentSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            //            var sourceBuilder = new StringBuilder(@"
            //using System;
            //namespace HelloWorldGenerated
            //{
            //    public static class HelloWorld
            //    {
            //        public static void SayHello()
            //        {
            //            Console.WriteLine(""Hello from generated code!"");
            //            Console.WriteLine(""The following syntax trees existed in the compilation that created this program:"");
            //");

            //            // using the context, get a list of syntax trees in the users compilation
            //            var syntaxTrees = context.Compilation.SyntaxTrees;

            //            // add the filepath of each tree to the class we're building
            //            foreach (SyntaxTree tree in syntaxTrees)
            //            {
            //                sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
            //            }

            //            // finish creating the source to inject
            //            sourceBuilder.Append(@"
            //        }
            //    }
            //}");

            //            // inject the created source into the users compilation
            //context.AddSource("helloWorldGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

            if (context.SyntaxReceiver is CustomSyntaxReceiver receiver)
            {

            }
            var source = new StringBuilder();
            source.Append(@"
            using IApplicationService;
            using Microsoft.AspNetCore.Mvc;
            using Microsoft.Extensions.Logging;
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using System.Text.Json;
            using System.Net.Http;
            namespace ApiDocument
            {");
            foreach (var interfaceType in typeof(IApplicationService.Base.AppQuery.PageQueryInputBase).Assembly.GetTypes().Where(x => x.IsInterface && !x.GetInterfaces().Any(x => x.Name == "IActorService")))
            {
                var remotesrvAttr = interfaceType.GetCustomAttribute<RemoteServiceAttribute>();
                if (remotesrvAttr != null && remotesrvAttr is RemoteServiceAttribute svcattr)
                {
                    source.Append(@$"
                        [ApiController]
                        [Route(""[controller]/[Action]"")]
                        public class {interfaceType.Name}Controller : ControllerBase
                        {{
                            private readonly IHttpClientFactory httpClientFactory;
                            public {interfaceType.Name}Controller(IHttpClientFactory httpClientFactory)
                            {{
                                this.httpClientFactory = httpClientFactory;
                            }}
                        ");
                    foreach (var method in interfaceType.GetMethods())
                    {
                        var remotefuncAttr = method.GetCustomAttribute<RemoteFuncAttribute>();
                        if (remotefuncAttr != null && remotefuncAttr is RemoteFuncAttribute funattr && funattr.FuncType == FuncType.Invoke)
                        {
                            source.Append($@"
                            /// <summary>
                            /// {(remotefuncAttr.FuncDescription)}
                            /// </summary>
                            {(method.GetParameters().Any() ? $"/// <param name=\"{method.GetParameters()[0].Name}\"></param>" : "")}
                            [HttpPost]
                            public async Task<dynamic> {method.Name}({(method.GetParameters().Any() ? $"{GetGericType(method.GetParameters()[0].ParameterType)} {method.GetParameters()[0].Name}" : "")})
                            {{
                                var req = new HttpRequestMessage();
                                req.RequestUri = new Uri(""http://apigateway.dapreshop/{svcattr.HostName}/{svcattr.ServerName}/{method.Name}"");
                                req.Content = new StringContent({(method.GetParameters().Any() ? "JsonSerializer.Serialize(" + method.GetParameters()[0].Name + ")" : "string.Empty")});
                                if (Request.Headers.ContainsKey(""Authentication""))
                                    req.Headers.Add(""Authentication"", Request.Headers[""Authentication""].ToString());
                                var result = await httpClientFactory.CreateClient().SendAsync(req);
                                return JsonSerializer.Deserialize<object>(await result.Content.ReadAsStringAsync());
                            }}");
                        }
                    }
                    source.Append("}");
                }
            }
            source.Append("}");
            context.AddSource("DynamicApiDocument.cs", SourceText.From(source.ToString(), Encoding.UTF8));
        }
        string GetGericType(Type type)
        {
            if (type.IsGenericType)
                return $"{type.Name.Replace("`1", "")}<{string.Join(",", type.GetGenericArguments().Select(x => GetGericType(x)))}>";
            return type.FullName;
        }
        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => new CustomSyntaxReceiver());
        }
    }

    internal class CustomSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Models { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                Models.Add(classDeclarationSyntax);
            }
        }
    }
}


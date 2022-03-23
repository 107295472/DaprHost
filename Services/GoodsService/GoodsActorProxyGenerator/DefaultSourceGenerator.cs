﻿using ApplicationService;
using IApplicationService.Base;
using Mesh.Dapr;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;

namespace GoodsActorProxyGenerator
{
    [Generator]
    public class DefaultSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var proxylist = ProxyCodeGeneratorTemplate.GetTemplate<ApiResult, EventHandler>();
            foreach (var item in proxylist)
            {
                context.AddSource(item.sourceName, SourceText.From(item.sourceCode, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();
        }
    }
}

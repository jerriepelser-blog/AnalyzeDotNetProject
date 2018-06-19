using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;

namespace AnalyzeDotNetProject
{
    /// <remarks>
    /// Credit for the stuff happening in here goes to the https://github.com/jaredcnance/dotnet-status project
    /// </remarks>
    internal class DependencyGraphService
    {
        public DependencyGraphSpec GenerateDependencyGraph(string projectPath)
        {
            var dotNetRunner = new DotNetRunner();

            string dgOutput = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                
            string[] arguments = new[] {"msbuild", $"\"{projectPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={dgOutput}"};

            var runStatus = dotNetRunner.Run(Path.GetDirectoryName(projectPath), arguments);

            if (runStatus.IsSuccess)
            {
                string dependencyGraphText = File.ReadAllText(dgOutput);
                return new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
            }
            else
            {
                throw new Exception($"Unable to process the the project `{projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?" +
                                                     $"\r\n\r\nHere is the full error message returned from the Microsoft Build Engine:\r\n\r\n" + runStatus.Output);
            }
        }
    }
}
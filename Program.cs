using System;
using System.Linq;
using NuGet.ProjectModel;

namespace AnalyzeDotNetProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Replace to point to your project or solution
            string projectPath = @"c:\development\jerriep\dotnet-outdated\DotNetOutdated.sln";

            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            foreach(var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                Console.WriteLine(project.Name);
                
                foreach(var targetFramework in project.TargetFrameworks)
                {
                    Console.WriteLine($"  [{targetFramework.FrameworkName}]");

                    foreach(var dependency in targetFramework.Dependencies)
                    {
                        Console.WriteLine($"  {dependency.Name}, v{dependency.LibraryRange.VersionRange.ToShortString()}");
                    }
                }
            }
        }
    }
}

﻿using System;
using Entitas.CodeGeneration;
using System.IO;
using Entitas.Unity;
using System.Reflection;
using CSharpCodeGenerator;

namespace CodeGeneration.Roslyn
{
    class MainClass
    {
        public static void Main (string [] args)
        {
            /* Properties:
Entitas.CodeGenerator.ProjectPath = CodeGenerator.Dependency.dll
Entitas.CodeGenerator.GeneratedFolderPath = ../../GeneratedTestFolderPath
Entitas.CodeGenerator.PoolNames = meta,world
Entitas.CodeGenerator.BlueprintNames =

            */
            var propertiesContent = File.ReadAllText (args [0]);
            var properties = new Properties (propertiesContent);
            var projectPath = properties ["Entitas.CodeGenerator.ProjectPath"];
            if (!File.Exists (projectPath)) {
                throw new Exception ($"Project at path '{projectPath}' does not exist!");
            }

            var project = ProjectStructure.Load(projectPath);
            var outputDirectory = properties ["Entitas.CodeGenerator.GeneratedFolderPath"];
            var pools = properties ["Entitas.CodeGenerator.PoolNames"].Split (',');
            var blueprintNames = properties ["Entitas.CodeGenerator.BlueprintNames"].Split (',');

            var codeGenerators = new ICodeGenerator []{
                new BlueprintsGenerator(),
                new ComponentExtensionsGenerator(),
                new ComponentIndicesGenerator(),
                new PoolAttributesGenerator(),
                new PoolsGenerator()
            };


            var output = RoslynCodeGenerator.Generate (project, pools, blueprintNames, outputDirectory, codeGenerators);

            foreach (var file in output) {
                System.Console.WriteLine ("file.fileName: " + file.fileName);
            }

        }
    }
}

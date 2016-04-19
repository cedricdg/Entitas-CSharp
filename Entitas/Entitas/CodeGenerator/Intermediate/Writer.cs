﻿using System;
using System.IO;
using Entitas.CodeGenerator;

namespace Entitas.CodeGenerator {
    public class Writer : IWriter {
        string directory;

        public Writer(string outputDirectory) {
            this.directory = outputDirectory;
        }

        public void Write(CodeGenFile[] codegenFiles) {
            CleanDir(directory);

            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            foreach (var file in codegenFiles) {
                var fileName = directory + file.fileName + ".cs";
                var fileContent = file.fileContent.Replace("\n", Environment.NewLine);
                var header = string.Format(CodeGenerator.AUTO_GENERATED_HEADER_FORMAT, file.generatorName);
                File.WriteAllText(fileName, header + fileContent);
            }
        }

        public static string GetSafeDir(string directory) {
            if (!directory.EndsWith("/", StringComparison.Ordinal)) {
                directory += "/";
            }
            if (!directory.EndsWith("Generated/", StringComparison.Ordinal)) {
                directory += "Generated/";
            }
            return directory;
        }

        public static void CleanDir(string directory) {
            directory = GetSafeDir(directory);
            if (Directory.Exists(directory)) {
                var files = new DirectoryInfo(directory).GetFiles("*.cs", SearchOption.AllDirectories);
                foreach (var file in files) {
                    try {
                        File.Delete(file.FullName);
                    } catch {
                        Console.WriteLine("Could not delete file " + file);
                    }
                }
            } else {
                Directory.CreateDirectory(directory);
            }
        }
    }
}


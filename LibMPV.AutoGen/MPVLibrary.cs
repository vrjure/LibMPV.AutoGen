using CppSharp;
using CppSharp.AST;
using CppSharp.Generators.CSharp;
using CppSharp.Parser;
using CppSharp.Passes;
using LibMPV.AutoGen.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace LibMPV.AutoGen
{
    internal class MPVLibrary : ILibrary
    {
        private readonly string _includeDir;
        private readonly string _outputDir;
        public MPVLibrary(string includeDir, string outputDir)
        {
            _includeDir = includeDir;
            _outputDir = outputDir;
        }

        public void Setup(Driver driver)
        {
            var options = driver.Options;
            options.StripLibPrefix = false;
            options.OutputDir = _outputDir;
            options.GeneratorKind = CppSharp.Generators.GeneratorKind.CSharp;
            options.MarshalCharAsManagedChar = true;
            options.GenerationOutputMode = GenerationOutputMode.FilePerUnit;
            options.GenerateName = unit => $"{char.ToUpper(unit.FileNameWithoutExtension[0])}{unit.FileNameWithoutExtension[1..]}";
            options.GenerateFunctionTemplates = false;

            var module = options.AddModule("libmpv-2");
            module.OutputNamespace = $"LibMPVSharp";

            var files = Directory.GetFiles(_includeDir);
            foreach (var item in files)
            {
                module.Headers.Add(Path.GetFileName(item));
            }

            var parserOptions = driver.ParserOptions;
            parserOptions.AddIncludeDirs(_includeDir);
            parserOptions.LanguageVersion = LanguageVersion.C99_GNU;
            parserOptions.Verbose = true;
            parserOptions.SetupMSVC(VisualStudioVersion.VS2022);
        }

        public void SetupPasses(Driver driver)
        {

        }

        public void Preprocess(Driver driver, ASTContext ctx)
        {
            //ctx.SetFunctionParameterUsage("mpv_render_context_create", 1, ParameterUsage.Out);
        }

        public void Postprocess(Driver driver, ASTContext ctx)
        {
            var includes = Directory.GetFiles(_includeDir).Where(f => f.EndsWith(".h")).Select(f=> Path.GetFileName(f)).ToList();
            var units = ctx.TranslationUnits.Where(f => includes.Contains(f.FileName));

            var generatorOption = new GeneratorOption();
            foreach (var item in units)
            {
                using var generator = new LibMPV.AutoGen.Generation.CSharpGenerator(driver.Options, generatorOption, item);
                generator.Generate();
            }
        }
    }
}

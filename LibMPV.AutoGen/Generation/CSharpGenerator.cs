using CppSharp;
using CppSharp.AST;
using LibMPV.AutoGen.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMPV.AutoGen.Generation
{
    internal class CSharpGenerator : GeneratorBase
    {
        public CSharpGenerator(DriverOptions option, GeneratorOption generatorOption, TranslationUnit unit) : base(option, generatorOption, unit)
        {
            
        }

        protected override string Extension => ".cs";

        protected override IEnumerable<string> Usings()
        {
            yield return "using System;";
            yield return "using System.Runtime.CompilerServices;";
            yield return "using System.Runtime.InteropServices;";

        }

        public override void Generate()
        {
            foreach (var item in Usings())
            {
                WriteLine(item);
            }

            WriteLine($"namespace {_option.Modules[1].OutputNamespace}");
            using (BeginBlock())
            {
                WriteLine($"public unsafe partial class {_option.GenerateName(_unit)}");
                using (BeginBlock())
                {
                    GenerateFunctions(_unit);
                }
                WriteLine();

                GenerateClass(_unit);

                WriteLine();

                GenerateEnums(_unit);

                WriteLine();

                GenerateDelegates(_unit);
            }
        }

        protected override void GenerateFunctions(TranslationUnit unit)
        {
            var functions = unit.Functions;
            foreach (var func in functions)
            {
                CommentParser.Parse(func.Comment, _textWriter);
                WriteLine($"[LibraryImport(LibraryName.Name, EntryPoint = \"{func.OriginalName}\", StringMarshalling = StringMarshalling.Utf8)]");
                WriteLine("[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]");
                Write($"internal static partial {TypePrint(func.ReturnType, isReturnType: true)} {func.Name}(");

                var first = true;
                foreach (var p in func.Parameters)
                {
                    if (!first)
                    {
                        Write(", ");
                    }
                    first = false;

                    if (p.IsOut)
                    {
                        Write("out ");
                    }
                    else if (p.IsInOut)
                    {
                        Write("ref ");
                    }

                    if (p.QualifiedType.ToString() == "string")
                    {
                        Write("[MarshalAs(UnmanagedType.LPUTF8Str)]");
                    }
                    Write($"{TypePrint(p.QualifiedType, $"{func.Name}_{p.OriginalName}")} {NamePrint(p.OriginalName)}");
                }

                WriteLine(");");
                WriteLine();
            }
        }

        protected override void GenerateClass(TranslationUnit unit)
        {
            var classes = unit.Classes;
            foreach (var @class in classes)
            {
                if (_generatorOption.SkipClasses.Contains(@class.Name)) continue;
                GenerateClass(@class);
                WriteLine();
            }
        }

        private void GenerateClass(Class @class)
        {
            CommentParser.Parse(@class.Comment, _textWriter);

            WriteLine("[StructLayout(LayoutKind.Sequential)]");
            WriteLine($"public unsafe struct {@class.Name}");

            using (BeginBlock())
            {
                if (@class.Classes.Any())
                {
                    foreach (var item in @class.Classes)
                    {
                        if (_generatorOption.SkipClasses.Contains(item.Name)) continue;
                        GenerateClass(item);
                    }
                }

                foreach (var field in @class.Fields)
                {
                    CommentParser.Parse(field.Comment, _textWriter);
                    if (field.QualifiedType.ToString() == "string")
                    {
                        WriteLine("[MarshalAs(UnmanagedType.LPUTF8Str)]");
                    }
                    WriteLine($"public {TypePrint(field.QualifiedType, $"{@class.Name}_{field.OriginalName}")} {NamePrint(field.OriginalName)};");
                }
            }
        }

        protected override void GenerateEnums(TranslationUnit unit)
        {
            var enums = unit.Enums;
            foreach (var @enum in enums)
            {
                CommentParser.Parse(@enum.Comment, _textWriter);
                if (@enum.IsFlags)
                {
                    WriteLine("[Flags]");
                }

                WriteLine($"public enum {@enum.Name}");
                using (BeginBlock())
                {
                    var first = true;
                    foreach (var item in @enum.Items)
                    {
                        if (!first)
                        {
                            WriteLine(",");
                        }

                        first = false;
                        CommentParser.Parse(item.Comment, _textWriter);
                        Write($"{NamePrint(item.Name)} = {(int)item.Value}");
                    }
                    WriteLine();
                }

                WriteLine();
            }
        }

        protected override void GenerateDelegates(TranslationUnit unit)
        {
            var delegates = unit.Typedefs.Where(f => f.QualifiedType.Type is PointerType pt && pt.Pointee is FunctionType);
            foreach (var item in delegates)
            {
                var funcType = (item.QualifiedType.Type as PointerType)!.Pointee as FunctionType;
                CommentParser.Parse(item.Comment, _textWriter);
                WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl)]");
                Write($"public unsafe delegate {TypePrint(funcType!.ReturnType)} {item.Name}(");

                var first = true;
                foreach (var p in funcType.Parameters)
                {
                    if (!first)
                    {
                        Write(", ");
                    }
                    first = false;
                    Write($"{TypePrint(p.QualifiedType)} {NamePrint(p.OriginalName)}");
                }

                WriteLine(");");
                WriteLine();
            }

            if (_addOnDelegates.Count > 0)
            {
                foreach (var item in _addOnDelegates)
                {
                    WriteLine(item);
                }
            }
        }
    }
}

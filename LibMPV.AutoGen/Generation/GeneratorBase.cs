using CppSharp;
using CppSharp.AST;
using CppSharp.AST.Extensions;
using LibMPV.AutoGen.Parsers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibMPV.AutoGen.Generation
{
    internal abstract class GeneratorBase : IDisposable
    {
        private readonly StreamWriter _streamWriter;

        protected readonly IndentedTextWriter _textWriter;
        protected readonly DriverOptions _option;
        protected readonly TranslationUnit _unit;
        protected GeneratorOption _generatorOption;
        protected readonly List<string> _addOnDelegates = new List<string>();

        public GeneratorBase(DriverOptions option, GeneratorOption generatorOption, TranslationUnit unit)
        {
            _option = option;
            _unit = unit;
            _generatorOption = generatorOption;
            if (!Directory.Exists(option.OutputDir))
            {
                Directory.CreateDirectory(option.OutputDir);
            }

            var fullPath = Path.Combine(option.OutputDir, $"{_option.GenerateName(_unit)}{Extension}");

            _streamWriter = File.CreateText(fullPath);
            _textWriter = new IndentedTextWriter(_streamWriter);
        }

        protected abstract string Extension { get; }

        protected abstract IEnumerable<string> Usings();

        public abstract void Generate();

        protected abstract void GenerateEnums(TranslationUnit unit);

        protected abstract void GenerateClass(TranslationUnit unit);

        protected abstract void GenerateFunctions(TranslationUnit unit);

        protected abstract void GenerateDelegates(TranslationUnit unit);

        protected internal void Write(string value) => _textWriter.Write(value);

        protected internal void WriteLine() => _textWriter.WriteLine();

        protected internal void WriteLine(string line) => _textWriter.WriteLine(line);

        protected void WriteLineWithoutIntent(string line) => _textWriter.WriteLineNoTabs(line);

        protected string TypePrint(QualifiedType type, string callbackName = "", bool isReturnType = false)
        {
            var name = TypePrint(type.ToString(), callbackName, isReturnType);
            if (type.Type is PointerType pt && !pt.GetFinalPointee().IsPrimitiveType())
            {
                name += "*";
                if (pt.Pointee is PointerType)
                {
                    name += "*";
                }
                return name;
            }
            else
            {
                return name;
            }
        }
        protected string TypePrint(string type, string callbackName = "", bool isReturnType = false)
        {
            if (string.IsNullOrEmpty(type)) return type;

            if (_generatorOption.AddOnDelegates.TryGetValue($"{callbackName}Callback", out var deldateStr))
            {
                _addOnDelegates.AddRange(deldateStr);
            }

            var functionReturnType = isReturnType ? $"function_return_type_{type}" : type.ToString();
            if (_generatorOption.TypeConverters.TryGetValue(functionReturnType, out var val))
            {
                return val;
            }

            return type;
        }

        protected string NamePrint(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (_generatorOption.NameConverters.TryGetValue(name, out var n))
            {
                return n;
            }

            return name;
        }

        protected virtual IDisposable BeginBlock(bool inline = false)
        {
            WriteLine("{");
            _textWriter.Indent++;
            return new Disposable(() =>
            {
                _textWriter.Indent--;

                if (inline)
                    Write("}");
                else
                    WriteLine("}");
            });
        }

        public void Dispose()
        {
            _textWriter?.Flush();
            _streamWriter?.Flush();
            _streamWriter?.Dispose();
            _textWriter?.Dispose();
        }

        private sealed class Disposable : IDisposable
        {
            private readonly Action _action;

            public Disposable(Action action) => _action = action;

            public void Dispose() => _action();
        }
    }
}

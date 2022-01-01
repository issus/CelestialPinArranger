using SymbolBuilder.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SymbolBuilder.Translators
{
    public interface IEdaTranslator
    {
        string ProgramName { get; }

        bool SupportsSymbol(SymbolDefinition symbol);

        void GenerateFile(IEnumerable<SymbolDefinition> symbols, string filePath);
        IEnumerable<Bitmap> GeneratePreview(SymbolDefinition symbol, int sizeX, int sizeY);
        void WriteStream(IEnumerable<SymbolDefinition> symbols, Stream stream);

        object GenerateNativeType(IEnumerable<SymbolDefinition> symbols);
    }
}

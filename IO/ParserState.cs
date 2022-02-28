using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialang.Compilation.IO
{
    public enum ParserState
    {
        None = -1,
        Reading = 0,
        EntryName = 1,
        Event = 2,
        Emote = 4,
        Color = 8,
        Combine = 16,
        Pause = 32,
        FormatBold = 64,
        FormatItalic = 128,
        FormatUnder = 256,
        FormatStrike = 512,
        Format = FormatBold | FormatItalic | FormatUnder | FormatStrike,
        Backslash = 1024,
        Choice = 2048,
        BackslashReliant = Choice,
    }
}

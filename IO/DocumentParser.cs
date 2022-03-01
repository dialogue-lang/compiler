using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;

using Dialang.Compilation.Classification;
using System.Collections;

namespace Dialang.Compilation.IO
{
    // TODO: clean this class up what the fuck is happening
    public sealed class DocumentParser : IDisposable
    {
        private readonly LoggingHandle log;
        private readonly StringReader s;
        private string str;
        private ParserState state;
        private bool disposed;
        private Hashtable entries;
        private Entry current;
        private int line;
        private bool reset;
        private int read;
        private StringBuilder temp;
        private int tempStart;
        private int offset;

        public Hashtable Parse()
        {
            while (Next())
                Thread.Yield();

            if (!reset)
                PushCurrent();

            log("Finished parse.");
            log("");

            return entries;
        }

        public bool Next()
        {
            if (disposed)
                return false;

            str = s.ReadLine();
            offset = 0;

            if (str == null)
                return false;

            if (str.Length <= 0)
            {
                Reset();
                return true;
            }

            log(str);

            SwitchState();
            ActState();

            return true;
        }

        private void SwitchState()
        {
            switch (state)
            {
                case ParserState.None:
                    if (str!.StartsWith('#'))
                    {
                        state = ParserState.EntryName;
                        reset = false;
                    } else
                        Reset();
                        
                    break;

                case ParserState.EntryName:
                    state = ParserState.Reading;
                    break;
            }
        }

        private void ActState()
        {
            switch (state)
            {
                case ParserState.None:
                    log("No valid data...");
                    break;

                case ParserState.EntryName:
                    log("Found entry name.");
                    current.Name = str!.Substring(1);
                    break;

                default:
                    log("Reading...");
                    Read();
                    break;
            }
        }
        
        private void Reset()
        {
            if (reset)
                return;
            reset = true;

            log("Resetting...");

            state = ParserState.None;
            PushCurrent();
            current = new Entry();
            line = 0;
            offset = 0;
        }

        private void PushCurrent()
        {
            log("<< Push Entry >>");
            entries.Add(current.Name, current);
        }

        private void Read()
        {
            using StringReader sr = new StringReader(str!);
            StringBuilder sb = new StringBuilder();
            line = current.Add();
            offset = 0;

            while (sr.Peek() >= 0)
            {
                switch (state)
                {
                    default:
                        ReadHandle(ReadNext(sr), sb, sr);
                        break;
                }
            }

            current.Scripts[line].Text = sb.ToString();
            state = ParserState.Reading;
            read = 0;
            reset = false;
        }

        private char ReadNext(StringReader sr)
        {
            read++;
            return (char)sr.Read();
        }

        private void ReadHandle(char c, StringBuilder b, StringReader r)
        {
            if (ReadCheck(c))
            {
                temp.Clear();
                return;
            }

            switch (state)
            {
                case ParserState.Reading:
                    b.Append(c);
                    break;

                case ParserState.Reading | ParserState.Combine:
                    offset++;
                    temp.Append(c);
                    break;

                case ParserState.Reading | ParserState.Emote:
                    offset++;
                    temp.Append(c);
                    break;

                case ParserState.Reading | ParserState.Event:
                    offset++;
                    temp.Append(c);
                    break;

                case ParserState.Reading | ParserState.Pause:
                    if (int.TryParse(new string(c, 1), out int i))
                    {
                        log($"Pause for {i / 4d:0.00} seconds.");
                        current.Scripts[line].Add(new Pause(read - offset - 2, i));
                        offset++;
                    } else
                    {
                        b.Append(c);
                    }
                    offset++;

                    state ^= ParserState.Pause;
                    break;

                case ParserState.Reading | ParserState.Backslash:
                    offset++;

                    switch (c)
                    {
                        case 'r':
                            state |= ParserState.Choice;
                            break;

                        case 'n':
                            b.Append('\n');
                            break;

                        default:
                            b.Append(c);
                            break;
                    }

                    state ^= ParserState.Backslash;
                    break;

                case ParserState.Reading | ParserState.Choice:
                    if (c == '\\')
                    {
                        current.Scripts[line].Add(new Choice(current.Scripts[line].Choices.Length, temp.ToString()));
                        state ^= ParserState.Choice;
                    } else
                    {
                        offset++;
                        temp.Append(c);
                    }
                    break;
            }
        }

        private bool ReadCheck(char c)
        {
            if (Contains(state, ParserState.BackslashReliant))
                return false;

            switch (c)
            {
                case '{':
                    if (!state.HasFlag(ParserState.Event))
                    {
                        state |= ParserState.Event;
                        return true;
                    }

                    return false;

                case '}':
                    if (state.HasFlag(ParserState.Event))
                    {
                        state ^= ParserState.Event;
                        offset++;
                        current.Scripts[line].Add(new Event(temp.ToString(), tempStart - offset));
                        log($"Event: {current.Scripts[line].Events[^1].Name}");
                        return true;
                    }

                    return false;

                case '[':
                    if (!state.HasFlag(ParserState.Emote))
                    {
                        state |= ParserState.Emote;
                        return true;
                    }

                    return false;

                case ']':
                    if (state.HasFlag(ParserState.Emote))
                    {
                        state ^= ParserState.Emote;
                        current.Scripts[line].Add(new Emote(temp.ToString(), tempStart - offset));
                        offset++;
                        log($"Emote: {current.Scripts[line].Emotes[^1].Name}");
                        return true;
                    }

                    return false;

                case '<':
                    if ((state ^ ParserState.Format) == ParserState.Reading)
                    {
                        state |= ParserState.Color;
                        return true;
                    }

                    return false;

                case '>':
                    if (state.HasFlag(ParserState.Color))
                    {
                        state ^= ParserState.Color;
                        return true;
                    }

                    return false;

                case '|':
                    if (!state.HasFlag(ParserState.Combine))
                    {
                        state |= ParserState.Combine;
                        return true;
                    } else
                    {
                        current.Scripts[line].Add(new Combine(tempStart, tempStart + temp.Length - offset));
                        log($"Combine: {temp}");
                        state ^= ParserState.Combine;
                        return true;
                    }

                case '^':
                    if (!state.HasFlag(ParserState.Pause))
                    {
                        state |= ParserState.Pause;
                        return true;
                    }

                    return false;

                case '\\':
                    if (state == ParserState.Reading)
                    {
                        state |= ParserState.Backslash;
                        return true;
                    }

                    return false;

                    //case '*': // Template for later
                    //    if (!state.HasFlag(ParserState.FormatBold))
                    //    {
                    //        state |= ParserState.FormatBold;
                    //        return true;
                    //    } else
                    //    {
                    //        state ^= ParserState.FormatBold;
                    //        return true;
                    //    }
                    //
                    // TODO: Add support for formatting (*bold*, `italic`, _underline_, ~strikethrough~)
            }

            return false;
        }

        private bool Contains(ParserState x, ParserState y)
        {
            return (x & y) != 0;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;
            state = ParserState.None;
            s.Dispose();
        }

        public DocumentParser(string str, LoggingHandle log)
        {
            s = new StringReader(str);
            entries = new Hashtable();
            current = new Entry();
            temp = new StringBuilder();
            tempStart = 0;
            state = ParserState.None;
            this.log = log;
        }
    }
}

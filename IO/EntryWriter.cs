using System;
using System.IO;
using System.Text;

using Dialang.Compilation.Classification;

namespace Dialang.Compilation.IO
{
    public sealed class EntryWriter : IDisposable
    {
        private readonly Stream s;

        #region Buffer Writing

        public void Write(byte[] buffer, int offset, int length)
        {
            s.Write(buffer, offset, length);
        }

        public void Write(ReadOnlySpan<byte> buffer)
        {
            s.Write(buffer);
        }

        #endregion

        #region .NET Types

        public void Write(sbyte x)
        {
            s.WriteByte((byte)x);
        }

        public void Write(byte x)
        {
            s.WriteByte(x);
        }

        public void Write(short x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(ushort x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(int x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(uint x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(long x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(ulong x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(float x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(double x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(bool x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(char x)
        {
            s.Write(BitConverter.GetBytes(x));
        }

        public void Write(string x)
        {
            s.Write(BitConverter.GetBytes(x.Length));
            s.Write(Encoding.UTF8.GetBytes(x));
        }

        public void Write(string x, Encoding enc)
        {
            s.Write(BitConverter.GetBytes(x.Length));
            s.Write(enc.GetBytes(x));
        }

        #endregion

        #region DiaLang Types

        public void Write(Document x)
        {
            foreach (object k in x.Entries.Values)
            {
                Write((Entry)k);
            }
        }

        public void Write(Entry x)
        {
            Write(x.Name);

            Write(x.Scripts.Length);
            foreach (Script l in x.Scripts)
            {
                Write(l);
            }
        }

        public void Write(Script x)
        {
            Write(x.Text);

            Write(x.Events.Length);
            Write(x.Emotes.Length);
            Write(x.Formats.Length);
            Write(x.Pauses.Length);
            Write(x.Choices.Length);

            foreach (Event y in x.Events)
            {
                Write(y);
            }

            foreach (Emote y in x.Emotes)
            {
                Write(y);
            }

            foreach (Format y in x.Formats)
            {
                Write(y);
            }

            foreach (Pause y in x.Pauses)
            {
                Write(y);
            }

            foreach (Choice y in x.Choices)
            {
                Write(y);
            }
        }

        public void Write(Event x)
        {
            Write(x.Name);
            Write(x.Position);
        }

        public void Write(Emote x)
        {
            Write(x.Name);
            Write(x.Position);
        }

        public void Write(Format x)
        {
            Write(x.Type);
            Write(x.Start);
            Write(x.End);
        }

        public void Write(Pause x)
        {
            Write(x.Start);
            Write(x.Length);
        }

        public void Write(Choice x)
        {
            Write(x.Index);
            Write(x.Value);
        }

        #endregion

        public void Dispose()
        {
            s.Dispose();
        }

        public EntryWriter(Stream input)
        {
            s = input;
        }
    }
}

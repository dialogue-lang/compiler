using System;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Script
    {
        public string Text { get; set; }
        public Event[] Events { get; set; }
        public Emote[] Emotes { get; set; }
        public Format[] Formats { get; set; }
        public Pause[] Pauses { get; set; }

        public void Add(Event x)
        {
            Event[] a = Events;
            Array.Resize(ref a, a.Length + 1);
            a[a.Length - 1] = x;
            Events = a; // Probably redundant, but it's always good to be safe.
        }

        public void Add(Emote x)
        {
            Emote[] a = Emotes;
            Array.Resize(ref a, a.Length + 1);
            a[a.Length - 1] = x;
            Emotes = a;
        }

        public void Add(Format x)
        {
            Format[] a = Formats;
            Array.Resize(ref a, a.Length + 1);
            a[a.Length - 1] = x;
            Formats = a;
        }

        public void Add(Pause x)
        {
            Pause[] a = Pauses;
            Array.Resize(ref a, a.Length + 1);
            a[a.Length - 1] = x;
            Pauses = a;
        }

        public Script()
        {
            Text = "Cool test entry text over here!";
            Events = new Event[] { new Event() };
            Emotes = new Emote[] { new Emote() };
            Formats = new Format[] { new Format() };
            Pauses = new Pause[] { new Pause() };
        }

        public Script(string text)
        {
            Text = text;
            Events = new Event[] { new Event() };
            Emotes = new Emote[] { new Emote() };
            Formats = new Format[] { new Format() };
            Pauses = new Pause[] { new Pause() };
        }

        public Script(string text, Event[] events, Emote[] emotes, Format[] formats, Pause[] pauses)
        {
            Text = text;
            Events = events;
            Emotes = emotes;
            Formats = formats;
            Pauses = pauses;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}

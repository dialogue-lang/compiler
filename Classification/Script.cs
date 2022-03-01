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
        public Choice[] Choices { get; set; }
        public Combine[] Combines { get; set; }

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

        public void Add(Choice x)
        {
            Choice[] a = Choices;
            Array.Resize(ref a, a.Length + 1);
            a[a.Length - 1] = x;
            Choices = a;
        }

        public void Add(Combine x)
        {
            Combine[] a = Combines;
            Array.Resize(ref a, a.Length + 1);
            a[a.Length - 1] = x;
            Combines = a;
        }

        public Script()
        {
            Text = "Cool test entry text over here!";
            Events = new Event[0];
            Emotes = new Emote[0];
            Formats = new Format[0];
            Pauses = new Pause[0];
            Choices = new Choice[0];
            Combines = new Combine[0];
        }

        public Script(string text)
        {
            Text = text;
            Events = new Event[0];
            Emotes = new Emote[0];
            Formats = new Format[0];
            Pauses = new Pause[0];
            Choices = new Choice[0];
            Combines = new Combine[0];
        }

        public Script(string text, Event[] events, Emote[] emotes, Format[] formats, Pause[] pauses, Choice[] choices, Combine[] combines)
        {
            Text = text;
            Events = events;
            Emotes = emotes;
            Formats = formats;
            Pauses = pauses;
            Choices = choices;
            Combines = combines;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}

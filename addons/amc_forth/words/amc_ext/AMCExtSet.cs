using System.ComponentModel.Design;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Godot;

// Forth STRING word set

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class AMCExtSet : Godot.RefCounted
    {
        public BlinkV BlinkV;
        public BoldV BoldV;
        public Help Help;
        public HelpS HelpS;
        public HelpWS HelpWS;
        public In In;
        public InAddr InAddr;
        public InvisibleV InvisibleV;
        public Listen Listen;
        public LoadSnap LoadSnap;
        public LowV LowV;
        public NomodeV NomodeV;
        public Out Out;
        public OutAddr OutAddr;
        public PTimer PTimer;
        public PStop PStop;
        public PopXY PopXY;
        public PushXY PushXY;
        public ReverseV ReverseV;
        public SaveSnap SaveSnap;
        public UnderlineV UnderlineV;
        public Unlisten Unlisten;
        private const string Wordset = "AMC EXT";

        public AMCExtSet(AMCForth _forth, Stack stack)
        {
            BlinkV = new(_forth, stack, Wordset);
            BoldV = new(_forth, stack, Wordset);
            Help = new(_forth, stack, Wordset);
            HelpS = new(_forth, stack, Wordset);
            HelpWS = new(_forth, stack, Wordset);
            In = new(_forth, stack, Wordset);
            InAddr = new(_forth, stack, Wordset);
            InvisibleV = new(_forth, stack, Wordset);
            Listen = new(_forth, stack, Wordset);
            LoadSnap = new(_forth, stack, Wordset);
            LowV = new(_forth, stack, Wordset);
            NomodeV = new(_forth, stack, Wordset);
            Out = new(_forth, stack, Wordset);
            OutAddr = new(_forth, stack, Wordset);
            PTimer = new(_forth, stack, Wordset);
            PStop = new(_forth, stack, Wordset);
            PopXY = new(_forth, stack, Wordset);
            PushXY = new(_forth, stack, Wordset);
            ReverseV = new(_forth, stack, Wordset);
            SaveSnap = new(_forth, stack, Wordset);
            UnderlineV = new(_forth, stack, Wordset);
            Unlisten = new(_forth, stack, Wordset);
        }
    }
}

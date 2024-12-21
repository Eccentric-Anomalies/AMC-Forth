using System.Windows.Markup;
using Godot;

// Forth CORE EXT word set

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class CoreExtSet : Godot.RefCounted
    {
        public DotLeftParenthesis DotLeftParenthesis;
        public BackSlash BackSlash;
        public NotEqual NotEqual;
        public ZeroNotEqual ZeroNotEqual;
        public ZeroGreaterThan ZeroGreaterThan;
        public TwoToR TwoToR;
        public TwoRFrom TwoRFrom;
        public TwoRFetch TwoRFetch;
        public Again Again;
        public BufferColon BufferColon;
        public CQuote CQuote;
        public False False;
        public Hex Hex;
        public Marker Marker;
        public Nip Nip;
        public Parse Parse;
        public ParseName ParseName;
        public Pick Pick;
        public SourceId SourceId;
        public To To;
        public True True;
        public Tuck Tuck;
        public ULessThan ULessThan;
        public Unused Unused;
        public Value Value;
        private const string Wordset = "CORE EXT";

        public CoreExtSet(AMCForth _forth, Stack stack)
        {
            DotLeftParenthesis = new(_forth, stack, Wordset);
            BackSlash = new(_forth, stack, Wordset);
            NotEqual = new(_forth, stack, Wordset);
            ZeroNotEqual = new(_forth, stack, Wordset);
            ZeroGreaterThan = new(_forth, stack, Wordset);
            TwoToR = new(_forth, stack, Wordset);
            TwoRFrom = new(_forth, stack, Wordset);
            TwoRFetch = new(_forth, stack, Wordset);
            Again = new(_forth, stack, Wordset);
            BufferColon = new(_forth, stack, Wordset);
            CQuote = new(_forth, stack, Wordset);
            False = new(_forth, stack, Wordset);
            Hex = new(_forth, stack, Wordset);
            Marker = new(_forth, stack, Wordset);
            Nip = new(_forth, stack, Wordset);
            Parse = new(_forth, stack, Wordset);
            ParseName = new(_forth, stack, Wordset);
            Pick = new(_forth, stack, Wordset);
            SourceId = new(_forth, stack, Wordset);
            To = new(_forth, stack, Wordset);
            True = new(_forth, stack, Wordset);
            Tuck = new(_forth, stack, Wordset);
            ULessThan = new(_forth, stack, Wordset);
            Unused = new(_forth, stack, Wordset);
            Value = new(_forth, stack, Wordset);
        }
    }
}

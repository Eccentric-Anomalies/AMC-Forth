using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class BracketNumber : Forth.Words
    {
        public BracketNumber(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "<#";
            Description =
                "Initialize pictured output of an unsigned double-precision "
                + "integer. If the output is signed, a signed value n must be preserved before "
                + "the integer where it may be later passed to SIGN.";
            StackEffect = "( ud - ud ) or ( n ud - n ud )";
        }

        public override void Call()
        {
            // Initialize the NumFormatBuffPointer to the end of the buffer
            Forth.NumFormatBuffPointer = Map.NumFormatBuffer + Map.NumFormatBuffSize;
        }
    }
}

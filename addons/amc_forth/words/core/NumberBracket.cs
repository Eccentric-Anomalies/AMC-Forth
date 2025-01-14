using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class NumberBracket : Forth.Words
    {
        public NumberBracket(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "#>";
            Description =
                "Complete the conversion process after all digits have been converted. "
                + "Discard the (presumably) exhausted double number and leave resulting "
                + "string address and length.";
            StackEffect = "( ud - c-addr u )";
        }

        public override void Call()
        {
            Stack.PopDint();
            Stack.Push(Forth.NumFormatBuffPointer);
            Stack.Push(Map.NumFormatBuffer + Map.NumFormatBuffSize - Forth.NumFormatBuffPointer);
        }
    }
}

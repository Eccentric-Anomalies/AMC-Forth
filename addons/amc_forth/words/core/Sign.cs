using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Sign : Forth.Words
    {
        public Sign(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "SIGN";
            Description =
                "Insert a minus sign at the current position in the string being converted "
                + "if the value of n is negative.";
            StackEffect = "( n -  )";
        }

        public override void Call()
        {
            if (Stack.Pop() < 0)
            {
                Forth.NumFormatBuffPointer--;
                System.Diagnostics.Debug.Assert(Forth.NumFormatBuffPointer >= Map.NumFormatBuffer);
                Forth.Ram.SetByte(Forth.NumFormatBuffPointer, "-".ToAsciiBuffer()[0]);
            }
        }
    }
}

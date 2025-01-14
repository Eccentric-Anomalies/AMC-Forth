using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class NumberSign : Forth.Words
    {
        public NumberSign(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "#";
            Description =
                "Divide ud1 by BASE giving the quotient ud2, and the remainder n. "
                + " Convert n to an ASCII character and prepend it to the beginning "
                + " of the existing output string. Must be used after <# and before "
                + " #>. A character is always generated, even if the digit is zero.";
            StackEffect = "( ud1 - ud2 )";
        }

        public override void Call()
        {
            var numBase = (uint)Forth.Ram.GetInt(Map.Base);
            var fmt = numBase == 10 ? "F0" : "X";
            Forth.NumFormatBuffPointer--;
            System.Diagnostics.Debug.Assert(Forth.NumFormatBuffPointer >= Map.NumFormatBuffer);
            var currVal = Stack.PopDword();
            var rem = currVal % numBase;
            Stack.PushDword(currVal / numBase); // new value
            Forth.Ram.SetByte(Forth.NumFormatBuffPointer, rem.ToString(fmt).ToAsciiBuffer()[0]);
        }
    }
}

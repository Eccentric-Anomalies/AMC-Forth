using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class NumberSignS : Forth.Words
    {
        public NumberSignS(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "#S";
            Description =
                "Convert digits from ud1 until all significant digits in the  "
                + "source item are converted, leaving ud2 (which is zero). "
                + "Always results in at least one putput character, even if the number "
                + "to be converted is zero.";
            StackEffect = "( ud1 - ud2 )";
        }

        public override void Call()
        {
            var numBase = (uint)Forth.Ram.GetInt(Map.Base);
            var fmt = numBase == 10 ? "F0" : "X";
            var currVal = Stack.PopDword();
            do
            {
                Forth.NumFormatBuffPointer--;
                System.Diagnostics.Debug.Assert(Forth.NumFormatBuffPointer >= Map.NumFormatBuffer);
                var rem = currVal % numBase;
                currVal /= numBase; // new value
                Forth.Ram.SetByte(Forth.NumFormatBuffPointer, rem.ToString(fmt).ToAsciiBuffer()[0]);
            } while (currVal != 0);
            Stack.PushDword(currVal);
        }
    }
}

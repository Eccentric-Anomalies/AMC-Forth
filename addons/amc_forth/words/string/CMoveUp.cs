using Godot;
using Godot.Collections;

namespace Forth.String
{
    [GlobalClass]
    public partial class CMoveUp : Forth.Words
    {
        public CMoveUp(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "CMOVE>";
            Description =
                "Copy u characters from addr1 to addr2. The copy proceeds from HIGHER to LOWER addresses.";
            StackEffect = "( addr1 addr2 u - )";
        }

        public override void Call()
        {
            var u = Stack.Pop();
            var a2 = Stack.Pop();
            var a1 = Stack.Pop();
            var i = u;

            // move in descending order a1 -> a2, fast, then slow
            while (i > 0)
            {
                if (i >= RAM.DCellSize)
                {
                    i -= RAM.DCellSize;
                    Forth.Ram.SetDword(a2 + i, Forth.Ram.GetDword(a1 + i));
                }
                else
                {
                    i -= 1;
                    Forth.Ram.SetByte(a2 + i, Forth.Ram.GetByte(a1 + i));
                }
            }
        }
    }
}

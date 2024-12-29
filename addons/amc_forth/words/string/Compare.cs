using Godot;
using Godot.Collections;

namespace Forth.String
{
    [GlobalClass]
    public partial class Compare : Forth.Words
    {
        public Compare(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "COMPARE";
            Description = "If the data-space pointer is not aligned, reserve space to align it.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            var n2 = Stack.Pop();
            var a2 = Stack.Pop();
            var n1 = Stack.Pop();
            var a1 = Stack.Pop();
            var s2 = Forth.Util.StrFromAddrN(a2, n2);
            var s1 = Forth.Util.StrFromAddrN(a1, n1);
            var ret = 0;
            if (s1 == s2)
            {
                Stack.Push(ret);
            }
            else if (System.String.Compare(s1, s2) < 0)
            {
                Stack.Push(-1);
            }
            else
            {
                Stack.Push(1);
            }
        }
    }
}

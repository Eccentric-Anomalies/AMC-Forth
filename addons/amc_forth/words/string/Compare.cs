using System;
using System.Numerics;
using Forth.Core;
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
            Description =
                "Compare string c-addr1 u1 to string c-addr2 u2, returning "
                + "result code n. If strings are identical, return 0. If identical up to "
                + "the end of the shorter string return -1 if u1 < u2, or +1 if u2 < u1. "
                + "Otherwise, if the first non-matching character in c-addr1 is less than "
                + "the match in c-addr2, return -1, and +1 otherwise.";
            StackEffect = "( c-addr1 u1 c-addr2 u2 - n )";
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
            if (s2.Length > s1.Length)
            {
                s2 = s2.Substr(0, s1.Length);
                ret = -1;
            }
            else if (s1.Length > s2.Length)
            {
                s1 = s1.Substr(0, s2.Length);
                ret = 1;
            }
            if (s1 == s2)
            {
                Stack.Push(ret);
                return;
            }
            var bytes1 = s1.ToAsciiBuffer();
            var bytes2 = s2.ToAsciiBuffer();
            for (int i = 0; i < s1.Length; i++)
            {
                var diff = bytes1[i] - bytes2[i];
                if (diff != 0)
                {
                    ret = diff / Math.Abs(diff);
                    break;
                }
            }
            Stack.Push(ret);
        }
    }
}

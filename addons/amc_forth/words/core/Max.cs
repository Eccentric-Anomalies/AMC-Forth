using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Max : Forth.Words
    {
        public Max(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "MAX";
            Description = "Return n3, the greater of n1 and n2.";
            StackEffect = "( n1 n2 - n3 )";
        }

        public override void Call()
        {
            var n2 = Stack.Pop();
            if (n2 > Stack.DataStack[Stack.DsP])
            {
                Stack.DataStack[Stack.DsP] = n2;
            }
        }
    }
}

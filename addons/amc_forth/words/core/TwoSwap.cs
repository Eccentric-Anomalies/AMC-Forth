using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class TwoSwap : Forth.Words
    {
        public TwoSwap(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "2SWAP";
            Description = "Exchange the top two cell pairs.";
            StackEffect = "( x1 x2 x3 x4 - x3 x4 x1 x2 )";
        }

        public override void Call()
        {
            var x2 = Stack.DataStack[Stack.DsP + 2];
            var x1 = Stack.DataStack[Stack.DsP + 3];
            Stack.DataStack[Stack.DsP + 3] = Stack.DataStack[Stack.DsP + 1];
            Stack.DataStack[Stack.DsP + 2] = Stack.DataStack[Stack.DsP];
            Stack.DataStack[Stack.DsP + 1] = x1;
            Stack.DataStack[Stack.DsP] = x2;
        }
    }
}

using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class TwoDup : Forth.Words
    {
        public TwoDup(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2DUP";
            Description = "Duplicate the top cell pair.";
            StackEffect = "(x1 x2 - x1 x2 x1 x2 )";
        }

        public override void Call()
        {
            var x2 = Stack.DataStack[Stack.DsP];
            var x1 = Stack.DataStack[Stack.DsP + 1];
            Stack.Push(x1);
            Stack.Push(x2);
        }
    }
}

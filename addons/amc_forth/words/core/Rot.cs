using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Rot : Forth.Words
    {
        public Rot(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "ROT";
            Description = "Rotate the top three items on the stack.";
            StackEffect = "( x1 x2 x3 - x2 x3 x1 )";
        }

        public override void Call()
        {
            var t = Stack.DataStack[Stack.DsP + 2];
            Stack.DataStack[Stack.DsP + 2] = Stack.DataStack[Stack.DsP + 1];
            Stack.DataStack[Stack.DsP + 1] = Stack.DataStack[Stack.DsP];
            Stack.DataStack[Stack.DsP] = t;
        }
    }
}

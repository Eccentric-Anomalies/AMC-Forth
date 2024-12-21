using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DMinus : Forth.Words
    {
        public DMinus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "D-";
            Description = "Subtract d2 from d1, leaving the difference d3.";
            StackEffect = "( d1 d2 - d3 )";
        }

        public override void Call()
        {
            var t = Stack.PopDint();
            Stack.PushDint(Stack.PopDint() - t);
        }
    }
}

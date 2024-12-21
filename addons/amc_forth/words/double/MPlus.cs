using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class MPlus : Forth.Words
    {
        public MPlus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "M+";
            Description = "Add n to d1 leaving the sum d2.";
            StackEffect = "( d1 n - d2 )";
        }

        public override void Call()
        {
            var n = Stack.Pop();
            Stack.PushDint(Stack.PopDint() * n);
        }
    }
}

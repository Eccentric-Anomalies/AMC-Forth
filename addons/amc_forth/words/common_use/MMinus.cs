using Godot;

namespace Forth.CommonUse
{
    [GlobalClass]
    public partial class MMinus : Forth.Words
    {
        public MMinus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "M-";
            Description = "Subtract n from d1 leaving the difference d2.";
            StackEffect = "( d1 n - d2 )";
        }

        public override void Call()
        {
            var n = Stack.Pop();
            Stack.PushDint(Stack.PopDint() - n);
        }
    }
}

using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class SToD : Forth.Words
    {
        public SToD(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "S>D";
            Description = "Convert a single cell number n to its double equivalent d.";
            StackEffect = "( n - d )";
        }

        public override void Call()
        {
            Stack.PushDint(Stack.Pop());
        }
    }
}

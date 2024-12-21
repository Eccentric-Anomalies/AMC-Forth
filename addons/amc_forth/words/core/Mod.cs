using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Mod : Forth.Words
    {
        public Mod(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "MOD";
            Description = "Divide n1 by n2, giving the remainder n3.";
            StackEffect = "(n1 n2 - n3 )";
        }

        public override void Call()
        {
            var n2 = Stack.Pop();
            Stack.Push(Stack.Pop() % n2);
        }
    }
}

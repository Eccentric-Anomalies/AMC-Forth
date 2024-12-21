using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Fetch : Forth.Words
    {
        public Fetch(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "@";
            Description = "Replace a-addr with the contents of the cell at a_addr.";
            StackEffect = "( a_addr - x )";
        }

        public override void Call()
        {
            Stack.Push(Forth.Ram.GetInt(Stack.Pop()));
        }
    }
}

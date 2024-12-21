using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Store : Forth.Words
    {
        public Store(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "!";
            Description = "Store x in the cell at a-addr.";
            StackEffect = "( x a-addr - )";
        }

        public override void Call()
        {
            var addr = Stack.Pop();
            Forth.Ram.SetInt(addr, Stack.Pop());
        }
    }
}

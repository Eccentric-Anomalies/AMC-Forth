using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Fill : Forth.Words
    {
        public Fill(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "FILL";
            Description = "Fill memory at c_addr, length u, with LSB of b.";
            StackEffect = "( c-addr u b - )";
        }

        public override void Call()
        {
            var b = (byte)Stack.Pop();
            var u = Stack.Pop();
            var addr = Stack.Pop();
            for (int i = 0; i < u; i++)
            {
                Forth.Ram.SetByte(addr + i, b);
            }
        }
    }
}

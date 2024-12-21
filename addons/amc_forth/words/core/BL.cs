using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class BL : Forth.Words
    {
        public BL(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "BL";
            Description = "Return char, the ASCII character value of a space.";
            StackEffect = "( - char )";
        }

        public override void Call()
        {
            Stack.Push(Terminal.BL.ToAsciiBuffer()[0]);
        }
    }
}

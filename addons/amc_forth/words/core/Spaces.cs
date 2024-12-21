using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Spaces : Forth.Words
    {
        public Spaces(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "SPACES";
            Description = "Display u spaces on the current output device.";
            StackEffect = "( u - )";
        }

        public override void Call()
        {
            var u = Stack.Pop();
            for (int i = 0; i < u; i++)
            {
                Forth.Util.PrintTerm(Terminal.BL);
            }
        }
    }
}

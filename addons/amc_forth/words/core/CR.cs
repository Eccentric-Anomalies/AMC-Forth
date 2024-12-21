using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class CR : Forth.Words
    {
        public CR(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CR";
            Description = "Emit characters to generate a newline on the terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.CRLF);
        }
    }
}

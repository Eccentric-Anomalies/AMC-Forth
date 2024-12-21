using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class NomodeV : Forth.Words
    {
        public NomodeV(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "NOMODEV";
            Description = "Send MODESOFF command to video terminal.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.MODESOFF);
        }
    }
}

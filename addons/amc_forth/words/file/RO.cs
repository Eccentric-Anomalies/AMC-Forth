using Godot;

namespace Forth.File
{
    [GlobalClass]
    public partial class RO : Forth.Words
    {
        public RO(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "R/O";
            Description = "Return the read-only file access method.";
            StackEffect = "( - fam )";
        }

        public override void Call()
        {
            Stack.Push((int)FileAccess.ModeFlags.Read);
        }
    }
}

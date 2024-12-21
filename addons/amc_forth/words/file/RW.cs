using Godot;

namespace Forth.File
{
    [GlobalClass]
    public partial class RW : Forth.Words
    {
        public RW(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "R/W";
            Description = "Return the read-write file access method.";
            StackEffect = "( - fam )";
        }

        public override void Call()
        {
            Stack.Push((int)FileAccess.ModeFlags.ReadWrite);
        }
    }
}

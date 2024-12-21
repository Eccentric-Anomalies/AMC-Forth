using Godot;

namespace Forth.File
{
    [GlobalClass]
    public partial class WO : Forth.Words
    {
        public WO(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "W/O";
            Description = "Return the write-only file access method.";
            StackEffect = "( - fam )";
        }

        public override void Call()
        {
            Stack.Push((int)FileAccess.ModeFlags.Write);
        }
    }
}

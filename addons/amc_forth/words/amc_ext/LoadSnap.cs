using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class LoadSnap : Forth.Words
    {
        public LoadSnap(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "LOAD-SNAP";
            Description = "Restore the Forth system RAM from backup file.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.LoadSnapshot();
        }
    }
}

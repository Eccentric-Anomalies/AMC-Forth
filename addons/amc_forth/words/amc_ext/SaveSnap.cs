using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class SaveSnap : Forth.Words
    {
        public SaveSnap(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "SAVE-SNAP";
            Description = "Save the Forth system RAM to backup file.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.SaveSnapshot();
        }
    }
}

using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Begin : Forth.Words
    {
        public Begin(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "BEGIN";
            Description = "Mark the destination of a backward branch.";
            StackEffect = "( - dest )";
            Immediate = true;
        }

        public override void Call()
        {
            // backwards by one cell, so execution will advance it to the right point
            Forth.CfPushDest(Forth.DictTopP - RAM.CellSize);
        }
    }
}

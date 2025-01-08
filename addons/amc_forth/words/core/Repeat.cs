using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Repeat : Forth.Words
    {
        public Repeat(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "REPEAT";
            Description =
                "At compile time, resolve two branches, usually set up by BEGIN and WHILE. "
                + "At run-time, execute the unconditional backward branch to the location "
                + "following BEGIN.";
            StackEffect = "( - )";
            Immediate = true;
        }

        public override void Call()
        {
            // grab and set one (and ONLY one) WHILE origin
            if (Forth.CfIsOrig())
            {
                // account for backward link in next cell
                Forth.Ram.SetInt(Forth.CfPopOrig(), Forth.DictTopP + RAM.CellSize);
            }
            Forth.CoreExtWords.Again.Call();
        }
    }
}

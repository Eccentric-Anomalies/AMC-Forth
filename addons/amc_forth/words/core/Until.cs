using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Until : Forth.Words
    {
        public Until(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "UNTIL";
            Description =
                "Conditionally branch back to the point immediately following "
                + "the nearest previous BEGIN.";
            StackEffect = "( dest x - )";
            Immediate = true;
        }

        public override void Call()
        {
            Forth.Ram.SetInt(Forth.DictTopP, XtX); // copy the execution token
            // The link back
            Forth.Ram.SetInt(Forth.DictTopP + RAM.CellSize, Forth.CfPopDest());
            Forth.DictTopP += RAM.DCellSize; // two cells up and done
        }

        public override void CallExec()
        {
            // ( x - )
            if (Forth.Pop() == 0) // Conditional branch
            {
                Forth.DictIp = Forth.Ram.GetInt(Forth.DictIp + RAM.CellSize);
            }
            else
            {
                // TRUE, so skip over the link and continue executing
                Forth.DictIp += RAM.CellSize;
            }
        }
    }
}

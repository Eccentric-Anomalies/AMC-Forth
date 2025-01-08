using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Loop : Forth.Words
    {
        public Loop(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "LOOP";
            Description =
                "Increment the index value by one and compare to the limit value. "
                + "If they are equal, continue with the next instruction, otherwise "
                + "return to the address of the preceding DO.";
            StackEffect = "( dest orig - )";
            Immediate = true;
        }

        public override void Call()
        {
            Forth.Ram.SetInt(Forth.DictTopP, XtX); // copy the execution token
            // Check for any orig links
            while (!Forth.LcfIsEmpty())
            {
                // destination is on top of the back link
                Forth.Ram.SetInt(Forth.LcfPop(), Forth.DictTopP + RAM.CellSize);
            }
            // The link back
            Forth.Ram.SetInt(Forth.DictTopP + RAM.CellSize, Forth.CfPopDest());
            Forth.DictTopP += RAM.DCellSize;
            // two cells up and done
            // preserve dictionary state
            Forth.SaveDictTop();
        }

        public override void CallExec()
        {
            Forth.CoreExtWords.TwoRFrom.Call(); // Move to data stack.
            Forth.CoreWords.OnePlus.Call(); // Increment the count
            Forth.CoreWords.TwoDup.Call(); // Duplicate them
            Forth.CoreWords.Equal.Call(); // Check for equal
            if (Stack.Pop() == 0)
            {
                // not matched, branch back, placing loop parameters on the return stack
                Forth.CoreExtWords.TwoToR.Call();
                Forth.DictIp = Forth.Ram.GetInt(Forth.DictIp + RAM.CellSize);
            }
            else
            {
                Forth.CoreWords.TwoDrop.Call(); // spare pair of loop parameters is not needed.
                Forth.DictIp += RAM.CellSize; // step ahead over the branch
            }
        }
    }
}

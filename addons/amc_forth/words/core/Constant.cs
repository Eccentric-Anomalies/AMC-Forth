using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Constant : Forth.Words
    {
        public Constant(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CONSTANT";
            Description =
                "Create a dictionary entry for <name>, associated with constant x. "
                + "Executing <name> places the value on the stack. "
                + "Usage: <x> CONSTANT <name>";
            StackEffect = "Compile: ( 'name' x - ), Execute: ( - x )";
        }

        public override void Call()
        {
            var init_val = Stack.Pop();
            if (Forth.CreateDictEntryName() != 0)
            {
                Forth.Ram.SetInt(Forth.DictTopP, XtX); // copy the execution token
                // store the constant
                Forth.Ram.SetInt(Forth.DictTopP + RAM.CellSize, init_val);
                Forth.DictTopP += RAM.DCellSize;
                // two cells up
                // preserve dictionary state
                Forth.SaveDictTop();
            }
        }

        public override void CallExec()
        {
            // return contents of cell after execution token
            Stack.Push(Forth.Ram.GetInt(Forth.DictIp + RAM.CellSize));
        }
    }
}

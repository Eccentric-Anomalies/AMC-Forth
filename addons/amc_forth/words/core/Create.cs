using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Create : Forth.Words
    {
        public Create(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CREATE";
            Description =
                "Construct a dictionary entry for the next token <name> in the input stream. "
                + "Execution of <name> will return the address of its data space.";
            StackEffect = "( 'name' - ), Execute: ( - addr )";
        }

        public override void Call()
        {
            if (Forth.CreateDictEntryName() != 0)
            {
                Forth.Ram.SetInt(Forth.DictTopP, XtX);
                Forth.DictTopP += RAM.CellSize;
                Forth.SaveDictTop(); // preserve dictionary state
            }
        }

        public override void CallExec()
        {
            // return address of cell after execution token
            Stack.Push(Forth.DictIp + RAM.CellSize);
            Forth.DictIp += RAM.CellSize;
        }
    }
}

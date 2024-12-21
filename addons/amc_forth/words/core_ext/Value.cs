using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class Value : Forth.Words
    {
        public Value(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "VALUE";
            Description =
                "Create a dictionary entry for name, associated with value x. "
                + "Usage: <x> VALUE <name>";
            StackEffect = "( 'name' x - )";
        }

        public override void Call()
        {
            var init_val = Forth.Pop();
            if (Forth.CreateDictEntryName() != 0)
            {
                // copy the execution token
                Forth.Ram.SetInt(Forth.DictTopP, XtX);
                // store the initial value
                Forth.Ram.SetInt(Forth.DictTopP + RAM.CellSize, init_val);
                Forth.DictTopP += RAM.DCellSize;
                Forth.SaveDictTop(); // preserve the state
            }
        }

        public override void CallExec()
        {
            // execution time functionality of value
            // return contents of the cell after the execution token
            Forth.Push(Forth.Ram.GetInt(Forth.DictIp + RAM.CellSize));
            Forth.DictIp += RAM.CellSize;
        }
    }
}

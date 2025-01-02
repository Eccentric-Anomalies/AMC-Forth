using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class TwoLiteral : Forth.Words
    {
        public TwoLiteral(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "2LITERAL";
            Description =
                "At compile time, remove the top two numbers from the stack and compile "
                + "into the current definition.";
            StackEffect = "Compile:  ( x x - ), Execute: ( - x x )";
            Immediate = true;
        }

        public override void Call()
        {
            var literal_val1 = Stack.Pop(); // high order, low address
            var literal_val2 = Stack.Pop(); // low order, high address
            // copy the execution token
            Forth.Ram.SetInt(Forth.DictTopP, XtX);
            // store the value
            Forth.Ram.SetInt(Forth.DictTopP + RAM.CellSize, literal_val1);
            Forth.Ram.SetInt(Forth.DictTopP + RAM.DCellSize, literal_val2);
            Forth.DictTopP += RAM.CellSize * 3; // three cells up
            // preserve dictionary state
            Forth.SaveDictTop();
        }

        public override void CallExec()
        {
            // execution time functionality of literal
            // return contents of cell after execution token
            Stack.Push(Forth.Ram.GetInt(Forth.DictIp + RAM.DCellSize)); // low order
            Stack.Push(Forth.Ram.GetInt(Forth.DictIp + RAM.CellSize)); // high order
            // advance the instruction pointer by two to skip over the data
            Forth.DictIp += RAM.DCellSize;
        }
    }
}

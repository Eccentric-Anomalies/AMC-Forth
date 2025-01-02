using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Comma : Forth.Words
    {
        public Comma(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = ",";
            Description = "Reserve one cell of data space and store x in it.";
            StackEffect = "( x - )";
        }

        public override void Call()
        {
            Forth.Ram.SetInt(Forth.DictTopP, Stack.Pop());
            Forth.DictTopP += RAM.CellSize;
            Forth.SaveDictTop(); // preserve dictionary state
        }
    }
}

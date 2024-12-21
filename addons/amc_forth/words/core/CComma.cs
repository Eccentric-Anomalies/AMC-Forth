using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class CComma : Forth.Words
    {
        public CComma(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "C,";
            Description = "Reserve one byte of data space and store char in the byte.";
            StackEffect = "( char - )";
        }

        public override void Call()
        {
            Forth.Ram.SetByte(Forth.DictTopP, Stack.Pop());
            Forth.DictTopP += 1;
            Forth.SaveDictTop(); // preserve dictionary state
        }
    }
}

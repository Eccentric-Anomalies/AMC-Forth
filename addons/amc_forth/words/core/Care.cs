using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Care : Forth.Words
    {
        public Care(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "CHAR";
            Description =
                "Parse the following word and put the ASCII value of its "
                + "first character on the stack. See [CHAR] for a compiled version.";
            StackEffect = "( - char )";
        }

        public override void Call()
        {
            Forth.CoreExtWords.ParseName.Call(); // get c-addr u
            Forth.CoreWords.Drop.Call();
            Stack.Push(Forth.Ram.GetByte(Stack.Pop())); // the first character byte
        }
    }
}

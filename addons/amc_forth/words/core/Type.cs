using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Type : Forth.Words
    {
        public Type(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "TYPE";
            Description = "Output the character string at c-addr, length u.";
            StackEffect = "( c-addr u - )";
        }

        public override void Call()
        {
            var l = Stack.Pop();
            var s = Stack.Pop();
            for (int i = 0; i < l; i++)
            {
                Stack.Push(Forth.Ram.GetByte(s + i));
                Forth.CoreWords.Emit.Call();
            }
        }
    }
}

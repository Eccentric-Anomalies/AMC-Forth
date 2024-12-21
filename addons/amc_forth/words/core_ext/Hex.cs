using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class Hex : Forth.Words
    {
        public Hex(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "HEX";
            Description = "Sets BASE to 16.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Stack.Push(16);
            Forth.CoreWords.Base.Call();
            Forth.CoreWords.Store.Call();
        }
    }
}

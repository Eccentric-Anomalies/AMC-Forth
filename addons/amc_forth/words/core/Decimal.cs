using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Decimal : Forth.Words
    {
        public Decimal(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "DECIMAL";
            Description = "Sets BASE to 10.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Stack.Push(10);
            Forth.CoreWords.Base.Call();
            Forth.CoreWords.Store.Call();
        }
    }
}

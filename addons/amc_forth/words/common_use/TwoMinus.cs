using Godot;

namespace Forth.CommonUse
{
    [GlobalClass]
    public partial class TwoMinus : Forth.Words
    {
        public TwoMinus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2-";
            Description = "Subtract two from n1, leaving n2.";
            StackEffect = "( n1 - n2 )";
        }

        public override void Call()
        {
            Stack.Push(2);
            Forth.CoreWords.Minus.Call();
        }
    }
}

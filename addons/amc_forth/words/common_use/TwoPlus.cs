using Godot;

namespace Forth.CommonUse
{
    [GlobalClass]
    public partial class TwoPlus : Forth.Words
    {
        public TwoPlus(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "2+";
            Description = "Add two to n1, leaving n2.";
            StackEffect = "( n1 - n2 )";
        }

        public override void Call()
        {
            Stack.Push(2);
            Forth.CoreWords.Plus.Call();
        }
    }
}

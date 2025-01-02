using Godot;

namespace Forth.DoubleExt
{
    [GlobalClass]
    public partial class TwoRot : Forth.Words
    {
        public TwoRot(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "2ROT";
            Description = "Rotate the top three cell pairs on the stack.";
            StackEffect = "( x1 x2 x3 x4 x5 x6 - x3 x4 x5 x6 x1 x2 )";
        }

        public override void Call()
        {
            var t = Stack.GetDint(4);
            Stack.SetDint(4, Stack.GetDint(2));
            Stack.SetDint(2, Stack.GetDint(0));
            Stack.SetDint(0, t);
        }
    }
}

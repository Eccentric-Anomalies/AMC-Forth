using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DZeroEqual : Forth.Words
    {
        public DZeroEqual(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "D0=";
            Description =
                "Return true if and only if the double precision value d is equal to zero.";
            StackEffect = "( d - flag )";
        }

        public override void Call()
        {
            if (Stack.PopDint() == 0)
            {
                Stack.Push(AMCForth.True);
            }
            else
            {
                Stack.Push(AMCForth.False);
            }
        }
    }
}

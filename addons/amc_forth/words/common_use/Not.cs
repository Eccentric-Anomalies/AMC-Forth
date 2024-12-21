using Godot;

namespace Forth.CommonUse
{
    [GlobalClass]
    public partial class Not : Forth.Words
    {
        public Not(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "NOT";
            Description = "Identical to 0=, used for program clarity to reverse logical result.";
            StackEffect = "( x - flag )";
        }

        public override void Call()
        {
            Forth.CoreWords.ZeroEqual.Call();
        }
    }
}

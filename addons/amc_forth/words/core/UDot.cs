using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class UDot : Forth.Words
    {
        public UDot(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "U.";
            Description = "Display the value of unsigned u, on the top of the stack.";
            StackEffect = "( u - )";
        }

        public override void Call()
        {
            var fmt = Forth.Ram.GetInt(AMCForth.Base) == 10 ? "F0" : "X";
            var num = (uint)Forth.Pop();
            Forth.Util.PrintTerm(" " + num.ToString(fmt));
        }
    }
}

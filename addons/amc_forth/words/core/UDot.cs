using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class UDot : Forth.Words
    {
        public UDot(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Salt = "UDot";
            Name = "U.";
            Description = "Display the value of unsigned u, on the top of the stack.";
            StackEffect = "( u - )";
        }

        public override void Call()
        {
            var fmt = Forth.Ram.GetInt(Map.Base) == 10 ? "F0" : "X";
            var num = (uint)Stack.Pop();
            Forth.Util.PrintTerm(" " + num.ToString(fmt));
        }
    }
}

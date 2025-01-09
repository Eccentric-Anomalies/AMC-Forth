using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class UDotR : Forth.Words
    {
        public UDotR(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "U.R";
            Description =
                "Display the unsigned integer u with enough leading "
                + "spaces to fill a field of width +n.";
            StackEffect = "( u +n - )";
        }

        public override void Call()
        {
            var fmt = Forth.Ram.GetInt(Map.Base) == 10 ? "F0" : "X";
            var spaces = Stack.Pop();
            var str = ((uint)Stack.Pop()).ToString(fmt);
            spaces = str.Length > spaces ? 0 : spaces - str.Length;
            Forth.Util.PrintTerm(new string(Terminal.BL.ToCharArray()[0], spaces) + str + " ");
        }
    }
}

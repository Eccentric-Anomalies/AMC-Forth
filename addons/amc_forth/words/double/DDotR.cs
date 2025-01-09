using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DDotR : Forth.Words
    {
        public DDotR(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "D.R";
            Description =
                "Display the double integer d with enough leading "
                + "spaces to fill a field of width +n.";
            StackEffect = "( d +n - )";
        }

        public override void Call()
        {
            var fmt = Forth.Ram.GetInt(Map.Base) == 10 ? "F0" : "X";
            var spaces = Stack.Pop();
            var str = Stack.PopDint().ToString(fmt);
            spaces = str.Length > spaces ? 0 : spaces - str.Length;
            Forth.Util.PrintTerm(new string(Terminal.BL.ToCharArray()[0], spaces) + str + " ");
        }
    }
}

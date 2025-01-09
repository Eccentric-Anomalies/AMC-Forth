using Godot;

namespace Forth.CoreExt
{
    [GlobalClass]
    public partial class DotR : Forth.Words
    {
        public DotR(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = ".R";
            Description =
                "Display the signed integer n1 with enough leading "
                + "spaces to fill a field of width +n.";
            StackEffect = "( n1 +n - )";
        }

        public override void Call()
        {
            var fmt = Forth.Ram.GetInt(Map.Base) == 10 ? "F0" : "X";
            var spaces = Stack.Pop();
            var str = Stack.Pop().ToString(fmt);
            spaces = str.Length > spaces ? 0 : spaces - str.Length;
            Forth.Util.PrintTerm(new string(Terminal.BL.ToCharArray()[0], spaces) + str + " ");
        }
    }
}

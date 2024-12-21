using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class PushXY : Forth.Words
    {
        public PushXY(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "PUSH-XY";
            Description =
                "Tell the output device to save its current output position, to "
                + "be retrieved later using POP-XY.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            Forth.Util.PrintTerm(Terminal.ESC + "7");
        }
    }
}

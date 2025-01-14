using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Hold : Forth.Words
    {
        public Hold(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "HOLD";
            Description =
                "While constructing a pictured numeric output string, insert "
                + "char at the current position. HOLD must only occur inside a number "
                + "conversion sequence.";
            StackEffect = "( char - )";
        }

        public override void Call()
        {
            Forth.NumFormatBuffPointer--;
            System.Diagnostics.Debug.Assert(Forth.NumFormatBuffPointer >= Map.NumFormatBuffer);
            Forth.Ram.SetByte(Forth.NumFormatBuffPointer, (byte)Stack.Pop());
        }
    }
}

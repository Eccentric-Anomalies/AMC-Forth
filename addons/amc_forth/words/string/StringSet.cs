using Godot;

// Forth STRING word set

namespace Forth.String
{
    [GlobalClass]
    public partial class StringSet : Godot.RefCounted
    {
        public Compare Compare;
        public CMove CMove;
        public CMoveUp CMoveUp;
        private const string Wordset = "STRING";

        public StringSet(AMCForth _forth, Stack stack)
        {
            Compare = new(_forth, stack, Wordset);
            CMove = new(_forth, stack, Wordset);
            CMoveUp = new(_forth, stack, Wordset);
        }
    }
}

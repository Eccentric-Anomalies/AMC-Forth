using System;
using Godot;

// Forth COMMON USE word set

namespace Forth.CommonUse
{
    [GlobalClass]
    public partial class CommonUseSet : Godot.RefCounted
    {
        public TwoPlus TwoPlus;
        public TwoMinus TwoMinus;
        public MMinus MMinus;
        public MSlash MSlash;
        public Not Not;
        public NumberQuestion NumberQuestion;

        private const string Wordset = "COMMON USE";

        public CommonUseSet(AMCForth _forth, Stack stack)
        {
            TwoPlus = new(_forth, stack, Wordset);
            TwoMinus = new(_forth, stack, Wordset);
            MMinus = new(_forth, stack, Wordset);
            MSlash = new(_forth, stack, Wordset);
            Not = new(_forth, stack, Wordset);
            NumberQuestion = new(_forth, stack, Wordset);
        }
    }
}

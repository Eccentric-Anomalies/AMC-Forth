using System;
using Godot;

namespace Forth.Double
{
    [GlobalClass]
    public partial class DAbs : Forth.Words
    {
        public DAbs(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "DABS";
            Description = "Replace the top stack double item with its absolute value.";
            StackEffect = "( d - +d )";
        }

        public override void Call()
        {
            Stack.SetDint(0, Math.Abs(Stack.GetDint(0)));
        }
    }
}

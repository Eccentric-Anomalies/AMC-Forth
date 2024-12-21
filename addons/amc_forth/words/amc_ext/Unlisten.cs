using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class Unlisten : Forth.Words
    {
        public Unlisten(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "UNLISTEN";
            Description = "Remove a lookup entry for the IO port p.";
            StackEffect = "( p - )";
        }

        public override void Call()
        {
            var p = Stack.Pop(); // port number
            Stack.Push(0);
            Stack.Push(Map.IoInMapStart + p * 2 * RAM.CellSize); // address of xt
            Forth.CoreWords.Store.Call(); // store the XT
            Stack.Push(0);
            Stack.Push(Map.IoInMapStart + RAM.CellSize * (p * 2 + 1)); // address of q mode
            Forth.CoreWords.Store.Call(); // store the Q mode
        }
    }
}

using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class Listen : Forth.Words
    {
        public Listen(AMCForth forth, Stack stack, string wordset)
            : base(forth, stack, wordset)
        {
            Name = "LISTEN";
            Description =
                "Add a lookup entry for the IO port p, to execute 'word'. "
                + "Events to port p are enqueued with q mode (0, 1, 2), "
                + "where q = enqueue: 0 - always, 1 - if new value, 2 - replace all prior. "
                + "Note: An input port may have only one handler word.";
            StackEffect = "( 'word' p q - )";
        }

        public override void Call()
        {
            // Store the queue mode
            var q = Stack.Pop(); // queue mode
            var p = Stack.Pop(); // port number
            Forth.CoreWords.Tick.Call(); // retrieve XT for the handler (on stack)
            Stack.Push(Map.IoInMapStart + p * 2 * RAM.CellSize); // address of xt
            Forth.CoreWords.Store.Call(); // store the XT
            Stack.Push(q); // q mode
            Stack.Push(Map.IoInMapStart + RAM.CellSize * (p * 2 + 1)); // address of q mode
            Forth.CoreWords.Store.Call(); // store the Q mode
        }
    }
}

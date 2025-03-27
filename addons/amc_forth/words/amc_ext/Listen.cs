using Godot;

namespace Forth.AMCExt
{
    [GlobalClass]
    public partial class Listen : Forth.Words
    {
        public Listen(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "LISTEN";
            Description =
                "Add a lookup entry for the IO port p, to execute 'word'. "
                + "Events to port p are enqueued with q mode (0, 1, 2), "
                + "where q = enqueue: 0 - always, 1 - if new value, 2 - replace all prior. "
                + "LISTEN should not be used inside a colon definition, unless <name> is "
                + "provided following invocation of the definition. "
                + "Note: An input port may have only one handler word.";
            StackEffect = "( 'word' p q - )";
        }

        public override void Call()
        {
            // Get the XT from the following word
            Forth.CoreWords.Tick.Call(); // retrieve XT for the handler ( p q xt )
            // Set up to call LISTENX
            Forth.CoreWords.Rot.Call(); // ( q xt p )
            Forth.CoreWords.Rot.Call(); // ( xt p q )
            Forth.AMCExtWords.ListenX.Call();
        }
    }
}

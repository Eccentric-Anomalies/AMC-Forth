using System;
using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class BracketTick : Forth.Words
    {
        public BracketTick(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "[']";
            Description =
                "Used in a definition, finds the name and compiles its "
                + "execution token as a literal to be pushed on the stack "
                + "when the definition in which it appears is executed.";
            StackEffect = "( 'name' - xt )";
            Immediate = true;
        }

        public override void Call()
        {
            // retrieve the name token
            Forth.CoreExtWords.ParseName.Call();
            var len = Stack.Pop(); // length
            var caddr = Stack.Pop(); // start
            var word = Forth.Util.StrFromAddrN(caddr, len);
            var token = Forth.FindInDict(word).Addr; // look the name up
            if (token == 0) // not in dictionary, a built-in xt, or neither?
            {
                try
                {
                    // copy the execution token
                    token = Forth.BuiltinFromName(word).Xt;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Forth.Util.PrintUnknownWord(e.ParamName);
                }
            }
            // copy ['] execution token
            Forth.Ram.SetInt(Forth.DictTopP, XtX);
            Forth.DictTopP += RAM.CellSize;
            Forth.Ram.SetInt(Forth.DictTopP, token);
            Forth.DictTopP += RAM.CellSize;
        }

        public override void CallExec()
        {
            // execution time functionality of literal
            // return contents of cell after execution token
            Stack.Push(Forth.Ram.GetInt(Forth.DictIp + RAM.CellSize));
            // advance the instruction pointer by one to skip over the data
            Forth.DictIp += RAM.CellSize;
        }
    }
}

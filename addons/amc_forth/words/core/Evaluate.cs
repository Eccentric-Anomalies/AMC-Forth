using System;
using Godot;

namespace Forth.Core
{
    [GlobalClass]
    public partial class Evaluate : Forth.Words
    {
        public Evaluate(AMCForth forth, string wordset)
            : base(forth, wordset)
        {
            Name = "EVALUATE";
            Description =
                "Evaluate Forth code from SOURCE buffer. Unlike Standard Forth,"
                + " EVALUATE is for AMCForth internal use only and may not be called from"
                + " user code.";
            StackEffect = "( - )";
        }

        public override void Call()
        {
            // buffer pointer is based on source-id
            ResetBuffToIn();
            while (true)
            {
                Forth.CoreExtWords.ParseName.Call();
                var len = Stack.Pop(); // length of word
                var caddr = Stack.Pop(); // start of word
                if (len == 0) // out of tokens?
                {
                    break;
                }
                var t = Forth.Util.StrFromAddrN(caddr, len);
                // t should be the next token, try to get an execution token from it
                var xt_immediate = Forth.FindInDict(t);
                if ((xt_immediate.Addr == 0) && HasName(t.ToUpper()))
                {
                    // token is not in the dictionary
                    t = t.ToUpper(); // use upper case for searching built-ins
                    try
                    {
                        xt_immediate = new AMCForth.DictResult(Forth.BuiltinFromName(t).Xt, false);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        xt_immediate = new AMCForth.DictResult(0, false);
                        Forth.Util.PrintUnknownWord(e.ParamName);
                    }
                }
                if (xt_immediate.Addr != 0) // an execution token exists
                {
                    Stack.Push(xt_immediate.Addr);
                    // check if it is a built-in immediate or dictionary immediate before storing
                    if (
                        Forth.State
                        && !(
                            (HasName(t) && Forth.BuiltinFromName(t).Immediate)
                            || xt_immediate.IsImmediate
                        )
                    )
                    {
                        // Compiling
                        Forth.CoreWords.Comma.Call();
                    }
                    // store at the top of the current : definition
                    else
                    {
                        // Not Compiling or immediate - just execute
                        Forth.CoreWords.Execute.Call();
                    }
                }
                else // no valid token, so maybe valid numeric value (double first)
                {
                    // check for a number
                    Stack.Push(caddr);
                    Stack.Push(len);
                    Forth.CommonUseWords.NumberQuestion.Call();
                    var type = Stack.Pop();
                    if (type == 2 && Forth.State)
                    {
                        Forth.DoubleWords.TwoLiteral.Call();
                    }
                    else if (type == 1 && Forth.State)
                    {
                        Forth.CoreWords.Literal.Call();
                    }
                    else if (type == 0)
                    {
                        Forth.Util.PrintUnknownWord(t);

                        // do some clean up if we were compiling
                        Forth.UnwindCompile();
                        break;
                        // not ok
                    }
                } // check the stack at each step..
                if (Stack.DsP < 0)
                {
                    Forth.Util.RprintTerm(" Data stack overflow");
                    Stack.DsP = Stack.DataStackSize;
                    break;
                }
                // not ok
                if (Stack.DsP > Stack.DataStackSize)
                {
                    Forth.Util.RprintTerm(" Data stack underflow");
                    Stack.DsP = Stack.DataStackSize;
                    break;
                }
            }
        }

        public void ResetBuffToIn()
        {
            // retrieve the address of the current buffer pointer
            Forth.CoreWords.ToIn.Call();
            // and set its contents to zero
            Forth.Ram.SetInt(Stack.Pop(), 0);
        }
    }
}

// Forth Data and Return Stacks and Utility Methods

using System;
using Godot;

namespace Forth
{
    public partial class Stack : Godot.RefCounted
    {
        public const int DataStackSize = 100;
        public const int DataStackTop = DataStackSize - 1;

        public const int ReturnStackSize = 100;

        public int[] DataStack = new int[DataStackSize];
        public int DsP;

        // Forth: return stack
        public int[] ReturnStack = new int[ReturnStackSize];
        public int RsP;

        //# Create with a reference to AMCForth
        protected AMCForth Forth;

        public void Initialize(AMCForth forth)
        {
            Forth = forth;
            // Initialize the data stack pointer
            DsP = DataStackSize;

            // Initialize the return stack pointer
            RsP = ReturnStackSize;
        }

        // Forth Data Stack Push and Pop Routines

        public void Push(int val)
        {
            DsP -= 1;
            DataStack[DsP] = val;
        }

        public int Pop()
        {
            if (DsP < DataStackSize)
            {
                DsP += 1;
                return DataStack[DsP - 1];
            }
            Forth.Util.RprintTerm(" Data stack underflow");
            return 0;
        }

        public void PushDint(long val)
        {
            var t = RAM.Split64(val);
            Push(t.Lo);
            Push(t.Hi);
        }

        public long PopDint()
        {
            return RAM.Combine64(Pop(), Pop());
        }

        // Forth Return Stack Push and Pop Routines

        public void RPush(int val)
        {
            RsP -= 1;
            ReturnStack[RsP] = val;
        }

        public int RPop()
        {
            if (RsP < ReturnStackSize)
            {
                RsP += 1;
                return ReturnStack[RsP - 1];
            }
            Forth.Util.RprintTerm(" Return stack underflow");
            return 0;
        }

        public void RPushDint(long val)
        {
            var t = RAM.Split64(val);
            RPush(t.Lo);
            RPush(t.Hi);
        }

        public long RPopDint()
        {
            return RAM.Combine64(RPop(), RPop());
        }

        // top of stack is 0, next dint is at 2, etc.
        public long GetDint(int index)
        {
            return RAM.Combine64(DataStack[DsP + index], DataStack[DsP + index + 1]);
        }

        public void SetDint(int index, long value)
        {
            var s = RAM.Split64(value);
            DataStack[DsP + index] = s.Hi;
            DataStack[DsP + index + 1] = s.Lo;
        }

        public void PushDword(ulong value)
        {
            var s = RAM.Split64((long)value);
            Push(s.Lo);
            Push(s.Hi);
        }

        public void SetDword(int index, ulong value)
        {
            var s = RAM.Split64((int)value);
            DataStack[DsP + index] = s.Hi;
            DataStack[DsP + index + 1] = s.Lo;
        }

        public ulong PopDword()
        {
            return (ulong)RAM.Combine64(Pop(), Pop());
        }

        // top of stack is -1, next dint is at -3, etc.
        public ulong GetDword(int index)
        {
            return (ulong)RAM.Combine64(DataStack[DsP + index], DataStack[DsP + index + 1]);
        }
    }
}

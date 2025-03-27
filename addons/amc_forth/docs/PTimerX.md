# P-TIMERX &emsp; (PTimerX)
Start a periodic timer with execution token xt, id i, and interval n (msec), with xt to be executed on timer expiration. Does nothing if the id is in use. Usage: <id> <msec> P-TIMER <name>. Note: Timeouts less than 50 msec will suffer from long-term timing drift. Each timeout 50 msec or greater may be slightly inaccurate, but will average to the correct period with no long-term drift.
* ( xt i n - )
* [Source Code](../words/amc_ext/PTimerX.cs)
* Execution Tokens: 1099446651 (interpreted) and 562575739 (compiled)


[BACK](builtins.md#PTimerX)

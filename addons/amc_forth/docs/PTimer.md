# P-TIMER &emsp; (PTimer)
Start a periodic timer with id i, and interval n (msec) that calls execution token given by <name>. Does nothing if the id is in use. Usage: <id> <msec> P-TIMER <name>. Note: Timeouts less than 50 msec will suffer from long-term timing drift. Each timeout 50 msec or greater may be slightly inaccurate, but will average to the correct period with no long-term drift. P-TIMER should not be used inside a colon definition, unless <name> is provided following the invocation of the definition.
* ( 'name' i n - )
* [Source Code](../words/amc_ext/PTimer.cs)
* Execution Tokens: 1090789571 (interpreted) and 553918659 (compiled)


[BACK](builtins.md#PTimer)

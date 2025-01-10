# COMPARE &emsp; (Compare)
Compare string c-addr1 u1 to string c-addr2 u2, returning result code n. If strings are identical, return 0. If identical up to the end of the shorter string return -1 if u1 < u2, or +1 if u2 < u1. Otherwise, if the first non-matching character in c-addr1 is less than the match in c-addr2, return -1, and +1 otherwise.
* ( c-addr1 u1 c-addr2 u2 - n )
* [Source Code](../words/string/Compare.cs)
* Execution Tokens: 1193503244 (interpreted) and 656632332 (compiled)


[BACK](builtins.md#Compare)

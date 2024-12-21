# LISTEN &emsp; (Listen)
Add a lookup entry for the IO port p, to execute 'word'. Events to port p are enqueued with q mode (0, 1, 2), where q = enqueue: 0 - always, 1 - if new value, 2 - replace all prior. Note: An input port may have only one handler word.
* ( 'word' p q - )
* [Source Code](../words/amc_ext/Listen.cs)
* Execution Tokens: 1216969799 (interpreted) and 680098887 (compiled)


[BACK](builtins.md#Listen)

# LISTENX &emsp; (ListenX)
Add a lookup entry for the IO port p, to execute xt. Events to port p are enqueued with q mode (0, 1, 2), where q = enqueue: 0 - always, 1 - if new value, 2 - replace all prior. Note: An input port may have only one handler word.
* ( xt p q - )
* [Source Code](../words/amc_ext/ListenX.cs)
* Execution Tokens: 1314876972 (interpreted) and 778006060 (compiled)


[BACK](builtins.md#ListenX)

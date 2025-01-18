( === CLOCK.FTH === )

( === PERIODIC TIMER HANDLERS === )
( MSEC TICK COUNT )
( TICK STORAGE )
2VARIABLE MSEC-COUNT
10 CONSTANT MSEC-INTERVAL
( TICK HANDLER )
: HANDLE-TICK-MSEC 
    MSEC-COUNT 2@   ( fetch the dword msec count )
    MSEC-INTERVAL   ( fetch the msec period )
    M+              ( add them )
    MSEC-COUNT 2!   ( update the dword count )
    ;

( SECONDS COMPUTATION AND DISPLAY )
( SECONDS STORAGE )
VARIABLE SEC-COUNT
: COMPUTE-SECONDS
    MSEC-COUNT 2@   ( fetch the dword msec count )
    1000 M/         ( divide by msec per sec )
    SEC-COUNT !     ( store it )
    ;

( SECONDS DISPLAY )
: DISPLAY-SECONDS
    PUSH-XY         ( save display cursor )
    35 1 AT-XY      ( position near top center )
    SEC-COUNT @     ( retrieve seconds count )
    10 U.R          ( display in 10 character field )
    POP-XY          ( restore display cursor )
    ;

( ONE SECOND TICK HANDLER )
: HANDLE-TICK-SEC
    COMPUTE-SECONDS ( update the seconds count )
    DISPLAY-SECONDS ( display the seconds count)
    ;

( === PERIODIC TIMER STARTS === )
( Create periodic tick at MSEC-INTERVAL rate)
1 MSEC-INTERVAL P-TIMER HANDLE-TICK-MSEC
( Create periodic 1 second tick )
2 100 P-TIMER HANDLE-TICK-SEC
.( WORDS TESTED FOR AMCFORTH) CR
\ Test tools for AMC Forth. Derived from the following:

\ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS LABORATORY
\ MAY BE DISTRIBUTED FREELY AS LONG AS THIS COPYRIGHT NOTICE REMAINS.
\ VERSION 1.1

\ Revision history and possibly newer versions can be found at
\ http://www.forth200x/tests/ttester.fs

MARKER CLEANUP    \ To restore original dictionary

VARIABLE ACTUAL-DEPTH \ stack record
CREATE ACTUAL-RESULTS 20 CELLS ALLOT
VARIABLE START-DEPTH
VARIABLE XCURSOR \ for ...}T
VARIABLE ERROR-XT

: ERROR ERROR-XT @ EXECUTE ; \ for vectoring of error reporting

' TYPE ERROR-XT !

: T{ \ ( -- ) record the pre-test depth.
   DEPTH START-DEPTH ! 0 XCURSOR ! ;

: -> \ ( ... -- ) record depth and contents of stack.
   DEPTH DUP ACTUAL-DEPTH ! \ record depth
   START-DEPTH @ > IF       \ if there is something on the stack
     DEPTH START-DEPTH @ - 0 DO \ save them
       ACTUAL-RESULTS I CELLS + !
     LOOP
   THEN ;

: }T \ ( ... -- ) compare stack (expected) contents with saved
   \ (actual) contents.
   DEPTH ACTUAL-DEPTH @ = IF           \ if depths match
     DEPTH START-DEPTH @ > IF          \ if something on the stack
       DEPTH START-DEPTH @ - 0 DO      \ for each stack item
         ACTUAL-RESULTS I CELLS + @    \ compare actual with expected
         <> IF S" INCORRECT RESULT: " ERROR LEAVE THEN
       LOOP
     THEN
   ELSE                                    \ depth mismatch
     S" WRONG NUMBER OF RESULTS: " ERROR
   THEN ;

\ NECESSARY DEFINITIONS

0 CONSTANT 0S   \ all 0 bits
0S INVERT CONSTANT 1S   \ all 1 bits

0 INVERT CONSTANT MAX-UINT
0 INVERT 1 RSHIFT CONSTANT MAX-INT
0 INVERT 1 RSHIFT INVERT   CONSTANT MIN-INT
0 INVERT 1 RSHIFT CONSTANT MID-UINT
0 INVERT 1 RSHIFT INVERT   CONSTANT MID-UINT+1

0 CONSTANT <FALSE>
-1 CONSTANT <TRUE>

1S 1 RSHIFT INVERT CONSTANT MSB

HERE 3 C, CHAR G C, CHAR T C, CHAR 1 C, CONSTANT GT1STRING
HERE 3 C, CHAR G C, CHAR T C, CHAR 2 C, CONSTANT GT2STRING 

\ MEMORY MOVEMENT
CREATE FBUF 00 C, 00 C, 00 C,
CREATE SBUF 12 C, 34 C, 56 C,
: SEEBUF FBUF C@ FBUF CHAR+ C@ FBUF CHAR+ CHAR+ C@ ; 

\ DIVISION
: IFFLOORED [ -3 2 / -2 = INVERT ] LITERAL IF POSTPONE \ THEN ;
: IFSYM      [ -3 2 / -1 = INVERT ] LITERAL IF POSTPONE \ THEN ; 

\ CORE TESTS

 T{ -> }T                      ( Start with a clean slate )
( Test if any bits are set; Answer in base 1 )
T{ : BITSSET? IF 0 0 ELSE 0 THEN ; -> }T
T{  0 BITSSET? -> 0 }T           ( Zero is all bits clear )
T{  1 BITSSET? -> 0 0 }T         ( Other numbers have at least one bit )
T{ -1 BITSSET? -> 0 0 }T 

DECIMAL
16 CONSTANT MAX-BASE
: COUNT-BITS
   0 0 INVERT BEGIN DUP WHILE >R 1+ R> 2* REPEAT DROP ;
COUNT-BITS 2* CONSTANT #BITS-UD    \ NUMBER OF BITS IN UD 


CR 
.( *** CORE WORDS ***) CR
CR 

.( NUMBER SIGN and NUMBER BRACKET and BRACKET NUMBER --> OK IF BLANK \/) CR
: GP3 <# 1 0 # # #> S" 01" COMPARE ;
T{ GP3 -> 0 }T

.( NUMBER SIGN S --> OK IF BLANK \/) CR
: GP4 <# 1 0 #S #> S" 1" COMPARE ;
T{ GP4 -> 0 }T
: GP5
   BASE @ 0
   MAX-BASE 1+ 2 DO     \ FOR EACH POSSIBLE BASE
     I BASE !             \ TBD: ASSUMES BASE WORKS
       I 0 <# #S #> S" 10" COMPARE OR
   LOOP
   SWAP BASE ! ;
T{ GP5 -> 0 }T 
: GP6
   BASE @ >R 2 BASE !
   MAX-UINT MAX-UINT <# #S #>   \ MAXIMUM UD TO BINARY
   R> BASE !                       \ S: C-ADDR U
   DUP #BITS-UD = SWAP
   0 DO                             \ S: C-ADDR FLAG
     OVER C@ [CHAR] 1 = AND    \ ALL ONES
     >R CHAR+ R>
   LOOP SWAP DROP ;
T{ GP6 -> <TRUE> }T 


.( HOLD --> OK IF BLANK \/) CR
HEX
: GP1 <# 41 HOLD 42 HOLD 0 0 #> S" BA" COMPARE ;
T{ GP1 -> 0 }T

DECIMAL
.( SIGN --> OK IF BLANK \/) CR
: GP2 <# -1 SIGN 0 SIGN -1 SIGN 0 0 #> S" --" COMPARE ;
T{ GP2 -> 0 }T

.( PLUS --> OK IF BLANK \/) CR
T{        0  5 + ->          5 }T
T{        5  0 + ->          5 }T
T{        0 -5 + ->         -5 }T
T{       -5  0 + ->         -5 }T
T{        1  2 + ->          3 }T
T{        1 -2 + ->         -1 }T
T{       -1  2 + ->          1 }T
T{       -1 -2 + ->         -3 }T
T{       -1  1 + ->          0 }T
T{ MID-UINT  1 + -> MID-UINT+1 }T

.( MINUS --> OK IF BLANK \/) CR
T{          0  5 - ->       -5 }T
T{          5  0 - ->        5 }T
T{          0 -5 - ->        5 }T
T{         -5  0 - ->       -5 }T
T{          1  2 - ->       -1 }T
T{          1 -2 - ->        3 }T
T{         -1  2 - ->       -3 }T
T{         -1 -2 - ->        1 }T
T{          0  1 - ->       -1 }T
T{ MID-UINT+1  1 - -> MID-UINT }T

.( COMMA HERE STORE FETCH and CELL PLUS and TWO FETCH and TWO STORE --> OK IF BLANK \/) CR
HERE 1 ,
HERE 2 ,
CONSTANT 2ND
CONSTANT 1ST

T{       1ST 2ND U< -> <TRUE> }T \ HERE MUST GROW WITH ALLOT
T{       1ST CELL+  -> 2ND }T \ ... BY ONE CELL
T{   1ST 1 CELLS +  -> 2ND }T
T{     1ST @ 2ND @  -> 1 2 }T
T{         5 1ST !  ->     }T
T{     1ST @ 2ND @  -> 5 2 }T
T{         6 2ND !  ->     }T
T{     1ST @ 2ND @  -> 5 6 }T
T{           1ST 2@ -> 6 5 }T
T{       2 1 1ST 2! ->     }T
T{           1ST 2@ -> 2 1 }T
T{ 1S 1ST !  1ST @  -> 1S  }T    \ CAN STORE CELL-WIDE VALUE

.( ONE PLUS --> OK IF BLANK \/) CR
T{        0 1+ ->          1 }T
T{       -1 1+ ->          0 }T
T{        1 1+ ->          2 }T
T{ MID-UINT 1+ -> MID-UINT+1 }T

.( ONE MINUS --> OK IF BLANK \/) CR
T{          2 1- ->        1 }T
T{          1 1- ->        0 }T
T{          0 1- ->       -1 }T
T{ MID-UINT+1 1- -> MID-UINT }T

.( TICK EXECUTE --> OK IF BLANK \/) CR
T{ : GT1 123 ;   ->     }T
T{ ' GT1 EXECUTE -> 123 }T

.( BRACKET TICK --> OK IF BLANK \/) CR
T{ : GT2 ['] GT1 ; IMMEDIATE -> }T
T{ GT2 EXECUTE -> 123 }T

.( STAR --> OK IF BLANK \/) CR
T{  0  0 * ->  0 }T          \ TEST IDENTITIES
T{  0  1 * ->  0 }T
T{  1  0 * ->  0 }T
T{  1  2 * ->  2 }T
T{  2  1 * ->  2 }T
T{  3  3 * ->  9 }T
T{ -3  3 * -> -9 }T
T{  3 -3 * -> -9 }T
T{ -3 -3 * ->  9 }T

T{ MID-UINT+1 1 RSHIFT 2 *               -> MID-UINT+1 }T
T{ MID-UINT+1 2 RSHIFT 4 *               -> MID-UINT+1 }T
T{ MID-UINT+1 1 RSHIFT MID-UINT+1 OR 2 * -> MID-UINT+1 }T

.( STAR SLASH MOD --> OK IF BLANK \/) CR
IFFLOORED    : T*/MOD >R M* R> FM/MOD ;
IFSYM        : T*/MOD >R M* R> SM/REM ;
T{       0 2       1 */MOD ->       0 2       1 T*/MOD }T
T{       1 2       1 */MOD ->       1 2       1 T*/MOD }T
T{       2 2       1 */MOD ->       2 2       1 T*/MOD }T
T{      -1 2       1 */MOD ->      -1 2       1 T*/MOD }T
T{      -2 2       1 */MOD ->      -2 2       1 T*/MOD }T
T{       0 2      -1 */MOD ->       0 2      -1 T*/MOD }T
T{       1 2      -1 */MOD ->       1 2      -1 T*/MOD }T
T{       2 2      -1 */MOD ->       2 2      -1 T*/MOD }T
T{      -1 2      -1 */MOD ->      -1 2      -1 T*/MOD }T
T{      -2 2      -1 */MOD ->      -2 2      -1 T*/MOD }T
T{       2 2       2 */MOD ->       2 2       2 T*/MOD }T
T{      -1 2      -1 */MOD ->      -1 2      -1 T*/MOD }T
T{      -2 2      -2 */MOD ->      -2 2      -2 T*/MOD }T
T{       7 2       3 */MOD ->       7 2       3 T*/MOD }T
T{       7 2      -3 */MOD ->       7 2      -3 T*/MOD }T
T{      -7 2       3 */MOD ->      -7 2       3 T*/MOD }T
T{      -7 2      -3 */MOD ->      -7 2      -3 T*/MOD }T
T{ MAX-INT 2 MAX-INT */MOD -> MAX-INT 2 MAX-INT T*/MOD }T
T{ MIN-INT 2 MIN-INT */MOD -> MIN-INT 2 MIN-INT T*/MOD }T 

.( STAR SLASH --> OK IF BLANK \/) CR
IFFLOORED    : T*/ T*/MOD SWAP DROP ;
IFSYM        : T*/ T*/MOD SWAP DROP ;

T{       0 2       1 */ ->       0 2       1 T*/ }T
T{       1 2       1 */ ->       1 2       1 T*/ }T
T{       2 2       1 */ ->       2 2       1 T*/ }T
T{      -1 2       1 */ ->      -1 2       1 T*/ }T
T{      -2 2       1 */ ->      -2 2       1 T*/ }T
T{       0 2      -1 */ ->       0 2      -1 T*/ }T
T{       1 2      -1 */ ->       1 2      -1 T*/ }T
T{       2 2      -1 */ ->       2 2      -1 T*/ }T
T{      -1 2      -1 */ ->      -1 2      -1 T*/ }T
T{      -2 2      -1 */ ->      -2 2      -1 T*/ }T
T{       2 2       2 */ ->       2 2       2 T*/ }T
T{      -1 2      -1 */ ->      -1 2      -1 T*/ }T
T{      -2 2      -2 */ ->      -2 2      -2 T*/ }T
T{       7 2       3 */ ->       7 2       3 T*/ }T
T{       7 2      -3 */ ->       7 2      -3 T*/ }T
T{      -7 2       3 */ ->      -7 2       3 T*/ }T
T{      -7 2      -3 */ ->      -7 2      -3 T*/ }T
T{ MAX-INT 2 MAX-INT */ -> MAX-INT 2 MAX-INT T*/ }T
T{ MIN-INT 2 MIN-INT */ -> MIN-INT 2 MIN-INT T*/ }T 

.( SLASH MOD --> OK IF BLANK \/) CR
IFFLOORED    : T/MOD >R S>D R> FM/MOD ;
IFSYM        : T/MOD >R S>D R> SM/REM ;

T{       0       1 /MOD ->       0       1 T/MOD }T
T{       1       1 /MOD ->       1       1 T/MOD }T
T{       2       1 /MOD ->       2       1 T/MOD }T
T{      -1       1 /MOD ->      -1       1 T/MOD }T
T{      -2       1 /MOD ->      -2       1 T/MOD }T
T{       0      -1 /MOD ->       0      -1 T/MOD }T
T{       1      -1 /MOD ->       1      -1 T/MOD }T
T{       2      -1 /MOD ->       2      -1 T/MOD }T
T{      -1      -1 /MOD ->      -1      -1 T/MOD }T
T{      -2      -1 /MOD ->      -2      -1 T/MOD }T
T{       2       2 /MOD ->       2       2 T/MOD }T
T{      -1      -1 /MOD ->      -1      -1 T/MOD }T
T{      -2      -2 /MOD ->      -2      -2 T/MOD }T
T{       7       3 /MOD ->       7       3 T/MOD }T
T{       7      -3 /MOD ->       7      -3 T/MOD }T
T{      -7       3 /MOD ->      -7       3 T/MOD }T
T{      -7      -3 /MOD ->      -7      -3 T/MOD }T
T{ MAX-INT       1 /MOD -> MAX-INT       1 T/MOD }T
T{ MIN-INT       1 /MOD -> MIN-INT       1 T/MOD }T
T{ MAX-INT MAX-INT /MOD -> MAX-INT MAX-INT T/MOD }T
T{ MIN-INT MIN-INT /MOD -> MIN-INT MIN-INT T/MOD }T 

.( SLASH --> OK IF BLANK \/) CR
IFFLOORED    : T/ T/MOD SWAP DROP ;
IFSYM        : T/ T/MOD SWAP DROP ;

T{       0       1 / ->       0       1 T/ }T
T{       1       1 / ->       1       1 T/ }T
T{       2       1 / ->       2       1 T/ }T
T{      -1       1 / ->      -1       1 T/ }T
T{      -2       1 / ->      -2       1 T/ }T
T{       0      -1 / ->       0      -1 T/ }T
T{       1      -1 / ->       1      -1 T/ }T
T{       2      -1 / ->       2      -1 T/ }T
T{      -1      -1 / ->      -1      -1 T/ }T
T{      -2      -1 / ->      -2      -1 T/ }T
T{       2       2 / ->       2       2 T/ }T
T{      -1      -1 / ->      -1      -1 T/ }T
T{      -2      -2 / ->      -2      -2 T/ }T
T{       7       3 / ->       7       3 T/ }T
T{       7      -3 / ->       7      -3 T/ }T
T{      -7       3 / ->      -7       3 T/ }T
T{      -7      -3 / ->      -7      -3 T/ }T
T{ MAX-INT       1 / -> MAX-INT       1 T/ }T
T{ MIN-INT       1 / -> MIN-INT       1 T/ }T
T{ MAX-INT MAX-INT / -> MAX-INT MAX-INT T/ }T
T{ MIN-INT MIN-INT / -> MIN-INT MIN-INT T/ }T 

.( COLON and SEMI COLON --> OK IF BLANK \/) CR
T{ : NOP : POSTPONE ; ; -> }T
T{ NOP NOP1 NOP NOP2 -> }T
T{ NOP1 -> }T
T{ NOP2 -> }T
\ The following tests the dictionary search order:
T{ : GDX   123 ;    : GDX   GDX 234 ; -> }T
T{ GDX -> 123 234 }T 

.( QUESTION DO --> OK IF BLANK \/) CR
BASE @
DECIMAL
: qd ?DO I LOOP ;
T{   789   789 qd -> }T
T{ -9876 -9876 qd -> }T
T{     5     0 qd -> 0 1 2 3 4 }T

: qd1 ?DO I 10 +LOOP ;
T{ 50 1 qd1 -> 1 11 21 31 41 }T
T{ 50 0 qd1 -> 0 10 20 30 40 }T 

: qd2 ?DO I 3 > IF LEAVE ELSE I THEN LOOP ;
T{ 5 -1 qd2 -> -1 0 1 2 3 }T 

: qd3 ?DO I 1 +LOOP ;
T{ 4  4 qd3 -> }T
T{ 4  1 qd3 ->  1 2 3 }T
T{ 2 -1 qd3 -> -1 0 1 }T 

: qd4 ?DO I -1 +LOOP ;
T{  4 4 qd4 -> }T
T{  1 4 qd4 -> 4 3 2 1 }T 
T{ -1 2 qd4 -> 2 1 0 -1 }T

: qd5 ?DO I -10 +LOOP ;
T{   1 50 qd5 -> 50 40 30 20 10   }T
T{   0 50 qd5 -> 50 40 30 20 10 0 }T
T{ -25 10 qd5 -> 10 0 -10 -20     }T 

VARIABLE qditerations
VARIABLE qdincrement

: qd6 ( limit start increment -- )    qdincrement !
   0 qditerations !
   ?DO
     1 qditerations +!
     I
     qditerations @ 6 = IF LEAVE THEN
     qdincrement @
   +LOOP qditerations @
;

T{  4  4 -1 qd6 ->                   0  }T
T{  1  4 -1 qd6 ->  4  3  2  1       4  }T
T{  4  1 -1 qd6 ->  1  0 -1 -2 -3 -4 6  }T
T{  4  1  0 qd6 ->  1  1  1  1  1  1 6  }T
T{  0  0  0 qd6 ->                   0  }T
T{  1  4  0 qd6 ->  4  4  4  4  4  4 6  }T
T{  1  4  1 qd6 ->  4  5  6  7  8  9 6  }T
T{  4  1  1 qd6 ->  1  2  3          3  }T
T{  4  4  1 qd6 ->                   0  }T
T{  2 -1 -1 qd6 -> -1 -2 -3 -4 -5 -6 6  }T
T{ -1  2 -1 qd6 ->  2  1  0 -1       4  }T
T{  2 -1  0 qd6 -> -1 -1 -1 -1 -1 -1 6  }T
T{ -1  2  0 qd6 ->  2  2  2  2  2  2 6  }T
T{ -1  2  1 qd6 ->  2  3  4  5  6  7 6  }T
T{  2 -1  1 qd6 -> -1  0  1          3  }T 
BASE !

.( QUESTION DUP --> OK IF BLANK \/) CR
T{ -1 ?DUP -> -1 -1 }T
T{  0 ?DUP ->  0    }T
T{  1 ?DUP ->  1  1 }T


.( PLUS STORE --> OK IF BLANK \/) CR
T{  0 1ST !        ->   }T
T{  1 1ST +!       ->   }T
T{    1ST @        -> 1 }T
T{ -1 1ST +! 1ST @ -> 0 }T

.( PLUS LOOP and DO I --> OK IF BLANK \/) CR
T{ : GD2 DO I -1 +LOOP ; -> }T
T{        1          4 GD2 -> 4 3 2  1 }T
T{       -1          2 GD2 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD2 -> MID-UINT+1 MID-UINT }T

VARIABLE gditerations
VARIABLE gdincrement

: gd7 ( limit start increment -- )
   gdincrement !
   0 gditerations !
   DO
     1 gditerations +!
     I
     gditerations @ 6 = IF LEAVE THEN
     gdincrement @
   +LOOP gditerations @
;

T{    4  4  -1 gd7 ->  4                  1  }T
T{    1  4  -1 gd7 ->  4  3  2  1         4  }T
T{    4  1  -1 gd7 ->  1  0 -1 -2  -3  -4 6  }T
T{    4  1   0 gd7 ->  1  1  1  1   1   1 6  }T
T{    0  0   0 gd7 ->  0  0  0  0   0   0 6  }T
T{    1  4   0 gd7 ->  4  4  4  4   4   4 6  }T
T{    1  4   1 gd7 ->  4  5  6  7   8   9 6  }T
T{    4  1   1 gd7 ->  1  2  3            3  }T
T{    4  4   1 gd7 ->  4  5  6  7   8   9 6  }T
T{    2 -1  -1 gd7 -> -1 -2 -3 -4  -5  -6 6  }T
T{   -1  2  -1 gd7 ->  2  1  0 -1         4  }T
T{    2 -1   0 gd7 -> -1 -1 -1 -1  -1  -1 6  }T
T{   -1  2   0 gd7 ->  2  2  2  2   2   2 6  }T
T{   -1  2   1 gd7 ->  2  3  4  5   6   7 6  }T
T{    2 -1   1 gd7 -> -1 0 1              3  }T
T{  -20 30 -10 gd7 -> 30 20 10  0 -10 -20 6  }T
T{  -20 31 -10 gd7 -> 31 21 11  1  -9 -19 6  }T
T{  -20 29 -10 gd7 -> 29 19  9 -1 -11     5  }T

\ With large and small increments

MAX-UINT 8 RSHIFT 1+ CONSTANT ustep
ustep NEGATE CONSTANT -ustep
MAX-INT 7 RSHIFT 1+ CONSTANT step
step NEGATE CONSTANT -step

VARIABLE bump

T{  : gd8 bump ! DO 1+ bump @ +LOOP ; -> }T

T{  0 MAX-UINT 0 ustep gd8 -> 256 }T
T{  0 0 MAX-UINT -ustep gd8 -> 256 }T
T{  0 MAX-INT MIN-INT step gd8 -> 256 }T
T{  0 MIN-INT MAX-INT -step gd8 -> 256 }T

.( LESS THAN --> OK IF BLANK \/) CR
T{       0       1 < -> <TRUE>  }T
T{       1       2 < -> <TRUE>  }T
T{      -1       0 < -> <TRUE>  }T
T{      -1       1 < -> <TRUE>  }T
T{ MIN-INT       0 < -> <TRUE>  }T
T{ MIN-INT MAX-INT < -> <TRUE>  }T
T{       0 MAX-INT < -> <TRUE>  }T
T{       0       0 < -> <FALSE> }T
T{       1       1 < -> <FALSE> }T
T{       1       0 < -> <FALSE> }T
T{       2       1 < -> <FALSE> }T
T{       0      -1 < -> <FALSE> }T
T{       1      -1 < -> <FALSE> }T
T{       0 MIN-INT < -> <FALSE> }T
T{ MAX-INT MIN-INT < -> <FALSE> }T
T{ MAX-INT       0 < -> <FALSE> }T

.( EQUAL --> OK IF BLANK \/) CR
T{  0  0 = -> <TRUE>  }T
T{  1  1 = -> <TRUE>  }T
T{ -1 -1 = -> <TRUE>  }T
T{  1  0 = -> <FALSE> }T
T{ -1  0 = -> <FALSE> }T
T{  0  1 = -> <FALSE> }T
T{  0 -1 = -> <FALSE> }T

.( GREATER THAN --> OK IF BLANK \/) CR
T{       0       1 > -> <FALSE> }T
T{       1       2 > -> <FALSE> }T
T{      -1       0 > -> <FALSE> }T
T{      -1       1 > -> <FALSE> }T
T{ MIN-INT       0 > -> <FALSE> }T
T{ MIN-INT MAX-INT > -> <FALSE> }T
T{       0 MAX-INT > -> <FALSE> }T
T{       0       0 > -> <FALSE> }T
T{       1       1 > -> <FALSE> }T
T{       1       0 > -> <TRUE>  }T
T{       2       1 > -> <TRUE>  }T
T{       0      -1 > -> <TRUE>  }T
T{       1      -1 > -> <TRUE>  }T
T{       0 MIN-INT > -> <TRUE>  }T
T{ MAX-INT MIN-INT > -> <TRUE>  }T
T{ MAX-INT       0 > -> <TRUE>  }T

.( ZERO LESS THAN --> OK IF BLANK \/) CR
T{       0 0< -> <FALSE> }T
T{      -1 0< -> <TRUE>  }T
T{ MIN-INT 0< -> <TRUE>  }T
T{       1 0< -> <FALSE> }T
T{ MAX-INT 0< -> <FALSE> }T

.( ZERO EQUAL --> OK IF BLANK \/) CR
T{        0 0= -> <TRUE>  }T
T{        1 0= -> <FALSE> }T
T{        2 0= -> <FALSE> }T
T{       -1 0= -> <FALSE> }T
T{ MAX-UINT 0= -> <FALSE> }T
T{ MIN-INT  0= -> <FALSE> }T
T{ MAX-INT  0= -> <FALSE> }T

.( TWO STAR --> OK IF BLANK \/) CR
T{   0S 2*       ->   0S }T
T{    1 2*       ->    2 }T
T{ 4000 2*       -> 8000 }T
T{   1S 2* 1 XOR ->   1S }T
T{  MSB 2*       ->   0S }T

.( TWO SLASH --> OK IF BLANK \/) CR
T{          0S 2/ ->   0S }T
T{           1 2/ ->    0 }T
T{        4000 2/ -> 2000 }T
T{          1S 2/ ->   1S }T \ MSB PROPOGATED
T{    1S 1 XOR 2/ ->   1S }T
T{ MSB 2/ MSB AND ->  MSB }T

.( TWO DROP --> OK IF BLANK \/) CR
T{ 1 2 2DROP -> }T

.( TWO DUP --> OK IF BLANK \/) CR
T{ 1 2 2DUP -> 1 2 1 2 }T

.( TWO OVER --> OK IF BLANK \/) CR
T{ 1 2 3 4 2OVER -> 1 2 3 4 1 2 }T

.( TWO SWAP --> OK IF BLANK \/) CR
T{ 1 2 3 4 2SWAP -> 3 4 1 2 }T

.( TO BODY and CREATE --> OK IF BLANK \/) CR
T{  CREATE CR0 ->      }T
T{ ' CR0 >BODY -> HERE }T

.( TO R and R FETCH and R FROM --> OK IF BLANK \/) CR
T{ : GR1 >R R> ; -> }T
T{ : GR2 >R R@ R> DROP ; -> }T
T{ 123 GR1 -> 123 }T
T{ 123 GR2 -> 123 }T
T{  1S GR1 ->  1S }T

.( TO IN IS NOT TESTED) CR

.( ABS --> OK IF BLANK \/) CR
T{       0 ABS ->          0 }T
T{       1 ABS ->          1 }T
T{      -1 ABS ->          1 }T
T{ MIN-INT ABS -> MID-UINT+1 }T

.( ALIGN ALIGNED --> OK IF BLANK \/) CR
ALIGN 1 ALLOT HERE ALIGN HERE 3 CELLS ALLOT
CONSTANT A-ADDR CONSTANT UA-ADDR
T{ UA-ADDR ALIGNED -> A-ADDR }T
T{       1 A-ADDR C!         A-ADDR       C@ ->       1 }T
T{    1234 A-ADDR !          A-ADDR       @  ->    1234 }T
T{ 123 456 A-ADDR 2!         A-ADDR       2@ -> 123 456 }T
T{       2 A-ADDR CHAR+ C!   A-ADDR CHAR+ C@ ->       2 }T
T{       3 A-ADDR CELL+ C!   A-ADDR CELL+ C@ ->       3 }T
T{    1234 A-ADDR CELL+ !    A-ADDR CELL+ @  ->    1234 }T
T{ 123 456 A-ADDR CELL+ 2!   A-ADDR CELL+ 2@ -> 123 456 }T

.( ALLOT HERE --> OK IF BLANK \/) CR
HERE 1 ALLOT
HERE
CONSTANT 2NDA
CONSTANT 1STA
T{ 1STA 2NDA U< -> <TRUE> }T    \ HERE MUST GROW WITH ALLOT
T{      1STA 1+ ->   2NDA }T    \ ... BY ONE ADDRESS UNIT 

.( AND INVERT --> OK IF BLANK \/) CR
T{ 0 0 AND -> 0 }T
T{ 0 1 AND -> 0 }T
T{ 1 0 AND -> 0 }T
T{ 1 1 AND -> 1 }T

T{ 0 INVERT 1 AND -> 1 }T
T{ 1 INVERT 1 AND -> 0 }T

T{ 0S 0S AND -> 0S }T
T{ 0S 1S AND -> 0S }T
T{ 1S 0S AND -> 0S }T
T{ 1S 1S AND -> 1S }T

.( BASE DECIMAL HEX --> OK IF BLANK \/) CR
: GN2 \ ( -- 16 10 )
   BASE @ >R HEX BASE @ DECIMAL BASE @ R> BASE ! ;
T{ GN2 -> 16 10 }T
DECIMAL

.( BL --> OK IF BLANK \/) CR
T{ BL -> 32 }T


.( CELLS --> OK IF BLANK \/) CR
: BITS ( X -- U )
   0 SWAP BEGIN DUP WHILE
     DUP MSB AND IF >R 1+ R> THEN 2*
   REPEAT DROP ;

( CELLS >= 1 AU, INTEGRAL MULTIPLE OF CHAR SIZE, >= 16 BITS )
T{ 1 CELLS 1 <         -> <FALSE> }T
T{ 1 CELLS 1 CHARS MOD ->    0    }T
T{ 1S BITS 10 <        -> <FALSE> }T

.( C COMMA and CHAR PLUS and C FETCH and C STORE and CHARS --> OK IF BLANK \/) CR
HERE 1 C,
HERE 2 C,
CONSTANT 2NDC
CONSTANT 1STC

T{    1STC 2NDC U< -> <TRUE> }T \ HERE MUST GROW WITH ALLOT
T{      1STC CHAR+ ->  2NDC  }T \ ... BY ONE CHAR
T{  1STC 1 CHARS + ->  2NDC  }T
T{ 1STC C@ 2NDC C@ ->   1 2  }T
T{       3 1STC C! ->        }T
T{ 1STC C@ 2NDC C@ ->   3 2  }T
T{       4 2NDC C! ->        }T
T{ 1STC C@ 2NDC C@ ->   3 4  }T

.( BRACKET CHAR --> OK IF BLANK \/) CR
BASE @ HEX
T{ : GC1 [CHAR] X     ; -> }T
T{ : GC2 [CHAR] HELLO ; -> }T
T{ GC1 -> 58 }T
T{ GC2 -> 48 }T
BASE !

.( CHARS --> OK IF BLANK \/) CR
( CHARACTERS >= 1 AU, <= SIZE OF CELL, >= 8 BITS )
T{ 1 CHARS 1 <       -> <FALSE> }T
T{ 1 CHARS 1 CELLS > -> <FALSE> }T

.( CONSTANT --> OK IF BLANK \/) CR
T{ 123 CONSTANT X123 -> }T
T{ X123 -> 123 }T
T{ : EQU CONSTANT ; -> }T
T{ X123 EQU Y123 -> }T
T{ Y123 -> 123 }T

.( COUNT --> OK IF BLANK \/) CR
T{ GT1STRING COUNT -> GT1STRING CHAR+ 3 }T

.( DEPTH --> OK IF BLANK \/) CR
T{ 0 1 DEPTH -> 0 1 2 }T
T{   0 DEPTH -> 0 1   }T
T{     DEPTH -> 0     }T

.( DUP --> OK IF BLANK \/) CR
T{ 1 DUP -> 1 1 }T

.( DROP --> OK IF BLANK \/) CR
T{ 1 2 DROP -> 1 }T
T{ 0   DROP ->   }T

.( EMIT DOT and DOT QUOTE and U DOT and CR SPACE SPACES TYPE \/  \/  \/) CR
: OUTPUT-TEST
  [ BASE @ HEX ]
  BASE @ HEX
   ." YOU SHOULD SEE THE STANDARD GRAPHIC CHARACTERS:" CR
   41 BL DO I EMIT LOOP CR
   61 41 DO I EMIT LOOP CR
   7F 61 DO I EMIT LOOP CR
   ." YOU SHOULD SEE 0-9 SEPARATED BY A SPACE:" CR
   9 1+ 0 DO I . LOOP CR
   ." YOU SHOULD SEE 0-9 (WITH NO SPACES):" CR
   [CHAR] 9 1+ [CHAR] 0 DO I 0 SPACES EMIT LOOP CR
   ." YOU SHOULD SEE A-G SEPARATED BY A SPACE:" CR
   [CHAR] G 1+ [CHAR] A DO I EMIT SPACE LOOP CR
   ." YOU SHOULD SEE 0-5 SEPARATED BY TWO SPACES:" CR
   5 1+ 0 DO I [CHAR] 0 + EMIT 2 SPACES LOOP CR
   ." YOU SHOULD SEE TWO SEPARATE LINES:" CR
   S" LINE 1" TYPE CR S" LINE 2" TYPE CR
   ." YOU SHOULD SEE THE NUMBER RANGES OF SIGNED AND UNSIGNED NUMBERS:" CR
   ." SIGNED: " MIN-INT . MAX-INT . CR
   ." UNSIGNED: " 0 U. MAX-UINT U. CR
   BASE !
   [ BASE ! ]
;
T{ OUTPUT-TEST -> }T

.( EVALUATE IS NOT TESTED AND MAY NOT BE CALLED FROM USER CODE) CR

.( IF ELSE THEN --> OK IF BLANK \/) CR
T{ : GI1 IF 123 THEN ; -> }T
T{ : GI2 IF 123 ELSE 234 THEN ; -> }T
T{  0 GI1 ->     }T
T{  1 GI1 -> 123 }T
T{ -1 GI1 -> 123 }T
T{  0 GI2 -> 234 }T
T{  1 GI2 -> 123 }T
T{ -1 GI1 -> 123 }T

\ Multiple ELSEs in an IF statement
: melse IF 1 ELSE 2 ELSE 3 ELSE 4 ELSE 5 THEN ;
T{ <FALSE> melse -> 2 4 }T
T{ <TRUE>  melse -> 1 3 5 }T

.( IMMEDIATE IS NOT TESTED) CR

.( INVERT --> OK IF BLANK \/) CR
T{ 0S INVERT -> 1S }T
T{ 1S INVERT -> 0S }T

.( J --> OK IF BLANK \/) CR
T{ : GD3 DO 1 0 DO J LOOP LOOP ; -> }T
T{          4        1 GD3 ->  1 2 3   }T
T{          2       -1 GD3 -> -1 0 1   }T
T{ MID-UINT+1 MID-UINT GD3 -> MID-UINT }T

T{ : GD4 DO 1 0 DO J LOOP -1 +LOOP ; -> }T
T{        1          4 GD4 -> 4 3 2 1             }T
T{       -1          2 GD4 -> 2 1 0 -1            }T
T{ MID-UINT MID-UINT+1 GD4 -> MID-UINT+1 MID-UINT }T 

.( LEAVE I --> OK IF BLANK \/) CR
T{ : GD5 123 SWAP 0 DO 
     I 4 > IF DROP 234 LEAVE THEN 
   LOOP ; -> }T
T{ 1 GD5 -> 123 }T
T{ 5 GD5 -> 123 }T
T{ 6 GD5 -> 234 }T

.( LEFT BRACKET and RIGHT BRACKET --> OK IF BLANK \/) CR
BASE @ HEX
T{ : GC3 [ GC1 ] LITERAL ; -> }T
T{ GC3 -> 58 }T
BASE !

.( LEFT PARENTHESIS  --> OK IF BLANK \/) CR
\ There is no space either side of the ).
T{ ( A comment)1234 -> 1234 }T  \ This is modified from original
T{ : pc1 ( A comment)1234 ; pc1 -> 1234 }T 

.( LITERAL --> OK IF BLANK \/) CR
T{ : GT3 GT2 LITERAL ; -> }T
T{ GT3 -> ' GT1 }T

.( LSHIFT --> OK IF BLANK \/) CR
BASE @ HEX
T{   1 0 LSHIFT ->    1 }T
T{   1 1 LSHIFT ->    2 }T
T{   1 2 LSHIFT ->    4 }T
T{   1 F LSHIFT -> 8000 }T      \ BIGGEST GUARANTEED SHIFT
T{  1S 1 LSHIFT 1 XOR -> 1S }T
T{ MSB 1 LSHIFT ->    0 }T
BASE !

.( LOOP DO I --> OK IF BLANK \/) CR
T{ : GD1 DO I LOOP ; -> }T
T{          4        1 GD1 ->  1 2 3   }T
T{          2       -1 GD1 -> -1 0 1   }T
T{ MID-UINT+1 MID-UINT GD1 -> MID-UINT }T

.( M STAR --> OK IF BLANK \/) CR
T{       0       0 M* ->       0 S>D }T
T{       0       1 M* ->       0 S>D }T
T{       1       0 M* ->       0 S>D }T
T{       1       2 M* ->       2 S>D }T
T{       2       1 M* ->       2 S>D }T
T{       3       3 M* ->       9 S>D }T
T{      -3       3 M* ->      -9 S>D }T
T{       3      -3 M* ->      -9 S>D }T
T{      -3      -3 M* ->       9 S>D }T
T{       0 MIN-INT M* ->       0 S>D }T
T{       1 MIN-INT M* -> MIN-INT S>D }T
T{       2 MIN-INT M* ->       0 1S  }T
T{       0 MAX-INT M* ->       0 S>D }T
T{       1 MAX-INT M* -> MAX-INT S>D }T
T{       2 MAX-INT M* -> MAX-INT     1 LSHIFT 0 }T
T{ MIN-INT MIN-INT M* ->       0 MSB 1 RSHIFT   }T
T{ MAX-INT MIN-INT M* ->     MSB MSB 2/         }T
T{ MAX-INT MAX-INT M* ->       1 MSB 2/ INVERT  }T

.( MAX --> OK IF BLANK \/) CR
T{       0       1 MAX ->       1 }T
T{       1       2 MAX ->       2 }T
T{      -1       0 MAX ->       0 }T
T{      -1       1 MAX ->       1 }T
T{ MIN-INT       0 MAX ->       0 }T
T{ MIN-INT MAX-INT MAX -> MAX-INT }T
T{       0 MAX-INT MAX -> MAX-INT }T
T{       0       0 MAX ->       0 }T
T{       1       1 MAX ->       1 }T
T{       1       0 MAX ->       1 }T
T{       2       1 MAX ->       2 }T
T{       0      -1 MAX ->       0 }T
T{       1      -1 MAX ->       1 }T
T{       0 MIN-INT MAX ->       0 }T
T{ MAX-INT MIN-INT MAX -> MAX-INT }T
T{ MAX-INT       0 MAX -> MAX-INT }T

.( MIN --> OK IF BLANK \/) CR
T{       0       1 MIN ->       0 }T
T{       1       2 MIN ->       1 }T
T{      -1       0 MIN ->      -1 }T
T{      -1       1 MIN ->      -1 }T
T{ MIN-INT       0 MIN -> MIN-INT }T
T{ MIN-INT MAX-INT MIN -> MIN-INT }T
T{       0 MAX-INT MIN ->       0 }T
T{       0       0 MIN ->       0 }T
T{       1       1 MIN ->       1 }T
T{       1       0 MIN ->       0 }T
T{       2       1 MIN ->       1 }T
T{       0      -1 MIN ->      -1 }T
T{       1      -1 MIN ->      -1 }T
T{       0 MIN-INT MIN -> MIN-INT }T
T{ MAX-INT MIN-INT MIN -> MIN-INT }T
T{ MAX-INT       0 MIN ->       0 }T

.( MOD --> OK IF BLANK \/) CR
IFFLOORED    : TMOD T/MOD DROP ;
IFSYM        : TMOD T/MOD DROP ;

T{       0       1 MOD ->       0       1 TMOD }T
T{       1       1 MOD ->       1       1 TMOD }T
T{       2       1 MOD ->       2       1 TMOD }T
T{      -1       1 MOD ->      -1       1 TMOD }T
T{      -2       1 MOD ->      -2       1 TMOD }T
T{       0      -1 MOD ->       0      -1 TMOD }T
T{       1      -1 MOD ->       1      -1 TMOD }T
T{       2      -1 MOD ->       2      -1 TMOD }T
T{      -1      -1 MOD ->      -1      -1 TMOD }T
T{      -2      -1 MOD ->      -2      -1 TMOD }T
T{       2       2 MOD ->       2       2 TMOD }T
T{      -1      -1 MOD ->      -1      -1 TMOD }T
T{      -2      -2 MOD ->      -2      -2 TMOD }T
T{       7       3 MOD ->       7       3 TMOD }T
T{       7      -3 MOD ->       7      -3 TMOD }T
T{      -7       3 MOD ->      -7       3 TMOD }T
T{      -7      -3 MOD ->      -7      -3 TMOD }T
T{ MAX-INT       1 MOD -> MAX-INT       1 TMOD }T
T{ MIN-INT       1 MOD -> MIN-INT       1 TMOD }T
T{ MAX-INT MAX-INT MOD -> MAX-INT MAX-INT TMOD }T
T{ MIN-INT MIN-INT MOD -> MIN-INT MIN-INT TMOD }T 

.( FILL --> OK IF BLANK \/) CR
T{ FBUF 0 20 FILL -> }T
T{ SEEBUF -> 00 00 00 }T

T{ FBUF 1 20 FILL -> }T
T{ SEEBUF -> 20 00 00 }T

T{ FBUF 3 20 FILL -> }T
T{ SEEBUF -> 20 20 20 }T 

.( MOVE --> OK IF BLANK \/) CR  \ MUST FOLLOW FILL TESTS
T{ FBUF FBUF 3 CHARS MOVE -> }T \ BIZARRE SPECIAL CASE
T{ SEEBUF -> 20 20 20 }T

T{ SBUF FBUF 0 CHARS MOVE -> }T
T{ SEEBUF -> 20 20 20 }T

T{ SBUF FBUF 1 CHARS MOVE -> }T
T{ SEEBUF -> 12 20 20 }T

T{ SBUF FBUF 3 CHARS MOVE -> }T
T{ SEEBUF -> 12 34 56 }T

T{ FBUF FBUF CHAR+ 2 CHARS MOVE -> }T
T{ SEEBUF -> 12 12 34 }T

T{ FBUF CHAR+ FBUF 2 CHARS MOVE -> }T
T{ SEEBUF -> 12 34 34 }T 

.( NEGATE --> OK IF BLANK \/) CR
T{  0 NEGATE ->  0 }T
T{  1 NEGATE -> -1 }T
T{ -1 NEGATE ->  1 }T
T{  2 NEGATE -> -2 }T
T{ -2 NEGATE ->  2 }T

.( OR --> OK IF BLANK \/) CR
T{ 0S 0S OR -> 0S }T
T{ 0S 1S OR -> 1S }T
T{ 1S 0S OR -> 1S }T
T{ 1S 1S OR -> 1S }T

.( OVER --> OK IF BLANK \/) CR
T{ 1 2 OVER -> 1 2 1 }T

.( POSTPONE IS NOT FULLY IMPLEMENTED AND SHOULD NOT BE USED) CR

.( ROT --> OK IF BLANK \/) CR
T{ 1 2 3 ROT -> 2 3 1 }T

.( R SHIFT --> OK IF BLANK \/) CR
BASE @ HEX
T{    1 0 RSHIFT -> 1 }T
T{    1 1 RSHIFT -> 0 }T
T{    2 1 RSHIFT -> 1 }T
T{    4 2 RSHIFT -> 1 }T
T{ 8000 F RSHIFT -> 1 }T              \ Biggest
T{  MSB 1 RSHIFT MSB AND ->   0 }T    \ RSHIFT zero fills MSBs
T{  MSB 1 RSHIFT     2*  -> MSB }T
BASE !

.( S QUOTE --> OK IF BLANK \/) CR
BASE @ HEX
T{ : GC4 S" XY" ; ->   }T
T{ GC4 SWAP DROP  -> 2 }T
T{ GC4 DROP DUP C@ SWAP CHAR+ C@ -> 58 59 }T

: GC5 S" A String"2DROP ; \ There is no space between the " and 2DROP
T{ GC5 -> }T 
BASE !

.( S TO D --> OK IF BLANK \/) CR
T{       0 S>D ->       0  0 }T
T{       1 S>D ->       1  0 }T
T{       2 S>D ->       2  0 }T
T{      -1 S>D ->      -1 -1 }T
T{      -2 S>D ->      -2 -1 }T
T{ MIN-INT S>D -> MIN-INT -1 }T
T{ MAX-INT S>D -> MAX-INT  0 }T

.( SM SLASH REM --> OK IF BLANK \/) CR
T{       0 S>D              1 SM/REM ->  0       0 }T
T{       1 S>D              1 SM/REM ->  0       1 }T
T{       2 S>D              1 SM/REM ->  0       2 }T
T{      -1 S>D              1 SM/REM ->  0      -1 }T
T{      -2 S>D              1 SM/REM ->  0      -2 }T
T{       0 S>D             -1 SM/REM ->  0       0 }T
T{       1 S>D             -1 SM/REM ->  0      -1 }T
T{       2 S>D             -1 SM/REM ->  0      -2 }T
T{      -1 S>D             -1 SM/REM ->  0       1 }T
T{      -2 S>D             -1 SM/REM ->  0       2 }T
T{       2 S>D              2 SM/REM ->  0       1 }T
T{      -1 S>D             -1 SM/REM ->  0       1 }T
T{      -2 S>D             -2 SM/REM ->  0       1 }T
T{       7 S>D              3 SM/REM ->  1       2 }T
T{       7 S>D             -3 SM/REM ->  1      -2 }T
T{      -7 S>D              3 SM/REM -> -1      -2 }T   \ MODIFIED FR. ORIG.
T{      -7 S>D             -3 SM/REM -> -1       2 }T
T{ MAX-INT S>D              1 SM/REM ->  0 MAX-INT }T
T{ MIN-INT S>D              1 SM/REM ->  0 MIN-INT }T
T{ MAX-INT S>D        MAX-INT SM/REM ->  0       1 }T
T{ MIN-INT S>D        MIN-INT SM/REM ->  0       1 }T
T{      1S 1                4 SM/REM ->  3 MAX-INT }T
T{       2 MIN-INT M*       2 SM/REM ->  0 MIN-INT }T
T{       2 MIN-INT M* MIN-INT SM/REM ->  0       2 }T
T{       2 MAX-INT M*       2 SM/REM ->  0 MAX-INT }T
T{       2 MAX-INT M* MAX-INT SM/REM ->  0       2 }T
T{ MIN-INT MIN-INT M* MIN-INT SM/REM ->  0 MIN-INT }T
T{ MIN-INT MAX-INT M* MIN-INT SM/REM ->  0 MAX-INT }T
T{ MIN-INT MAX-INT M* MAX-INT SM/REM ->  0 MIN-INT }T
T{ MAX-INT MAX-INT M* MAX-INT SM/REM ->  0 MAX-INT }T

.( SOURCE IS NOT TESTED) CR

.( SWAP --> OK IF BLANK \/) CR
T{ 1 2 SWAP -> 2 1 }T

.( U LESS THAN --> OK IF BLANK \/) CR
T{        0        1 U< -> <TRUE>  }T
T{        1        2 U< -> <TRUE>  }T
T{        0 MID-UINT U< -> <TRUE>  }T
T{        0 MAX-UINT U< -> <TRUE>  }T
T{ MID-UINT MAX-UINT U< -> <TRUE>  }T
T{        0        0 U< -> <FALSE> }T
T{        1        1 U< -> <FALSE> }T
T{        1        0 U< -> <FALSE> }T
T{        2        1 U< -> <FALSE> }T
T{ MID-UINT        0 U< -> <FALSE> }T
T{ MAX-UINT        0 U< -> <FALSE> }T
T{ MAX-UINT MID-UINT U< -> <FALSE> }T

.( UNLOOP EXIT DO I --> OK IF BLANK \/) CR
T{ : GD6 ( PAT: {0 0},{0 0}{1 0}{1 1},{0 0}{1 0}{1 1}{2 0}{2 1}{2 2} ) 
      0 SWAP 0 DO 
         I 1+ 0 DO 
           I J + 3 = IF I UNLOOP I UNLOOP EXIT THEN 1+ 
         LOOP 
      LOOP ; -> }T
T{ 1 GD6 -> 1 }T
T{ 2 GD6 -> 3 }T
T{ 3 GD6 -> 4 1 2 }T

.( UNTIL BEGIN --> OK IF BLANK \/) CR
T{ : GI4 BEGIN DUP 1+ DUP 5 > UNTIL ; -> }T
T{ 3 GI4 -> 3 4 5 6 }T
T{ 5 GI4 -> 5 6 }T
T{ 6 GI4 -> 6 7 }T

.( WHILE BEGIN REPEAT --> OK IF BLANK \/) CR
T{ : GI3 BEGIN DUP 5 < WHILE DUP 1+ REPEAT ; -> }T
T{ 0 GI3 -> 0 1 2 3 4 5 }T
T{ 4 GI3 -> 4 5 }T
T{ 5 GI3 -> 5 }T
T{ 6 GI3 -> 6 }T
T{ : GI5 BEGIN DUP 2 > WHILE 
      DUP 5 < WHILE DUP 1+ REPEAT 
      123 ELSE 345 THEN ; -> }T
T{ 1 GI5 -> 1 345 }T
T{ 2 GI5 -> 2 345 }T
T{ 3 GI5 -> 3 4 5 123 }T
T{ 4 GI5 -> 4 5 123 }T
T{ 5 GI5 -> 5 123 }T 


.( WORD --> OK IF BLANK \/) CR
 : GS3 WORD COUNT SWAP C@ ;
T{ BL GS3 HELLO -> 5 CHAR H }T
T{ CHAR " GS3 GOODBYE" -> 7 CHAR G }T
T{ BL GS3 
   DROP -> 0 }T \ Blank lines return zero-length strings 

.( UM STAR --> OK IF BLANK \/) CR
T{ 0 0 UM* -> 0 0 }T
T{ 0 1 UM* -> 0 0 }T
T{ 1 0 UM* -> 0 0 }T
T{ 1 2 UM* -> 2 0 }T
T{ 2 1 UM* -> 2 0 }T
T{ 3 3 UM* -> 9 0 }T

T{ MID-UINT+1 1 RSHIFT 2 UM* ->  MID-UINT+1 0 }T
T{ MID-UINT+1          2 UM* ->           0 1 }T
T{ MID-UINT+1          4 UM* ->           0 2 }T
T{         1S          2 UM* -> 1S 1 LSHIFT 1 }T
T{   MAX-UINT   MAX-UINT UM* ->    1 1 INVERT }T 

.( UM SLASH MOD --> OK IF BLANK \/) CR
T{        0            0        1 UM/MOD -> 0        0 }T
T{        1            0        1 UM/MOD -> 0        1 }T
T{        1            0        2 UM/MOD -> 1        0 }T
T{        3            0        2 UM/MOD -> 1        1 }T
T{ MAX-UINT        2 UM*        2 UM/MOD -> 0 MAX-UINT }T
T{ MAX-UINT        2 UM* MAX-UINT UM/MOD -> 0        2 }T
T{ MAX-UINT MAX-UINT UM* MAX-UINT UM/MOD -> 0 MAX-UINT }T

.( VARIABLE --> OK IF BLANK \/) CR
T{ VARIABLE V1 ->     }T
T{    123 V1 ! ->     }T
T{        V1 @ -> 123 }T

.( XOR --> OK IF BLANK \/) CR
T{ 0S 0S XOR -> 0S }T
T{ 0S 1S XOR -> 1S }T
T{ 1S 0S XOR -> 1S }T
T{ 1S 1S XOR -> 0S }T

CR 
.( *** CORE EXT WORDS ***) CR
CR 
.( AGAIN IS NOT TESTED) CR
.( BACK SLASH IS NOT TESTED) CR

.( BUFFER COLON NIP --> OK IF BLANK \/) CR
DECIMAL
T{ 127 CHARS BUFFER: TBUF1 -> }T
T{ 127 CHARS BUFFER: TBUF2 -> }T \ Buffer is aligned
T{ TBUF1 ALIGNED -> TBUF1 }T \ Buffers do not overlap
T{ TBUF2 TBUF1 - ABS 127 CHARS < -> <FALSE> }T \ Buffer can be written to
1 CHARS CONSTANT /CHAR
: TFULL? ( c-addr n char -- flag )
   TRUE 2SWAP CHARS OVER + SWAP ?DO
     OVER I C@ = AND
   /CHAR +LOOP NIP
;

T{ TBUF1 127 CHAR * FILL   ->        }T
T{ TBUF1 127 CHAR * TFULL? -> <TRUE> }T

T{ TBUF1 127 0 FILL   ->        }T
T{ TBUF1 127 0 TFULL? -> <TRUE> }T

.( C QUOTE and NUMBER QUESTION --> OK IF BLANK \/) CR
T{ : cq1 C" 123" ; -> }T
T{ : cq2 C" " ;    -> }T
T{ cq1 COUNT NUMBER? -> 123 1 }T
T{ cq2 COUNT NUMBER? ->  0   }T 

.( DOT LEFT PARENTHESIS IS NOT TESTED) CR

.( FALSE --> OK IF BLANK \/) CR
T{ FALSE -> 0 }T
T{ FALSE -> <FALSE> }T 

.( NOT EQUAL IS NOT TESTED) CR
.( PARSE IS NOT TESTED) CR

.( PARSE-NAME COMPARE --> OK IF BLANK \/) CR
T{ PARSE-NAME abcd S" abcd" COMPARE -> 0 }T
T{ PARSE-NAME   abcde   S" abcde" COMPARE -> 0 }T

\ test empty parse area
T{ PARSE-NAME 
   NIP -> 0 }T    \ empty line
T{ PARSE-NAME    
   NIP -> 0 }T    \ line with white space

T{ : parse-name-test ( "name1" "name2" -- n ) 
   PARSE-NAME PARSE-NAME COMPARE ; -> }T

T{ parse-name-test abcd abcd -> 0 }T
T{ parse-name-test  abcd   abcd   -> 0 }T
T{ parse-name-test abcde abcdf -> -1 }T
T{ parse-name-test abcdf abcde -> 1 }T
T{ parse-name-test abcde abcde 
    -> 0 }T
T{ parse-name-test abcde abcde  
    -> 0 }T    \ line with white space 

.( PICK IS NOT TESTED) CR
.( SOURCE-ID IS NOT TESTED) CR

.( TRUE --> OK IF BLANK \/) CR
T{ TRUE -> <TRUE> }T
T{ TRUE -> 0 INVERT }T

.( TUCK IS NOT TESTED) CR
.( DOT R IS NOT TESTED) CR
.( TWO R FETCH IS NOT TESTED) CR
.( TWO R FROM IS NOT TESTED) CR
.( TWO TO R IS NOT TESTED) CR

.( U LESS THAN --> OK IF BLANK \/) CR
T{        0        1 U< -> <TRUE>  }T
T{        1        2 U< -> <TRUE>  }T
T{        0 MID-UINT U< -> <TRUE>  }T
T{        0 MAX-UINT U< -> <TRUE>  }T
T{ MID-UINT MAX-UINT U< -> <TRUE>  }T
T{        0        0 U< -> <FALSE> }T
T{        1        1 U< -> <FALSE> }T
T{        1        0 U< -> <FALSE> }T
T{        2        1 U< -> <FALSE> }T
T{ MID-UINT        0 U< -> <FALSE> }T
T{ MAX-UINT        0 U< -> <FALSE> }T
T{ MAX-UINT MID-UINT U< -> <FALSE> }T

.( UNUSED IS NOT TESTED) CR
.( U DOT R IS NOT TESTED) CR

.( VALUE and TO --> OK IF BLANK \/) CR
T{  111 VALUE v1 -> }T
T{ -999 VALUE v2 -> }T
T{ v1 ->  111 }T
T{ v2 -> -999 }T
T{ 222 TO v1 -> }T
T{ v1 -> 222 }T
T{ : vd1 v1 ; -> }T
T{ vd1 -> 222 }T
T{ : vd2 TO v2 ; -> }T
T{ v2 -> -999 }T
T{ -333 vd2 -> }T
T{ v2 -> -333 }T
T{ v1 ->  222 }T 

.( ZERO GREATER THAN IS NOT TESTED) CR
.( ZERO NOT EQUAL IS NOT TESTED) CR

CR 
.( *** COMMON USE WORDS ***) CR
CR 

.( COMMA QUOTE IS NOT TESTED) CR
.( M MINUS IS NOT TESTED) CR
.( M SLASH IS NOT TESTED) CR
.( NOT IS NOT TESTED) CR
.( TWO MINUS IS NOT TESTED) CR
.( TWO PLUS IS NOT TESTED) CR

CR 
.( *** DOUBLE WORDS ***) CR
CR 

MAX-INT 2/ CONSTANT HI-INT \ 001...1
MIN-INT 2/ CONSTANT LO-INT \ 110...1 

1S MAX-INT 2CONSTANT MAX-2INT \ 01...1
0 MIN-INT 2CONSTANT MIN-2INT \ 10...0
MAX-2INT 2/ 2CONSTANT HI-2INT \ 001...1
MIN-2INT 2/ 2CONSTANT LO-2INT \ 110...0 

.( DABS --> OK IF BLANK \/) CR
T{       1. DABS -> 1.       }T
T{      -1. DABS -> 1.       }T
T{ MAX-2INT DABS -> MAX-2INT }T
T{ MIN-2INT 1. D+ DABS -> MAX-2INT }T 

.( D DOT R and D DOT --> OK IF OUTPUT VERIFIED \/ \/ \/) CR
MAX-2INT 71 73 M*/ 2CONSTANT dbl1
MIN-2INT 73 79 M*/ 2CONSTANT dbl2
\ THIS IS NOT THE WAY THIS TEST SHOULD WORK, BUT...
: DoubleOutput
   ." You should see lines duplicated:" CR
   ."      8970676912557384689" CR
   5 SPACES dbl1 D. CR
   ."         8970676912557384689" CR
   5 SPACES dbl1 19 3 + D.R CR
   ."      -8522862768232894101" CR
   5 SPACES dbl2 D. CR
   ."           -8522862768232894101" CR
   5 SPACES dbl2 20 5 + D.R CR
;

T{ DoubleOutput -> }T 

.( D EQUALS --> OK IF BLANK \/) CR
T{      -1.      -1. D= -> <TRUE>  }T
T{      -1.       0. D= -> <FALSE> }T
T{      -1.       1. D= -> <FALSE> }T
T{       0.      -1. D= -> <FALSE> }T
T{       0.       0. D= -> <TRUE>  }T
T{       0.       1. D= -> <FALSE> }T
T{       1.      -1. D= -> <FALSE> }T
T{       1.       0. D= -> <FALSE> }T
T{       1.       1. D= -> <TRUE>  }T

T{   0   -1    0  -1 D= -> <TRUE>  }T
T{   0   -1    0   0 D= -> <FALSE> }T
T{   0   -1    0   1 D= -> <FALSE> }T
T{   0    0    0  -1 D= -> <FALSE> }T
T{   0    0    0   0 D= -> <TRUE>  }T
T{   0    0    0   1 D= -> <FALSE> }T
T{   0    1    0  -1 D= -> <FALSE> }T
T{   0    1    0   0 D= -> <FALSE> }T
T{   0    1    0   1 D= -> <TRUE>  }T

T{ MAX-2INT MIN-2INT D= -> <FALSE> }T
T{ MAX-2INT       0. D= -> <FALSE> }T
T{ MAX-2INT MAX-2INT D= -> <TRUE>  }T
T{ MAX-2INT HI-2INT  D= -> <FALSE> }T
T{ MAX-2INT MIN-2INT D= -> <FALSE> }T
T{ MIN-2INT MIN-2INT D= -> <TRUE>  }T
T{ MIN-2INT LO-2INT  D= -> <FALSE> }T
T{ MIN-2INT MAX-2INT D= -> <FALSE> }T 

.( D LESS THAN --> OK IF BLANK \/) CR
T{       0.       1. D< -> <TRUE>  }T
T{       0.       0. D< -> <FALSE> }T
T{       1.       0. D< -> <FALSE> }T
T{      -1.       1. D< -> <TRUE>  }T
T{      -1.       0. D< -> <TRUE>  }T
T{      -2.      -1. D< -> <TRUE>  }T
T{      -1.      -2. D< -> <FALSE> }T
T{      -1. MAX-2INT D< -> <TRUE>  }T
T{ MIN-2INT MAX-2INT D< -> <TRUE>  }T
T{ MAX-2INT      -1. D< -> <FALSE> }T
T{ MAX-2INT MIN-2INT D< -> <FALSE> }T

T{ MAX-2INT 2DUP -1. D+ D< -> <FALSE> }T
T{ MIN-2INT 2DUP  1. D+ D< -> <TRUE>  }T 

.( D MAX --> OK IF BLANK \/) CR
T{       1.       2. DMAX ->  2.      }T
T{       1.       0. DMAX ->  1.      }T
T{       1.      -1. DMAX ->  1.      }T
T{       1.       1. DMAX ->  1.      }T
T{       0.       1. DMAX ->  1.      }T
T{       0.      -1. DMAX ->  0.      }T
T{      -1.       1. DMAX ->  1.      }T
T{      -1.      -2. DMAX -> -1.      }T

T{ MAX-2INT  HI-2INT DMAX -> MAX-2INT }T
T{ MAX-2INT MIN-2INT DMAX -> MAX-2INT }T
T{ MIN-2INT MAX-2INT DMAX -> MAX-2INT }T
T{ MIN-2INT  LO-2INT DMAX -> LO-2INT  }T

T{ MAX-2INT       1. DMAX -> MAX-2INT }T
T{ MAX-2INT      -1. DMAX -> MAX-2INT }T
T{ MIN-2INT       1. DMAX ->  1.      }T
T{ MIN-2INT      -1. DMAX -> -1.      }T 

.( D MIN --> OK IF BLANK \/) CR
T{       1.       2. DMIN ->  1.      }T
T{       1.       0. DMIN ->  0.      }T
T{       1.      -1. DMIN -> -1.      }T
T{       1.       1. DMIN ->  1.      }T
T{       0.       1. DMIN ->  0.      }T
T{       0.      -1. DMIN -> -1.      }T
T{      -1.       1. DMIN -> -1.      }T
T{      -1.      -2. DMIN -> -2.      }T

T{ MAX-2INT  HI-2INT DMIN -> HI-2INT  }T
T{ MAX-2INT MIN-2INT DMIN -> MIN-2INT }T
T{ MIN-2INT MAX-2INT DMIN -> MIN-2INT }T
T{ MIN-2INT  LO-2INT DMIN -> MIN-2INT }T

T{ MAX-2INT       1. DMIN ->  1.      }T
T{ MAX-2INT      -1. DMIN -> -1.      }T
T{ MIN-2INT       1. DMIN -> MIN-2INT }T
T{ MIN-2INT      -1. DMIN -> MIN-2INT }T 

.( D MINUS --> OK IF BLANK \/) CR
T{  0.  5. D- -> -5. }T              \ small integers
T{  5.  0. D- ->  5. }T
T{  0. -5. D- ->  5. }T
T{  1.  2. D- -> -1. }T
T{  1. -2. D- ->  3. }T
T{ -1.  2. D- -> -3. }T
T{ -1. -2. D- ->  1. }T
T{ -1. -1. D- ->  0. }T
T{  0  0  0  5 D- ->  0 -5 }T       \ mid-range integers
T{ -1  5  0  0 D- -> -1  5 }T
T{  0  0 -1 -5 D- ->  1  4 }T
T{  0 -5  0  0 D- ->  0 -5 }T
T{ -1  1  0  2 D- -> -1 -1 }T
T{  0  1 -1 -2 D- ->  1  2 }T
T{  0 -1  0  2 D- ->  0 -3 }T
T{  0 -1  0 -2 D- ->  0  1 }T
T{  0  0  0  1 D- ->  0 -1 }T

T{ MIN-INT 0 2DUP D- -> 0. }T
T{ MIN-INT S>D MAX-INT 0 D- -> 1 1S }T
T{ MAX-2INT MAX-2INT D- -> 0. }T    \ large integers
T{ MIN-2INT MIN-2INT D- -> 0. }T
T{ MAX-2INT  HI-2INT D- -> LO-2INT DNEGATE }T
T{  HI-2INT  LO-2INT D- -> MAX-2INT }T
T{  LO-2INT  HI-2INT D- -> MIN-2INT 1. D+ }T
T{ MIN-2INT MIN-2INT D- -> 0. }T
T{ MIN-2INT  LO-2INT D- -> LO-2INT }T 

.( D NEGATE --> OK IF BLANK \/) CR
T{   0. DNEGATE ->  0. }T
T{   1. DNEGATE -> -1. }T
T{  -1. DNEGATE ->  1. }T
T{ MAX-2INT DNEGATE -> MIN-2INT SWAP 1+ SWAP }T
T{ MIN-2INT SWAP 1+ SWAP DNEGATE -> MAX-2INT }T 

.( D PLUS --> OK IF BLANK \/) CR
T{  0.  5. D+ ->  5. }T                         \ small integers
T{ -5.  0. D+ -> -5. }T
T{  1.  2. D+ ->  3. }T
T{  1. -2. D+ -> -1. }T
T{ -1.  2. D+ ->  1. }T
T{ -1. -2. D+ -> -3. }T
T{ -1.  1. D+ ->  0. }T

T{  0  0  0  5 D+ ->  0  5 }T                  \ mid range integers
T{ -1  5  0  0 D+ -> -1  5 }T
T{  0  0  0 -5 D+ ->  0 -5 }T
T{  0 -5 -1  0 D+ -> -1 -5 }T
T{  0  1  0  2 D+ ->  0  3 }T
T{ -1  1  0 -2 D+ -> -1 -1 }T
T{  0 -1  0  2 D+ ->  0  1 }T
T{  0 -1 -1 -2 D+ -> -1 -3 }T
T{ -1 -1  0  1 D+ -> -1  0 }T

T{ MIN-INT 0 2DUP D+ -> 0 1 }T
T{ MIN-INT S>D MIN-INT 0 D+ -> 0 0 }T

T{  HI-2INT       1. D+ -> 0 HI-INT 1+ }T    \ large double integers
T{  HI-2INT     2DUP D+ -> 1S 1- MAX-INT }T
T{ MAX-2INT MIN-2INT D+ -> -1. }T
T{ MAX-2INT  LO-2INT D+ -> HI-2INT }T
T{  LO-2INT     2DUP D+ -> MIN-2INT }T
T{  HI-2INT MIN-2INT D+ 1. D+ -> LO-2INT }T 

.( D TO S --> OK IF BLANK \/) CR
T{    1234  0 D>S ->  1234   }T
T{   -1234 -1 D>S -> -1234   }T
T{ MAX-INT  0 D>S -> MAX-INT }T
T{ MIN-INT -1 D>S -> MIN-INT }T 

.( D TWO SLASH --> OK IF BLANK \/) CR
T{       0. D2/ -> 0.        }T
T{       1. D2/ -> 0.        }T
T{      0 1 D2/ -> MIN-INT 0 }T
T{ MAX-2INT D2/ -> HI-2INT   }T
T{      -1. D2/ -> -1.       }T
T{ MIN-2INT D2/ -> LO-2INT   }T 

.( D TWO STAR --> OK IF BLANK \/) CR
T{              0. D2* -> 0. D2* }T
T{ MIN-INT       0 D2* -> 0 1 }T
T{         HI-2INT D2* -> MAX-2INT 1. D- }T
T{         LO-2INT D2* -> MIN-2INT }T 

.( D ZERO EQUAL --> OK IF BLANK \/) CR
T{               1. D0= -> <FALSE> }T
T{ MIN-INT        0 D0= -> <FALSE> }T
T{         MAX-2INT D0= -> <FALSE> }T
T{      -1  MAX-INT D0= -> <FALSE> }T
T{               0. D0= -> <TRUE>  }T
T{              -1. D0= -> <FALSE> }T
T{       0  MIN-INT D0= -> <FALSE> }T 

.( D ZERO LESS --> OK IF BLANK \/) CR
T{                0. D0< -> <FALSE> }T
T{                1. D0< -> <FALSE> }T
T{  MIN-INT        0 D0< -> <FALSE> }T
T{        0  MAX-INT D0< -> <FALSE> }T
T{          MAX-2INT D0< -> <FALSE> }T
T{               -1. D0< -> <TRUE>  }T
T{          MIN-2INT D0< -> <TRUE>  }T 

.( M PLUS --> OK IF BLANK \/) CR
T{ HI-2INT   1 M+ -> HI-2INT   1. D+ }T
T{ MAX-2INT -1 M+ -> MAX-2INT -1. D+ }T
T{ MIN-2INT  1 M+ -> MIN-2INT  1. D+ }T
T{ LO-2INT  -1 M+ -> LO-2INT  -1. D+ }T 

.( M STAR / --> OK IF BLANK \/) CR
: ?floored [ -3 2 / -2 = ] LITERAL IF 1. D- THEN ; 
T{       5.       7             11 M*/ ->  3. }T
T{       5.      -7             11 M*/ -> -3. ?floored }T
T{      -5.       7             11 M*/ -> -3. ?floored }T
T{      -5.      -7             11 M*/ ->  3. }T
T{ MAX-2INT       8             16 M*/ -> HI-2INT }T
T{ MAX-2INT      -8             16 M*/ -> HI-2INT DNEGATE ?floored }T
T{ MIN-2INT       8             16 M*/ -> LO-2INT }T
T{ MIN-2INT      -8             16 M*/ -> LO-2INT DNEGATE }T

T{ MAX-2INT MAX-INT        MAX-INT M*/ -> MAX-2INT }T
T{ MAX-2INT MAX-INT 2/     MAX-INT M*/ -> MAX-INT 1- HI-2INT NIP }T
T{ MIN-2INT LO-2INT NIP DUP NEGATE M*/ -> MIN-2INT }T
T{ MIN-2INT LO-2INT NIP 1- MAX-INT M*/ -> MIN-INT 3 + HI-2INT NIP 2 + }T
T{ MAX-2INT LO-2INT NIP DUP NEGATE M*/ -> MAX-2INT DNEGATE }T
T{ MIN-2INT MAX-INT            DUP M*/ -> MIN-2INT }T 

.( TWO CONSTANT --> OK IF BLANK \/) CR
T{ 1 2 2CONSTANT 2c1 -> }T
T{ 2c1 -> 1 2 }T

T{ : cd1 2c1 ; -> }T
T{ cd1 -> 1 2 }T

T{ : cd2 2CONSTANT ; -> }T
T{ -1 -2 cd2 2c2 -> }T
T{ 2c2 -> -1 -2 }T

T{ 4 5 2CONSTANT 2c3 IMMEDIATE 2c3 -> 4 5 }T
T{ : cd6 2c3 2LITERAL ; cd6 -> 4 5 }T 

.( TWO LITERAL --> OK IF BLANK \/) CR
T{ : cd1 [ MAX-2INT ] 2LITERAL ; -> }T
T{ cd1 -> MAX-2INT }T

T{ 2VARIABLE 2v4 IMMEDIATE 5 6 2v4 2! -> }T
T{ : cd7 2v4 [ 2@ ] 2LITERAL ; cd7 -> 5 6 }T
T{ : cd8 [ 6 7 ] 2v4 [ 2! ] ; 2v4 2@ -> 6 7 }T 

.( TWO VARIABLE --> OK IF BLANK \/) CR
T{ 2VARIABLE 2v1 -> }T
T{ 0. 2v1 2! ->    }T
T{    2v1 2@ -> 0. }T
T{ -1 -2 2v1 2! ->       }T
T{       2v1 2@ -> -1 -2 }T

T{ : cd2 2VARIABLE ; -> }T
T{ cd2 2v2 -> }T
T{ : cd3 2v2 2! ; -> }T
T{ -2 -1 cd3 -> }T
T{ 2v2 2@ -> -2 -1 }T

T{ 2VARIABLE 2v3 IMMEDIATE 5 6 2v3 2! -> }T
T{ 2v3 2@ -> 5 6 }T 


CR 
.( *** DOUBLE EXT WORDS ***) CR
CR 

.( TWO ROT --> OK IF BLANK \/) CR
T{       1.       2. 3. 2ROT ->       2. 3.       1. }T
T{ MAX-2INT MIN-2INT 1. 2ROT -> MIN-2INT 1. MAX-2INT }T 

CR 
.( *** FACILITY WORDS ***) CR
CR 

.( AT XY IS NOT TESTED) CR
.( PAGE IS NOT TESTED) CR

CR 
.( *** FILE WORDS ***) CR
CR 

.( FILE WORDS ARE NOT TESTED) CR

CR 
.( *** STRING WORDS ***) CR
CR 

T{ : s1 S" abcdefghijklmnopqrstuvwxyz" ; -> }T   \ Prerequisite
T{ : s6 S" 12345" ; -> }T     \ From SEARCH test

.( C MOVE IS NOT TESTED) CR
.( C MOVE UP IS NOT TESTED) CR

.( COMPARE --> OK IF BLANK \/) CR
CREATE PAD 50 ALLOT   \ create our own PAD
T{ s1        s1 COMPARE ->  0  }T
T{ s1  PAD SWAP CMOVE   ->     }T    \ Copy s1 to PAD
T{ s1  PAD OVER COMPARE ->  0  }T
T{ s1     PAD 6 COMPARE ->  1  }T
T{ PAD 10    s1 COMPARE -> -1  }T
T{ s1     PAD 0 COMPARE ->  1  }T
T{ PAD  0    s1 COMPARE -> -1  }T
T{ s1        s6 COMPARE ->  1  }T
T{ s6        s1 COMPARE -> -1  }T

: "abdde" S" abdde" ;
: "abbde" S" abbde" ;
: "abcdf" S" abcdf" ;
: "abcdee" S" abcdee" ;

T{ s1 "abdde"  COMPARE -> -1 }T
T{ s1 "abbde"  COMPARE ->  1 }T
T{ s1 "abcdf"  COMPARE -> -1 }T
T{ s1 "abcdee" COMPARE ->  1 }T

: s11 S" 0abc" ;
: s12 S" 0aBc" ;

T{ s11 s12 COMPARE ->  1 }T
T{ s12 s11 COMPARE -> -1 }T

CR 
.( *** TOOLS EXT WORDS ***) CR
CR 

.( AHEAD --> OK IF BLANK \/) CR
T{ : pt1 AHEAD 1111 2222 THEN 3333 ; -> }T
T{ pt1 -> 3333 }T 

.( CS PICK IS NOT TESTED) CR
.( CS ROLL IS NOT TESTED) CR

CR 
.( *** TOOLS WORDS ***) CR
CR 

.( WORDS MARKER --> OK IF OUTPUT VERIFIED \/ \/ \/) CR
CLEANUP
WORDS


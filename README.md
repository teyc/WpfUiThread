WPF example showing scenarios around UI Thread
=================================================

The first example is a button which spends too long accessing
a database. As a consequence, the UI Thread locks up. While
the database update is in progress, the progress bar shows 
no activity, the checkbox cannot be clicked, and the window
could not be dragged around. The program has "frozen".

The second example shows us mitigating the problem by doing
the database update in a separate thread.

The third example is similar to the second. However, rather
than using MVVM binding, we decide to update a label directly
in a thread after the long running process has finished.
This crashes the program.

The fourth example demonstrates how to _not_ crash the program,
and still update the label directly via `Dispatcher.Invoke()`.

Binding to a list
------------------

The second screen demonstrates some of the problems the UI thread
poses when binding to a list.

The first example still demonstrates UI freezing, but does not
crash the program.

The second example which used to work now crashes.

The third example always crashed anyway.

The fourth example which used to work also crashes.






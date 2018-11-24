# Switches

```
SWD Execution Trending - JavaScript builder, version 2
Usage:

    SWDExecTrending --version | /version
        Print out the version of this tool.

    SWDExecTrending /raw
        Run the tool without filtering the data by _eventTime. The default filter removes events dated in the future and older than 30 days.

    SWDExecTrending
        Invoke this tool with no command line arguments. It will automatically read the content of EvT_AeX_SWD_Execution and will produce a JavaScript string to stdout.

    Output: The javascript defines a global variable named object_count that matches the count of Policy Execution for which trending data was generated (100 max).

    Each policy execution trend data will match the following outline:

        var top_n = { // where n is the row id
            "name" : "<SWD Policy name>",
            " stats" : {
                "hourly" : [
                    ["Date", "Total", "Success", "Error"],
                    [ ... ], // Hourly statistics for the Policy
                    [ ... ],
                    [ ... ]
                ]
            }
        }

    The policies are order by execution count in descending order, limited to 100 at most. The SQL was adjusted to take care of special cases:

        * Data that indicates the Policy execution took place in the future is not included
        * Data going back more than 30 days is not included

    The first is to avoid computers that are operating with an invalid date from disrupting the trend graphs, the later is to avoid computers coming back from long period of inactivity from disrupting the trend graphs (this is especially needed for SWD policies such as the "Windows System Assessment Scan" that is largely deployed and almost always running on computers.

    The program returns 0 if it produces any JavaScript output.
    The program returns 1 if it prints out command line help or version message.
    The program returns -1 if the user is not member of the Altiris Administrator group.
```
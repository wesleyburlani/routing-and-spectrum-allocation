# RoutingAndSpectrumAllocation

This project applies the RSA algorithm to supply random demands. 

It builds the graph based on 2 ".csv" files where 1 of them contains the graph links and the other contains the graph nodes. Like the follow example: 

[Nodes file example](/RoutingAndSpectrumAllocation/Data/arnes_nodes.csv)

[Links file example](/RoutingAndSpectrumAllocation/Data/arnes_links.csv)

And after read them, it generates ramdon demands and applies Routing and spectrum allocation(RSA), returning the results on 3 files:<br/> 

* demands.json 
* graph.json
* log.txt

## Setup

There are 5 configuration variables to fill on "Program.cs" file:<br/>

* <b>CsvLineSeparator:</b> specifies the character used to separate lines on specified ".csv" inputs files. 
* <b>CsvColumnSeparator:</b> specifies the character used to separate columns on specified ".csv" inputs files. 
* <b>LogPath:</b> specifies a folder path where the program must to save output files. 
* <b>ReadNodesPath:</b> specifies a file path where the program must to read the graph nodes.
* <b>ReadLinksPath:</b> specifies a file path where the program must to read the graph links.

## NuGET dependencies

* [Microsoft.Extensions.DependencyInjection 2.1.1](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)
* [Newtonsoft.Json 11.0.2](https://www.nuget.org/packages/Newtonsoft.Json/)

## Program Options

This program contains a lot of different ways to make routing and spectrum alocations. To change it's behavior you need to edit "Program.cs" file setting up the desired behavior changing the dependency injections. The following interfaces must be instanciated: 

#### IRoutingAndSpectrumAllocation 
Sets the RSA behavior. It can be instanciated by: SingleRSA, DedicatedProtectionRSA and SharedProtectionRSA. Where: 
* <b>SingleRSA:</b> Fills 1 dedicated allocation on RSA table that means the main path to destiantion node;
* <b>DedicatedProtectionRSA:</b> Fiils 2 dedicated allocations on RSA table that mens 1 main path and 1 secundary path to destination node;
* <b>SharedProtectionRSA:</b> Fills 1 dedicated allcation and 1 shared allocation on RSA table that means a main dedicated path to destination a secundary shared path that can be shared with another secundary path from another demand that main paths can't be unavailable simutaneously;


#### IProgramLogger
Set logs behavior to program output. It can be instanciated by: FileProgramLogger and  NullFileProgramLogger. Where:
* <b>FileProgramLogger:</b> Persists program output on a ".txt" file that the path is specified "LogPath" constant string;
* <b>NullProgramLogger:</b> Ignores program output;





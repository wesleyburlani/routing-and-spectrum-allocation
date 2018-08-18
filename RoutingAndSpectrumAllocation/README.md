# RoutingAndSpectrumAllocation

This project applies the RSA algorithm to supply random demands. 

It builds the graph based on 2 ".csv" files where 1 of them contains the graph links and the other contains the graph nodes. Like the follow example: 

### nodes file example:

Id,Lat,Long,Type<br/>
Celje,46.29,15.27,EOCC

### links file example: 

From,To,Length,Capacity,Cost,Designation,Delay<br/>
Koper,Ljubljana,100.165198199944,50,0,[Koper<->Ljubljana(1)],FIBER

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

* Microsoft.Extensions.DependencyInjection 2.1.1
* Newtonsoft.Json 11.0.2

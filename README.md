# PUBG-Tournaments

A simple Unity project that lists **PUBG tournaments**. 

It interacts with [PUBG's official Developer API](https://documentation.pubg.com/en/introduction.html), making a request to the `/tournaments` endpoint and rendering the obtained results in a GUI.  

A search input field was also added to filter the tournaments list by *tournament ID*. 

## Usage

Open project with Unity, and with `PUBGTournaments` scene in place, either run in-site or Build for creating a .exe file.


## Plugins

The [SimpleJSON](https://github.com/Bunny83/SimpleJSON) plugin was added to facilitate the deserialization of JSON data.

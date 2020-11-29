# BadgerAOPVote
A FiveM script for making AOP Votes. This script can link up to BadgerEssentials to automatically  
set the aop to the winning option.

## Commands
`/startVote <duration> <option1, option2, etc...> ` Starts an aop vote, duration is in seconds

`/vote <option>` Casts a vote for an AOP.

## Configuration
**"colour1":** Sets the first colour, by default "^1" / red  
**"colour2":** Sets the second colour, by default "^3" / yellow  
**"badgerEssentialsLink":** Chooses if you want to link this script to [BadgerEssentials](https://github.com/ChonkyBadger/BadgerEssentials)

## Permissions
`BadgerEssentials.Command.StartVote` Gives access to the /startVote command

## Installation
Under "releases", download the latest version and extract the files to a folder. You should have one folder with the script's  
files in it. You should see a config folder, fxmanifest.lua, README.md, License, and 3 dlls. If you see a .sln, it means you downloaded  
the source code instead of the compiled files. In your server.cfg, you will want to add:  
"start BadgerAOPVote". If you did not name the folder, "BadgerAOPVote", just replace it with whatever you did name it.  

## Links
- [My Discord Server](https://discord.gg/TFCQE8d)

# GreenBehaviors - GODOT

## To-do

- [ ] Tree Editor
- [ ] Behavior Tree Builder
- [x] Differenciate b/t update & tick
  - I've gone with TickFull. LAS.
- [x] Counter On Repeater
- [x] Composite; Set _runningNode in Start
- [x] Parallels & Monitors
- [x] Active Selectors / Dynamic Guards
  - Named Prioritized Selectors
- [x] Filters

## design-doc

### Features

- [ ] Tree Editor
- [x] C# btree

## dev-log

## Mono plugins: Refreshing the plugin

Writing GDScript plugins, you just have to deactivate and activate the plugin for the editor to update it from your code. I guess it's obvious (now that I think aboutit) that when working with Mono, you need to rebuild the project. Not even restarting Godot will help.

## Mono plugins: tool class not recognizing non tool classes

http://github.com/godotengine/godot/issues/36395

As thus some of the scripts for the editor will have to be tool.

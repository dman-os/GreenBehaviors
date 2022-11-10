# GreenBehaviors

Behavior trees for godot (C#).

## How to setup

First add as a submodule using the following command.

```shell
git submodule add https://github.com/skorpi-and-friends/GreenBehaviors addons/GreenBehaviors
```

Try to make your projects `.csproj` to look something like this:

```xml
<!-- You'll only need to add parts that don't exist -->
<Project Sdk="Godot.NET.Sdk/3.2.3">
  <PropertyGroup>
    <!-- . -->
    <!-- . -->
    <!-- 
      we need to disable default compilation so that GreenBehaviors source files
      will be compiled in a separate project
     -->
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <!-- . -->
    <!-- . -->
  </PropertyGroup>
  <!-- . -->
  <!-- . -->
  <ItemGroup>
    <Compile Include="**/*.cs" Exclude=".mono/**;addons/GreenBehaviors**" />
    <ProjectReference Include="addons\GreenBehaviors\GreenBehaviors.csproj" />
  </ItemGroup>
  <!-- . -->
  <!-- . -->
</Project>
```

### If using the old `.csproj` format

That is, the one that was in use before `3.2.3`, some more work awaits you. First go into `GreenBehaviors.csproj` and you'll find a large commented out section at the bottom. De-comment that and comment out the previously active section (the one at the top). Then add the following to your project's `.csproj` to link GreenBehaviors:

```xml
  <ItemGroup>
    <ProjectReference Include="addons\GreenBehaviors\GreenBehaviors.csproj" />
  </ItemGroup>
```


That's it.

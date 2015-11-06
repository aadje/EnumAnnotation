EnumAnnotation DisplayAttribute 
==========

[![Build status](https://ci.appveyor.com/api/projects/status/tl14tk8aag6fr5q4)](https://ci.appveyor.com/project/aadje/enumannotation)  


Enum wrapper for more conviently accessing the Display DataAnnotation Attribute on an Enum

1. Use by copy-pasting [EnumAnnotation.cs](https://raw.github.com/aadje/EnumAnnotation/master/EnumAnnotations/EnumAnnotation.cs) into your project, and modify as you see fit
2. Or reference the assembly using Nuget [```Install-Package EnumAnnotation```](https://nuget.org/packages/EnumAnnotation)   

* Use the in .net 4 added [```System.ComponentModel.DataAnnotations.DisplayAnnotation```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.aspx) to add friendly names to your [Enum](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations.Test/Data/SomeStatus.cs)
* Generate Lists with enum names and values to bind in your UI, using ```EnumAnnotation.GetDisplays<SomeStatus>();```  
Filter these Lists by supplying the enum values explicitly ```EnumAnnotation.GetDisplays(SomeStatus.Fine, SomeStatus.Ok);```   or using a lambda predicate function ```EnumAnnotation.GetDisplays<SomeStatus>(a => a.Name != "Fine");```
* Add multiple names to your Enums using the DisplayAnnotations [```Name```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.name.aspx), [```ShortName```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.shortname.aspx), [```Desciption```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.description.aspx) and [```GroupName```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.groupname.aspx) properties
* Supports the [```DisplayAnnotation.ResourceType```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.resourcetype.aspx) to add localization to the ```Name```, ```ShortName```, ```Desciption``` and ```GroupName``` properties using standard [resx](https://github.com/aadje/EnumAnnotation/tree/master/EnumAnnotations.Test/Resources) files. See [example](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations.Test/Data/LocalizedStatus.cs) and [unit tests](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations.Test/EnumAnnotationTest.cs)
* Extension method to easy access a DisplayAttribute on a single enum value, which looks like ```SomeStatus.Fine.GetDisplay();```, or access the Name value directly, using ```SomeStatus.Fine.GetName();```
* Reorder your Enum using the [```DisplayAnnotion.Order```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.order.aspx) property, without changing the Enums Underying value
* [Portable libary](http://msdn.microsoft.com/en-us/library/gg597391.aspx) that supports .Net 4.03, Silverlight 5 and Windows 8 and up. Portable libraries are supported in Nuget 2.1 and up

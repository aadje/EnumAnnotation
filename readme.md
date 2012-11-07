EnumAnnotation DisplayAttribute 
==========
Enum wrapper for more conviently accessing the Data Annotations Attributes on Enums

1. Use by copy pasting [EnumAnnotation.cs](https://raw.github.com/aadje/EnumAnnotation/master/EnumAnnotations/EnumAnnotation.cs) into your project, and change how you like
2. Or install using [Nuget](https://nuget.org/packages/EnumAnnotation) ```Install-Package EnumAnnotation```  

* Use the [```System.ComponentModel.DataAnnotations.DisplayAnnotation```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.aspx) to add friendly names to your [Enum](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations.Test/Data/SomeStatus.cs)
* Generate Lists with enum values and names for controls in your user interface, using ```EnumAnnotation.GetDisplays<SomeStatus>();```
* Add multiple names to your Enums using the DisplayAnnotations Name, ShortName, Desciption and GroupName properties
* Supports the DisplayAnnotation.ResourceType to add localization to the Name, ShortName, Desciption and GroupName properties. See [example](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations.Test/Data/LocalizedStatus.cs) and [tests](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations.Test/EnumAnnotationTest.cs)
* Extension method to easy access an DisplayAttribute on a single enum value with ```SomeStatus.Fine.GetDisplay()``` Or access the Name value directly, using ```SomeStatus.Fine.GetName()```
* Reorder your Enum using the [```DisplayAnnotion.Order```](http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.order.aspx) property, without changing the Enums Underying type
* [```IDisplayAnnotation```](https://github.com/aadje/EnumAnnotation/blob/master/EnumAnnotations/EnumAnnotation.cs) interface to hide the ugly generic type and implement in UI controls 
* [Portable libary](http://msdn.microsoft.com/en-us/library/gg597391.aspx) that supports .Net 4.03, Silverlight 5 and Windows 8 and higher
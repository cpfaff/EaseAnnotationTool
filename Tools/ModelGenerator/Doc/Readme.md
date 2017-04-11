Annotation item model auto-generation tool

XmlSchemaClassGenerator.Console.exe -n=""="CAFE.Core.Integration" -k=AnnotationItem -ct="System.Collections.Generic.List`1" cafe.xsd
- Generates Annotation integration/core model (this model user to map xml files on import/export)

XmlSchemaClassGenerator.Console.exe -n=""="CAFE.DAL.Models" -k=AnnotationItem -f  -ct="System.Collections.Generic.List`1" cafe.xsd
- Generates Annotation ef data model

XmlSchemaClassGenerator.Console.exe -n=""="CAFE.Web.Models" -k=AnnotationItemModel -m -ct="System.Collections.Generic.List`1" cafe.xsd
- Generates Annotation ASP.NET MVC model

options:
-n - using namespace for generated classes
-k - root clas name definition
-f - generate ef compatible model (Annotation ef data model)
-m - generate mvc model (Annotation ASP.NET MVC model)
-ct - user specific collection type for collection properties
cafe.xsd - schema class by generated on

For automage generation of all kinds of model run tool generate.bat from Compile folder

Generated files need to copy in project to 

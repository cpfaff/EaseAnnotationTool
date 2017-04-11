XmlSchemaClassGenerator.Console.exe -v -n=""="CAFE.Core.Integration" -k=AnnotationItem -y=Aliases.txt -ct="System.Collections.Generic.List`1" ease.xsd

XmlSchemaClassGenerator.Console.exe -v -n=""="CAFE.DAL.Models" -k=DbAnnotationItem -f  -y=Aliases.txt -ct="System.Collections.Generic.List`1" ease.xsd

XmlSchemaClassGenerator.Console.exe -v -n=""="CAFE.Web.Models" -k=AnnotationItemModel -m -y=Aliases.txt -ct="System.Collections.Generic.List`1" ease.xsd

pause
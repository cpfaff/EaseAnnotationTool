﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XmlSchemaClassGenerator
{
    public class Generator
    {
        private readonly GeneratorConfiguration _configuration = new GeneratorConfiguration();

        public NamespaceProvider NamespaceProvider
        {
            get { return _configuration.NamespaceProvider; }
            set { _configuration.NamespaceProvider = value; }
        }

        public string NamespacePrefix
        {
            get { return _configuration.NamespacePrefix; }
            set { _configuration.NamespacePrefix = value; }
        }

        public string OutputFolder
        {
            get { return _configuration.OutputFolder; }
            set { _configuration.OutputFolder = value; }
        }

        public Action<string> Log
        {
            get { return _configuration.Log; }
            set { _configuration.Log = value; }
        }

        /// <summary>
        /// Enable data binding with INotifyPropertyChanged
        /// </summary>
        public bool EnableDataBinding
        {
            get { return _configuration.EnableDataBinding; }
            set { _configuration.EnableDataBinding = value; }
        }

        /// <summary>
        /// Use XElement instead of XmlElement for Any nodes?
        /// </summary>
        public bool UseXElementForAny
        {
            get { return _configuration.UseXElementForAny; }
            set { _configuration.UseXElementForAny = value; }
        }

        /// <summary>
        /// How are the names of the created properties changed?
        /// </summary>
        public NamingScheme NamingScheme
        {
            get { return _configuration.NamingScheme; }
            set { _configuration.NamingScheme = value; }
        }

        /// <summary>
        /// Emit the "Order" attribute value for XmlElementAttribute to ensure the correct order
        /// of the serialized XML elements.
        /// </summary>
        public bool EmitOrder
        {
            get { return _configuration.EmitOrder; }
            set { _configuration.EmitOrder = value; }
        }

        /// <summary>
        /// Determines the kind of annotations to emit
        /// </summary>
        public DataAnnotationMode DataAnnotationMode
        {
            get { return _configuration.DataAnnotationMode; }
            set { _configuration.DataAnnotationMode = value; }
        }

        public bool GenerateNullables
        {
            get { return _configuration.GenerateNullables; }
            set { _configuration.GenerateNullables = value; }
        }

        public bool GenerateSerializableAttribute
        {
            get { return _configuration.GenerateSerializableAttribute; }
            set { _configuration.GenerateSerializableAttribute = value; }
        }

        public bool GenerateDesignerCategoryAttribute
        {
            get { return _configuration.GenerateDesignerCategoryAttribute; }
            set { _configuration.GenerateDesignerCategoryAttribute = value; }
        }

        public Type CollectionType
        {
            get { return _configuration.CollectionType; }
            set { _configuration.CollectionType = value; }
        }

        public Type CollectionImplementationType
        {
            get { return _configuration.CollectionImplementationType; }
            set { _configuration.CollectionImplementationType = value; }
        }

        public Type IntegerDataType
        {
            get { return _configuration.IntegerDataType; }
            set { _configuration.IntegerDataType = value; }
        }

        public bool EntityFramework
        {
            get { return _configuration.EntityFramework; }
            set { _configuration.EntityFramework = value; }
        }

        public bool ViewModel
        {
            get { return _configuration.ViewModel; }
            set { _configuration.ViewModel = value; }
        }

        public bool GenerateInterfaces
        {
            get { return _configuration.GenerateInterfaces; }
            set { _configuration.GenerateInterfaces = value; }
        }

        public CodeTypeReferenceOptions CodeTypeReferenceOptions
        {
            get { return _configuration.CodeTypeReferenceOptions; }
            set { _configuration.CodeTypeReferenceOptions = value; }
        }

        public IDictionary<string, string> Aliases => _configuration.Aliases;

        public void SetAliases(string path)
        {
            if(string.IsNullOrEmpty(path))
                throw new NullReferenceException("path");

            if(!File.Exists(path))
                throw new InvalidOperationException("Specified file does not exist");

            var fileLines = File.ReadAllLines(path);

            Dictionary<string, string> aliases = new Dictionary<string, string>();
            for (int line = 0; line < fileLines.Length; line++)
            {
                var aliaseArray = fileLines[line].Split(new [] {':'}, StringSplitOptions.RemoveEmptyEntries);
                if(!aliases.ContainsKey(aliaseArray[0].ToLower()))
                aliases.Add(aliaseArray[0].ToLower(), aliaseArray[1]);
            }

            _configuration.Aliases = aliases;
        }

        public string RootClassName
        {
            get { return _configuration.RootClassName; }
            set { _configuration.RootClassName = value; }
        }

        private readonly XmlSchemaSet Set = new XmlSchemaSet();
        private Dictionary<XmlQualifiedName, XmlSchemaAttributeGroup> AttributeGroups;
        private Dictionary<XmlQualifiedName, XmlSchemaGroup> Groups;
        private readonly Dictionary<NamespaceKey, NamespaceModel> Namespaces = new Dictionary<NamespaceKey, NamespaceModel>();
        private readonly Dictionary<XmlQualifiedName, TypeModel> Types = new Dictionary<XmlQualifiedName, TypeModel>();
        private static readonly XmlQualifiedName AnyType = new XmlQualifiedName("anyType", XmlSchema.Namespace);

        public void Generate(IEnumerable<string> files)
        {
            var schemas = files.Select(f => XmlSchema.Read(XmlReader.Create(f, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }), (s, e) =>
            {
                Trace.TraceError(e.Message);
            }));

            foreach (var s in schemas)
            {
                Set.Add(s);
            }

            Set.Compile();

            BuildModel();

            var namespaces = GenerateCode();
            namespaces = HackVirtualProperties(RemoveEnumPropertyTypes(CleanUnusedTypes(namespaces)));

            var provider = new Microsoft.CSharp.CSharpCodeProvider();

            foreach (var ns in namespaces)
            {
                var compileUnit = new CodeCompileUnit();
                compileUnit.Namespaces.Add(ns);

                var title = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(),
                    typeof(AssemblyTitleAttribute))).Title;
                var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                ns.Comments.Add(new CodeCommentStatement(string.Format("This code was generated by {0} version {1}.", title, version)));

                using (StringWriter sw = new StringWriter())
                {
                    provider.GenerateCodeFromCompileUnit(compileUnit, sw, new CodeGeneratorOptions { VerbatimOrder = true, BracingStyle = "C" });
                    var s = sw.ToString().Replace("};", "}"); // remove ';' at end of automatic properties
                    var flname = ns.Name;
                    if (!string.IsNullOrEmpty(_configuration.RootClassName))
                        flname = _configuration.RootClassName;

                    var pathParts = ns.Name.Split('.');
                    for (int i = 0; i < pathParts.Length; i++)
                    {
                        OutputFolder = Path.Combine(OutputFolder, pathParts[i]);
                    }

                    if (!Directory.Exists(OutputFolder))
                        Directory.CreateDirectory(OutputFolder);

                    var path = Path.Combine(OutputFolder, flname + ".cs");
                    if (Log != null) Log(path);
                    File.WriteAllText(path, s);
                }
            }
        }

        private IEnumerable<CodeNamespace> HackVirtualProperties(IEnumerable<CodeNamespace> hackableTypes)
        {
            if (!_configuration.EntityFramework) return hackableTypes;
            var hackVirtualProperties = hackableTypes as CodeNamespace[] ?? hackableTypes.ToArray();
            foreach (var codeNamespace in hackVirtualProperties)
            {
                foreach (CodeTypeDeclaration s in codeNamespace.Types)
                {
                    foreach (var m in s.Members)
                    {
                        var field = m as CodeMemberField;
                        if (field != null)
                        {
                            if (_configuration.EntityFramework &&
                                (field.Type.BaseType.Contains("System.Collections") || !field.Type.BaseType.Contains("System.")) &&
                                char.IsUpper(field.Name[0]) && !field.Type.BaseType.StartsWith("virtual"))
                                field.Type.BaseType = "virtual " + field.Type.BaseType;
                        }
                    }
                }
            }

            return hackVirtualProperties;
        }

        private IEnumerable<CodeNamespace> CleanUnusedTypes(IEnumerable<CodeNamespace> source)
        {
            var result = new List<CodeNamespace>();
            foreach (var codeNamespace in source)
            {
                
                bool itsRoot = true;
                var typesArray = new CodeTypeDeclaration[codeNamespace.Types.Count];                
                codeNamespace.Types.CopyTo(typesArray, 0);
                CodeTypeDeclarationCollection types = new CodeTypeDeclarationCollection(typesArray);

                codeNamespace.Types.Clear();
                foreach (CodeTypeDeclaration type in types)
                {
                    if (itsRoot)
                    {
                        codeNamespace.Types.Add(type);
                        if(_configuration.EntityFramework || _configuration.ViewModel)
                            type.BaseTypes.Add("CAFE.Core.Resources.AccessibleResourceDescriptorBase");
                        itsRoot = false;
                        continue;
                    }
                    
                    if (type.IsClass)
                    {
                        ChangeTypeNamesDependentOfTypeOfModel(type, codeNamespace.Types);
                    }
                    if(IsThatTypeUsed(type, types))
                    {
                        codeNamespace.Types.Add(type);
                    }
                }

                result.Add(codeNamespace);
            }

            return result;
        }

        private bool IsThatTypeUsed(CodeTypeDeclaration type, CodeTypeDeclarationCollection source)
        {
            var result = false;
            foreach (CodeTypeDeclaration s in source)
            {
                if(result) break;
                foreach (var m in s.Members)
                {
                    var field = m as CodeMemberField;
                    if (field != null)
                    {
                        if (field.Type.BaseType == type.Name || field.Type.BaseType.EndsWith("<" + type.Name + ">") || field.Type.BaseType == "virtual " + type.Name)
                        {
                            result = true;
                            break;
                        }
                    }
                    var property = m as CodeMemberProperty;
                    if (property != null)
                    {
                        if (property.Type.BaseType == type.Name || property.Type.BaseType.EndsWith("<" + type.Name + ">"))
                        {
                            result = true;
                            break;
                        }
                    }

                }
            }

            return result;
        }

        private IEnumerable<CodeNamespace> RemoveEnumPropertyTypes(IEnumerable<CodeNamespace> source)
        {
            var result = new List<CodeNamespace>();
            foreach (var codeNamespace in source)
            {
                foreach (CodeTypeDeclaration type in codeNamespace.Types)
                {
                    if (type.IsEnum)
                    {
                        ChangeTypeToString(type, codeNamespace.Types);
                    }      
                }

                result.Add(codeNamespace);
            }

            return result;
        }


        private void ChangeTypeToString(CodeTypeDeclaration type, CodeTypeDeclarationCollection source)
        {
            foreach (CodeTypeDeclaration s in source)
            {
                foreach (var m in s.Members)
                {
                    var field = m as CodeMemberField;
                    var propety = m as CodeMemberProperty;
                    if (field != null)
                    {
                        if (field.Type.BaseType == type.Name)
                        {
                            field.Type.BaseType = "System.String";
                        }

                    }
                    if (propety != null)
                    {
                        if (propety.Type.BaseType == type.Name)
                        {
                            propety.Type.BaseType = "System.String";
                        }
                    }

                }
            }
        }

        private void ChangeTypeNamesDependentOfTypeOfModel(CodeTypeDeclaration type,
            CodeTypeDeclarationCollection source)
        {
            var oldTypeName = type.Name;
            if (_configuration.EntityFramework && !type.Name.StartsWith("Db"))
            {
                type.Name = "Db" + type.Name;
            }
            else if (_configuration.ViewModel && !type.Name.EndsWith("Model"))
            {
                type.Name += "Model";
            }
            type.Name = ToAliased(type.Name);

            foreach (CodeTypeDeclaration s in source)
            {
                foreach (var m in s.Members)
                {
                    var field = m as CodeMemberField;
                    if (field != null)
                    {
                        if (field.Type.BaseType == oldTypeName || field.Type.BaseType.EndsWith("<" + oldTypeName + ">"))
                        {
                            field.Type.BaseType = field.Type.BaseType.Replace(oldTypeName, type.Name);
                        }
                        //if (_configuration.EntityFramework && 
                        //    (field.Type.BaseType.Contains("System.Collections") || !field.Type.BaseType.Contains("System.")) && 
                        //    char.IsUpper(field.Name[0]) && !field.Type.BaseType.StartsWith("virtual"))
                        //    field.Type.BaseType = "virtual " + field.Type.BaseType;


                        //else if (_configuration.EntityFramework &&
                        //         !char.IsUpper(field.Name[0]) && field.Type.BaseType.StartsWith("virtual"))
                        //{
                        //    field.Type.BaseType = field.Type.BaseType.Replace("virtual ", "");
                        //}

                    }
                    var property = m as CodeMemberProperty;
                    if (property != null)
                    {
                        if (property.Type.BaseType == oldTypeName || property.Type.BaseType.EndsWith("<" + oldTypeName + ">"))
                        {
                            property.Type.BaseType = property.Type.BaseType.Replace(oldTypeName, type.Name);
                        }
                    }

                    var constr = m as CodeConstructor;
                    if (constr != null)
                    {
                        if (constr.Statements.Count > 0)
                        {
                            foreach (CodeAssignStatement statement in constr.Statements)
                            {
                                var createExpr = (CodeObjectCreateExpression) statement.Right;
                                if (createExpr.CreateType.BaseType == oldTypeName ||
                                    createExpr.CreateType.BaseType.EndsWith("<" + oldTypeName + ">"))
                                {
                                    createExpr.CreateType.BaseType = createExpr.CreateType.BaseType.Replace(oldTypeName, type.Name);
                                }
                            }
                        }
                    }
                }
            }
        }


        private IEnumerable<CodeNamespace> GenerateCode()
        {
            var hierarchy = NamespaceHierarchyItem.Build(Namespaces.Values.GroupBy(x => x.Name).SelectMany(x => x))
                .MarkAmbiguousNamespaceTypes();
            return hierarchy.Flatten()
                .Select(nhi => NamespaceModel.Generate(nhi.FullName, nhi.Models, _configuration));
        }

        private string BuildNamespace(Uri source, string xmlNamespace)
        {
            var key = new NamespaceKey(source, xmlNamespace);
            var result = NamespaceProvider.FindNamespace(key);
            if (string.IsNullOrEmpty(result))
                result = "CAFE.Core.Integration";
            if (!string.IsNullOrEmpty(result))
                return result;

            throw new Exception(string.Format("Namespace {0} not provided through map or generator.", xmlNamespace));
        }

        private static readonly Dictionary<char, string> InvalidChars = CreateInvalidChars();

        private static Dictionary<char, string> CreateInvalidChars()
        {
            var invalidChars = new Dictionary<char, string>();

            invalidChars['\x00'] = "Null";
            invalidChars['\x01'] = "StartOfHeading";
            invalidChars['\x02'] = "StartOfText";
            invalidChars['\x03'] = "EndOfText";
            invalidChars['\x04'] = "EndOfTransmission";
            invalidChars['\x05'] = "Enquiry";
            invalidChars['\x06'] = "Acknowledge";
            invalidChars['\x07'] = "Bell";
            invalidChars['\x08'] = "Backspace";
            invalidChars['\x09'] = "HorizontalTab";
            invalidChars['\x0A'] = "LineFeed";
            invalidChars['\x0B'] = "VerticalTab";
            invalidChars['\x0C'] = "FormFeed";
            invalidChars['\x0D'] = "CarriageReturn";
            invalidChars['\x0E'] = "ShiftOut";
            invalidChars['\x0F'] = "ShiftIn";
            invalidChars['\x10'] = "DataLinkEscape";
            invalidChars['\x11'] = "DeviceControl1";
            invalidChars['\x12'] = "DeviceControl2";
            invalidChars['\x13'] = "DeviceControl3";
            invalidChars['\x14'] = "DeviceControl4";
            invalidChars['\x15'] = "NegativeAcknowledge";
            invalidChars['\x16'] = "SynchronousIdle";
            invalidChars['\x17'] = "EndOfTransmissionBlock";
            invalidChars['\x18'] = "Cancel";
            invalidChars['\x19'] = "EndOfMedium";
            invalidChars['\x1A'] = "Substitute";
            invalidChars['\x1B'] = "Escape";
            invalidChars['\x1C'] = "FileSeparator";
            invalidChars['\x1D'] = "GroupSeparator";
            invalidChars['\x1E'] = "RecordSeparator";
            invalidChars['\x1F'] = "UnitSeparator";
            //invalidChars['\x20'] = "Space";
            invalidChars['\x21'] = "ExclamationMark";
            invalidChars['\x22'] = "Quote";
            invalidChars['\x23'] = "Hash";
            invalidChars['\x24'] = "Dollar";
            invalidChars['\x25'] = "Percent";
            invalidChars['\x26'] = "Ampersand";
            invalidChars['\x27'] = "SingleQuote";
            invalidChars['\x28'] = "LeftParenthesis";
            invalidChars['\x29'] = "RightParenthesis";
            invalidChars['\x2A'] = "Asterisk";
            invalidChars['\x2B'] = "Plus";
            invalidChars['\x2C'] = "Comma";
            //invalidChars['\x2D'] = "Minus";
            invalidChars['\x2E'] = "Period";
            invalidChars['\x2F'] = "Slash";
            invalidChars['\x3A'] = "Colon";
            invalidChars['\x3B'] = "Semicolon";
            invalidChars['\x3C'] = "LessThan";
            invalidChars['\x3D'] = "Equal";
            invalidChars['\x3E'] = "GreaterThan";
            invalidChars['\x3F'] = "QuestionMark";
            invalidChars['\x40'] = "At";
            invalidChars['\x5B'] = "LeftSquareBracket";
            invalidChars['\x5C'] = "Backslash";
            invalidChars['\x5D'] = "RightSquareBracket";
            invalidChars['\x5E'] = "Caret";
            //invalidChars['\x5F'] = "Underscore";
            invalidChars['\x60'] = "Backquote";
            invalidChars['\x7B'] = "LeftCurlyBrace";
            invalidChars['\x7C'] = "Pipe";
            invalidChars['\x7D'] = "RightCurlyBrace";
            invalidChars['\x7E'] = "Tilde";
            invalidChars['\x7F'] = "Delete";

            return invalidChars;
        }

        private static readonly Regex InvalidCharsRegex = CreateInvalidCharsRegex();

        private static Regex CreateInvalidCharsRegex()
        {
            var r = string.Join("", InvalidChars.Keys.Select(c => string.Format(@"\x{0:x2}", (int)c)).ToArray());
            return new Regex("[" + r + "]", RegexOptions.Compiled);
        }

        private static readonly CodeDomProvider Provider = new Microsoft.CSharp.CSharpCodeProvider();
        public static string MakeValidIdentifier(string s)
        {
            var id = InvalidCharsRegex.Replace(s, m => InvalidChars[m.Value[0]]);
            return Provider.CreateValidIdentifier(Regex.Replace(id, @"\W+", "_"));
        }

        public string ToTitleCase(string s)
        {
            return ToTitleCase(s, NamingScheme);
        }

        public static string ToTitleCase(string s, NamingScheme namingScheme)
        {
            if (string.IsNullOrEmpty(s)) return s;
            switch (namingScheme)
            {
                case NamingScheme.PascalCase:
                    s = s.ToPascalCase();
                    break;
            }
            return MakeValidIdentifier(s);
        }

        private void BuildModel()
        {
            var objectModel = new SimpleModel(_configuration)
            {
                Name = "AnyType",
                Namespace = CreateNamespaceModel(new Uri(XmlSchema.Namespace), AnyType),
                XmlSchemaName = AnyType,
                XmlSchemaType = null,
                ValueType = typeof(object),
                UseDataTypeAttribute = false
            };

            Types[AnyType] = objectModel;

            AttributeGroups = Set.Schemas().Cast<XmlSchema>().SelectMany(s => s.AttributeGroups.Values.Cast<XmlSchemaAttributeGroup>()).ToDictionary(g => g.QualifiedName);
            Groups = Set.Schemas().Cast<XmlSchema>().SelectMany(s => s.Groups.Values.Cast<XmlSchemaGroup>()).ToDictionary(g => g.QualifiedName);

            foreach (var rootElement in Set.GlobalElements.Values.Cast<XmlSchemaElement>())
            {
                var source = new Uri(rootElement.GetSchema().SourceUri);
                var qualifiedName = rootElement.ElementSchemaType.QualifiedName;
                if (qualifiedName.IsEmpty) qualifiedName = rootElement.QualifiedName;
                if (!string.IsNullOrEmpty(_configuration.RootClassName))
                {
                    qualifiedName = new XmlQualifiedName(_configuration.RootClassName, "");
                }
                var type = CreateTypeModel(source, rootElement.ElementSchemaType, qualifiedName);
                if(type == null) continue;


                //if (_configuration.EntityFramework || _configuration.ViewModel)
                //{
                    //var tp = type as ClassModel;
                    //if (tp != null)
                    //{
                    //    tp.BaseClass = new ClassModel(_configuration)
                    //    {
                    //        Name = "AccessibleResourceDescriptorBase",
                    //        Namespace = new NamespaceModel(new NamespaceKey(""), _configuration)
                    //    };
                    //}
                //}

                if (type.RootElementName != null)
                {
                    if (type is ClassModel)
                    {
                        // There is already another global element with this type.
                        // Need to create an empty derived class.

                        var derivedClassModel = new ClassModel(_configuration)
                        {
                            Name = ToTitleCase(ToAliased(rootElement.QualifiedName.Name)),
                            Namespace = CreateNamespaceModel(source, rootElement.QualifiedName)
                        };

                        derivedClassModel.Documentation.AddRange(GetDocumentation(rootElement));

                        if (derivedClassModel.Namespace != null)
                        {
                            derivedClassModel.Name = derivedClassModel.Namespace.GetUniqueTypeName(derivedClassModel.Name);
                            derivedClassModel.Namespace.Types[derivedClassModel.Name] = derivedClassModel;
                        }

                        Types[rootElement.QualifiedName] = derivedClassModel;

                        derivedClassModel.BaseClass = (ClassModel)type;
                        ((ClassModel)derivedClassModel.BaseClass).DerivedTypes.Add(derivedClassModel);

                        derivedClassModel.RootElementName = rootElement.QualifiedName;
                    }
                    else
                    {
                        Types[rootElement.QualifiedName] = type;
                    }
                }
                else
                {
                    var classModel = type as ClassModel;
                    if (classModel != null)
                    {
                        classModel.Documentation.AddRange(GetDocumentation(rootElement));
                    }

                    type.RootElementName = rootElement.QualifiedName;
                }
            }

            foreach (var globalType in Set.GlobalTypes.Values.Cast<XmlSchemaType>())
            {
                var schema = globalType.GetSchema();
                var source = (schema == null ? null : new Uri(schema.SourceUri));
                var type = CreateTypeModel(source, globalType, globalType.QualifiedName);
            }
        }

        private string ToAliased(string name)
        {
            if (_configuration.Aliases.ContainsKey(name.ToLower()))
            {
                return _configuration.Aliases[name.ToLower()];
            }
            return name;
        }

        // see http://msdn.microsoft.com/en-us/library/z2w0sxhf.aspx
        private static readonly HashSet<string> EnumTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "string", "normalizedString", "token", "Name", "NCName", "ID", "ENTITY", "NMTOKEN" };

        // ReSharper disable once FunctionComplexityOverflow
        private TypeModel CreateTypeModel(Uri source, XmlSchemaAnnotated type, XmlQualifiedName qualifiedName)
        {
            TypeModel typeModel;
            if (!qualifiedName.IsEmpty && Types.TryGetValue(qualifiedName, out typeModel)) return typeModel;
            if (source == null)
                throw new ArgumentNullException("source");
            var namespaceModel = CreateNamespaceModel(source, qualifiedName);

            var docs = GetDocumentation(type);

            var group = type as XmlSchemaGroup;
            if (group != null)
            {
                var name = "I" + ToTitleCase(qualifiedName.Name);
                if (namespaceModel != null) name = namespaceModel.GetUniqueTypeName(name);

                var interfaceModel = new InterfaceModel(_configuration)
                {
                    Name = name,
                    Namespace = namespaceModel,
                    XmlSchemaName = qualifiedName
                };

                interfaceModel.Documentation.AddRange(docs);

                if (namespaceModel != null) namespaceModel.Types[name] = interfaceModel;
                if (!qualifiedName.IsEmpty) Types[qualifiedName] = interfaceModel;

                var particle = group.Particle;
                var items = GetElements(particle);
                var properties = CreatePropertiesForElements(source, interfaceModel, particle, items.Where(i => !(i.XmlParticle is XmlSchemaGroupRef)));
                interfaceModel.Properties.AddRange(properties);
                var interfaces = items.Select(i => i.XmlParticle).OfType<XmlSchemaGroupRef>()
                    .Select(i => (InterfaceModel)CreateTypeModel(new Uri(i.SourceUri), Groups[i.RefName], i.RefName));
                interfaceModel.Interfaces.AddRange(interfaces);

                return interfaceModel;
            }

            var attributeGroup = type as XmlSchemaAttributeGroup;
            if (attributeGroup != null)
            {
                var name = "I" + ToTitleCase(qualifiedName.Name);
                if (namespaceModel != null) name = namespaceModel.GetUniqueTypeName(name);

                var interfaceModel = new InterfaceModel(_configuration)
                {
                    Name = name,
                    Namespace = namespaceModel,
                    XmlSchemaName = qualifiedName
                };

                interfaceModel.Documentation.AddRange(docs);

                if (namespaceModel != null) namespaceModel.Types[name] = interfaceModel;
                if (!qualifiedName.IsEmpty) Types[qualifiedName] = interfaceModel;

                var items = attributeGroup.Attributes;
                var properties = CreatePropertiesForAttributes(source, interfaceModel, items.OfType<XmlSchemaAttribute>());
                interfaceModel.Properties.AddRange(properties);
                var interfaces = items.OfType<XmlSchemaAttributeGroupRef>()
                    .Select(a => (InterfaceModel)CreateTypeModel(new Uri(a.SourceUri), AttributeGroups[a.RefName], a.RefName));
                interfaceModel.Interfaces.AddRange(interfaces);

                return interfaceModel;
            }

            var complexType = type as XmlSchemaComplexType;
            if (complexType != null)
            {
                var name = ToTitleCase(ToAliased(qualifiedName.Name));
                if (namespaceModel != null) name = namespaceModel.GetUniqueTypeName(name);

                var classModel = new ClassModel(_configuration)
                {
                    Name = name,
                    Namespace = namespaceModel,
                    XmlSchemaName = qualifiedName,
                    XmlSchemaType = complexType,
                    IsAbstract = complexType.IsAbstract,
                    IsAnonymous = complexType.QualifiedName.Name == "",
                    IsMixed = complexType.IsMixed,
                    IsSubstitution = complexType.Parent is XmlSchemaElement && !((XmlSchemaElement)complexType.Parent).SubstitutionGroup.IsEmpty,
                    EnableDataBinding = EnableDataBinding,
                };

                classModel.Documentation.AddRange(docs);

                var sequence = complexType.ContentTypeParticle as XmlSchemaSequence;
                if (sequence != null && sequence.Items.Count > 0)
                {
                    XmlSchemaParticle firstItem = (XmlSchemaParticle)sequence.Items[0];
                    if (firstItem.MaxOccursString == "unbounded" && firstItem.MinOccurs == 1)
                    {
                        //at least one attribute
                        classModel.Restrictions.AddRange(new List<RestrictionModel>() { new AtLeastOneRestrictionModel(_configuration) });
                    }
                }

                var elemType = complexType.Parent as XmlSchemaElement;
                if (elemType != null)
                {
                    if (elemType.MinOccurs == 1 && elemType.MaxOccurs == 1)
                    {
                        classModel.Restrictions.AddRange(new List<RestrictionModel>() { new MinInclusiveRestrictionModel(_configuration) { Value = "1" }, new MaxInclusiveRestrictionModel(_configuration) { Value = "1" } });
                    }
                }
                else
                {
                    if (sequence != null && sequence.MinOccurs == 1 && sequence.MaxOccurs == 1)
                    {
                        classModel.Restrictions.AddRange(new List<RestrictionModel>() { new MinInclusiveRestrictionModel(_configuration) { Value = "1" }, new MaxInclusiveRestrictionModel(_configuration) { Value = "1" } });
                    }
                }


                if (namespaceModel != null)
                {
                    namespaceModel.Types[classModel.Name] = classModel;
                }

                if (!qualifiedName.IsEmpty) Types[qualifiedName] = classModel;

                if (complexType.BaseXmlSchemaType != null && complexType.BaseXmlSchemaType.QualifiedName != AnyType)
                {
                    var baseModel = CreateTypeModel(source, complexType.BaseXmlSchemaType, complexType.BaseXmlSchemaType.QualifiedName);
                    classModel.BaseClass = baseModel;
                    if (baseModel is ClassModel) ((ClassModel)classModel.BaseClass).DerivedTypes.Add(classModel);
                }

                XmlSchemaParticle particle = null;
                if (classModel.BaseClass != null)
                {
                    if (complexType.ContentModel.Content is XmlSchemaComplexContentExtension)
                        particle = ((XmlSchemaComplexContentExtension)complexType.ContentModel.Content).Particle;

                    // If it's a restriction, do not duplicate elements on the derived class, they're already in the base class.
                    // See https://msdn.microsoft.com/en-us/library/f3z3wh0y.aspx
                    //else if (complexType.ContentModel.Content is XmlSchemaComplexContentRestriction)
                    //    particle = ((XmlSchemaComplexContentRestriction)complexType.ContentModel.Content).Particle;
                }
                else particle = complexType.ContentTypeParticle;

                var items = GetElements(particle);
                var properties = CreatePropertiesForElements(source, classModel, particle, items);
                classModel.Properties.AddRange(properties);

                if (GenerateInterfaces)
                {
                    var interfaces = items.Select(i => i.XmlParticle).OfType<XmlSchemaGroupRef>()
                        .Select(i => (InterfaceModel)CreateTypeModel(new Uri(i.SourceUri), Groups[i.RefName], i.RefName));
                    classModel.Interfaces.AddRange(interfaces);
                }

                XmlSchemaObjectCollection attributes = null;
                if (classModel.BaseClass != null)
                {
                    if (complexType.ContentModel.Content is XmlSchemaComplexContentExtension)
                        attributes = ((XmlSchemaComplexContentExtension)complexType.ContentModel.Content).Attributes;
                    else if (complexType.ContentModel.Content is XmlSchemaSimpleContentExtension)
                        attributes = ((XmlSchemaSimpleContentExtension)complexType.ContentModel.Content).Attributes;

                    // If it's a restriction, do not duplicate attributes on the derived class, they're already in the base class.
                    // See https://msdn.microsoft.com/en-us/library/f3z3wh0y.aspx
                    //else if (complexType.ContentModel.Content is XmlSchemaComplexContentRestriction)
                    //    attributes = ((XmlSchemaComplexContentRestriction)complexType.ContentModel.Content).Attributes;
                    //else if (complexType.ContentModel.Content is XmlSchemaSimpleContentRestriction)
                    //    attributes = ((XmlSchemaSimpleContentRestriction)complexType.ContentModel.Content).Attributes;
                }
                else attributes = complexType.Attributes;

                if (attributes != null)
                {
                    var attributeProperties = CreatePropertiesForAttributes(source, classModel, attributes.Cast<XmlSchemaObject>());
                    classModel.Properties.AddRange(attributeProperties);

                    if (GenerateInterfaces)
                    {
                        var attributeInterfaces = attributes.OfType<XmlSchemaAttributeGroupRef>()
                            .Select(i => (InterfaceModel)CreateTypeModel(new Uri(i.SourceUri), AttributeGroups[i.RefName], i.RefName));
                        classModel.Interfaces.AddRange(attributeInterfaces);
                    }
                }

                if (complexType.AnyAttribute != null)
                {
                    var property = new PropertyModel(_configuration)
                    {
                        OwningType = classModel,
                        Name = "AnyAttribute",
                        Type = new SimpleModel(_configuration) { ValueType = typeof(XmlAttribute), UseDataTypeAttribute = false },
                        IsAttribute = true,
                        IsCollection = true,
                        IsAny = true
                    };

                    var attributeDocs = GetDocumentation(complexType.AnyAttribute);
                    property.Documentation.AddRange(attributeDocs);

                    classModel.Properties.Add(property);
                }

                return classModel;
            }

            var simpleType = type as XmlSchemaSimpleType;
            if (simpleType != null)
            {
                var restrictions = new List<RestrictionModel>();

                var typeRestriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;
                if (typeRestriction != null)
                {
                    var enumFacets = typeRestriction.Facets.OfType<XmlSchemaEnumerationFacet>().ToList();
                    var isEnum = (enumFacets.Count == typeRestriction.Facets.Count && enumFacets.Count != 0)
                                    || (EnumTypes.Contains(typeRestriction.BaseTypeName.Name) && enumFacets.Any());

                    if (isEnum && (!_configuration.EntityFramework && !_configuration.ViewModel))
                    {
                        //if (_configuration.EntityFramework || _configuration.ViewModel) return null;

                        // we got an enum
                        var name = ToTitleCase(qualifiedName.Name);
                        if (namespaceModel != null) name = namespaceModel.GetUniqueTypeName(name);

                        var enumModel = new EnumModel(_configuration)
                        {
                            Name = name,
                            Namespace = namespaceModel,
                            XmlSchemaName = qualifiedName,
                            XmlSchemaType = simpleType,
                        };

                        enumModel.Documentation.AddRange(docs);

                        foreach (var facet in enumFacets.DistinctBy(f => f.Value))
                        {
                            var value = new EnumValueModel
                            {
                                Name = ToTitleCase(facet.Value).ToNormalizedEnumName(),
                                Value = facet.Value
                            };

                            var valueDocs = GetDocumentation(facet);
                            value.Documentation.AddRange(valueDocs);

                            var deprecated = facet.Annotation != null && facet.Annotation.Items.OfType<XmlSchemaAppInfo>()
                                .Any(a => a.Markup.Any(m => m.Name == "annox:annotate" && m.HasChildNodes && m.FirstChild.Name == "jl:Deprecated"));
                            value.IsDeprecated = deprecated;

                            enumModel.Values.Add(value);
                        }

                        if (namespaceModel != null)
                        {
                            namespaceModel.Types[enumModel.Name] = enumModel;
                        }

                        if (!qualifiedName.IsEmpty) Types[qualifiedName] = enumModel;

                        return enumModel;
                    }


                    //if (isEnum && !_configuration.ViewModel)
                    //{
                    //    //if (_configuration.EntityFramework || _configuration.ViewModel) return null;

                    //    // we got an enum
                    //    var name = ToTitleCase(qualifiedName.Name);
                    //    if (namespaceModel != null) name = namespaceModel.GetUniqueTypeName(name);

                    //    var enumModel = new EnumModel(_configuration)
                    //    {
                    //        Name = name,
                    //        Namespace = namespaceModel,
                    //        XmlSchemaName = qualifiedName,
                    //        XmlSchemaType = simpleType,
                    //    };

                    //    enumModel.Documentation.AddRange(docs);

                    //    foreach (var facet in enumFacets.DistinctBy(f => f.Value))
                    //    {
                    //        var value = new EnumValueModel
                    //        {
                    //            Name = ToTitleCase(facet.Value).ToNormalizedEnumName(),
                    //            Value = facet.Value
                    //        };

                    //        var valueDocs = GetDocumentation(facet);
                    //        value.Documentation.AddRange(valueDocs);

                    //        var deprecated = facet.Annotation != null && facet.Annotation.Items.OfType<XmlSchemaAppInfo>()
                    //            .Any(a => a.Markup.Any(m => m.Name == "annox:annotate" && m.HasChildNodes && m.FirstChild.Name == "jl:Deprecated"));
                    //        value.IsDeprecated = deprecated;

                    //        enumModel.Values.Add(value);
                    //    }

                    //    if (namespaceModel != null)
                    //    {
                    //        namespaceModel.Types[enumModel.Name] = enumModel;
                    //    }

                    //    if (!qualifiedName.IsEmpty) Types[qualifiedName] = enumModel;

                    //    return enumModel;
                    //}

                    restrictions = GetRestrictions(typeRestriction.Facets.Cast<XmlSchemaFacet>(), simpleType).Where(r => r != null).Sanitize().ToList();
                }

                var simpleModelName = ToTitleCase(ToAliased(qualifiedName.Name));
                if (namespaceModel != null) simpleModelName = namespaceModel.GetUniqueTypeName(simpleModelName);

                var simpleModel = new SimpleModel(_configuration)
                {
                    Name = simpleModelName,
                    Namespace = namespaceModel,
                    XmlSchemaName = qualifiedName,
                    XmlSchemaType = simpleType,
                    ValueType = simpleType.Datatype.GetEffectiveType(_configuration),
                };

                simpleModel.Documentation.AddRange(docs);
                simpleModel.Restrictions.AddRange(restrictions);

                if (namespaceModel != null)
                {
                    namespaceModel.Types[simpleModel.Name] = simpleModel;
                }

                if (!qualifiedName.IsEmpty) Types[qualifiedName] = simpleModel;

                return simpleModel;
            }

            throw new Exception(string.Format("Cannot build declaration for {0}", qualifiedName));
        }

        private IEnumerable<PropertyModel> CreatePropertiesForAttributes(Uri source, TypeModel typeModel, IEnumerable<XmlSchemaObject> items)
        {
            var properties = new List<PropertyModel>();

            foreach (var item in items)
            {
                var attribute = item as XmlSchemaAttribute;
                if (attribute != null)
                {
                    if (attribute.Use != XmlSchemaUse.Prohibited)
                    {
                        var attributeQualifiedName = attribute.AttributeSchemaType.QualifiedName;

                        if (attributeQualifiedName.IsEmpty)
                        {
                            attributeQualifiedName = attribute.QualifiedName;

                            if (attributeQualifiedName.IsEmpty || attributeQualifiedName.Namespace == "")
                            {
                                // inner type, have to generate a type name

                                //GENERATED NAMES TOO LONG
                                //var typeName = ToTitleCase(typeModel.Name) + ToTitleCase(attribute.QualifiedName.Name);
                                var typeName = ToTitleCase(attribute.QualifiedName.Name);
                                attributeQualifiedName = new XmlQualifiedName(typeName, typeModel.XmlSchemaName.Namespace);
                                // try to avoid name clashes
                                if (NameExists(attributeQualifiedName))
                                    attributeQualifiedName = new[] { "Item", "Property", "Element" }
                                        .Select(s => new XmlQualifiedName(attributeQualifiedName.Name + s, attributeQualifiedName.Namespace))
                                        .First(n => !NameExists(n));
                            }
                        }

                        var attributeName = ToTitleCase(attribute.QualifiedName.Name);
                        if (attributeName == typeModel.Name) attributeName += "Property"; // member names cannot be the same as their enclosing type


                        var property = new PropertyModel(_configuration)
                        {
                            OwningType = typeModel,
                            Name = attributeName,
                            XmlSchemaName = attribute.QualifiedName,
                            Type = CreateTypeModel(source, attribute.AttributeSchemaType, attributeQualifiedName),
                            IsAttribute = true,
                            IsNullable = attribute.Use != XmlSchemaUse.Required,
                            DefaultValue = attribute.DefaultValue,
                            Form = attribute.Form == XmlSchemaForm.None ? attribute.GetSchema().AttributeFormDefault : attribute.Form,
                            XmlNamespace = attribute.QualifiedName.Namespace != "" && attribute.QualifiedName.Namespace != typeModel.XmlSchemaName.Namespace ? attribute.QualifiedName.Namespace : null,
                        };

                        var attributeDocs = GetDocumentation(attribute);
                        property.Documentation.AddRange(attributeDocs);

                        properties.Add(property);
                    }
                }
                else
                {
                    var attributeGroupRef = item as XmlSchemaAttributeGroupRef;
                    if (attributeGroupRef != null)
                    {
                        if (GenerateInterfaces)
                            CreateTypeModel(new Uri(attributeGroupRef.SourceUri), AttributeGroups[attributeGroupRef.RefName], attributeGroupRef.RefName);

                        var groupItems = AttributeGroups[attributeGroupRef.RefName].Attributes;
                        var groupProperties = CreatePropertiesForAttributes(source, typeModel, groupItems.Cast<XmlSchemaObject>());
                        properties.AddRange(groupProperties);
                    }
                }
            }

            return properties;
        }

        private IEnumerable<PropertyModel> CreatePropertiesForElements(Uri source, TypeModel typeModel, XmlSchemaParticle particle,  IEnumerable<Particle> items)
        {
            var properties = new List<PropertyModel>();
            var order = 0;
            foreach (var item in items)
            {
                PropertyModel property = null;

                var element = item.XmlParticle as XmlSchemaElement;
                // ElementSchemaType must be non-null. This is not the case when maxOccurs="0".
                if (element != null && element.ElementSchemaType != null)
                {
                    var elementQualifiedName = element.ElementSchemaType.QualifiedName;

                    if (elementQualifiedName.IsEmpty)
                    {
                        elementQualifiedName = element.RefName;

                        if (elementQualifiedName.IsEmpty)
                        {
                            // inner type, have to generate a type name

                            //GENERATED NAMES TOO LONG
                            //var typeName = ToTitleCase(typeModel.Name) + ToTitleCase(element.QualifiedName.Name);
                            var typeName = ToTitleCase(ToAliased(element.QualifiedName.Name));
                            elementQualifiedName = new XmlQualifiedName(typeName, typeModel.XmlSchemaName.Namespace);
                            // try to avoid name clashes
                            if (NameExists(elementQualifiedName))
                                elementQualifiedName = new[] { "Item", "Property", "Element" }
                                    .Select(s => new XmlQualifiedName(elementQualifiedName.Name + s, elementQualifiedName.Namespace))
                                    .First(n => !NameExists(n));
                        }
                    }

                    var propertyName = ToTitleCase(element.QualifiedName.Name);
                    if (propertyName == typeModel.Name) propertyName += "Property"; // member names cannot be the same as their enclosing type

                    var tm = CreateTypeModel(source, element.ElementSchemaType, elementQualifiedName);
                    property = new PropertyModel(_configuration)
                    {
                        OwningType = typeModel,
                        XmlSchemaName = element.QualifiedName,
                        Name = propertyName,
                        Type = tm,
                        IsNillable = element.IsNillable,
                        IsNullable = item.MinOccurs < 1.0m,
                        IsCollection = item.MaxOccurs > 1.0m || particle.MaxOccurs > 1.0m, // http://msdn.microsoft.com/en-us/library/vstudio/d3hx2s7e(v=vs.100).aspx
                        DefaultValue = element.DefaultValue,
                        Form = element.Form == XmlSchemaForm.None ? element.GetSchema().ElementFormDefault : element.Form,
                        XmlNamespace = element.QualifiedName.Namespace != "" && element.QualifiedName.Namespace != typeModel.XmlSchemaName.Namespace ? element.QualifiedName.Namespace : null,
                    };

                    if (item.MinOccurs == 1 && item.MaxOccurs == 1)
                    {
                        property.Restrictions.AddRange(new List<RestrictionModel>() { new MinInclusiveRestrictionModel(_configuration) { Value = "1" }, new MaxInclusiveRestrictionModel(_configuration) { Value = "1" } });
                    }            
                }
                else
                {
                    var any = item.XmlParticle as XmlSchemaAny;
                    if (any != null)
                    {
                        property = new PropertyModel(_configuration)
                        {
                            OwningType = typeModel,
                            Name = "Any",
                            Type = new SimpleModel(_configuration) { ValueType = (UseXElementForAny ? typeof(XElement) : typeof(XmlElement)), UseDataTypeAttribute = false },
                            IsNullable = item.MinOccurs < 1.0m,
                            IsCollection = item.MaxOccurs > 1.0m || particle.MaxOccurs > 1.0m, // http://msdn.microsoft.com/en-us/library/vstudio/d3hx2s7e(v=vs.100).aspx
                            IsAny = true,
                        };

                        if (item.MinOccurs == 1 && item.MaxOccurs == 1)
                        {
                            property.Restrictions.AddRange(new List<RestrictionModel>() { new MinInclusiveRestrictionModel(_configuration) { Value = "1" }, new MaxInclusiveRestrictionModel(_configuration) { Value = "1" } });
                        }

                    }
                    else
                    {
                        var groupRef = item.XmlParticle as XmlSchemaGroupRef;
                        if (groupRef != null)
                        {
                            if (GenerateInterfaces)
                                CreateTypeModel(new Uri(groupRef.SourceUri), Groups[groupRef.RefName], groupRef.RefName);

                            var groupItems = GetElements(groupRef.Particle);
                            var groupProperties = CreatePropertiesForElements(source, typeModel, item.XmlParticle, groupItems);
                            properties.AddRange(groupProperties);
                        }
                    }
                }

                // Discard duplicate property names. This is most likely due to:
                // - Choice or
                // - Element and attribute with the same name
                if (property != null && !properties.Any(p => p.Name == property.Name))
                {
                    var itemDocs = GetDocumentation(item.XmlParticle);
                    property.Documentation.AddRange(itemDocs);

                    if (EmitOrder)
                        property.Order = order++;
                    property.IsDeprecated = itemDocs.Any(d => d.Text.StartsWith("DEPRECATED"));

                    properties.Add(property);
                }
            }

            return properties;
        }

        private NamespaceModel CreateNamespaceModel(Uri source, XmlQualifiedName qualifiedName)
        {
            NamespaceModel namespaceModel = null;
            if (!qualifiedName.IsEmpty && qualifiedName.Namespace != XmlSchema.Namespace)
            {
                var key = new NamespaceKey(source, qualifiedName.Namespace);
                if (!Namespaces.TryGetValue(key, out namespaceModel))
                {
                    var namespaceName = BuildNamespace(source, qualifiedName.Namespace);
                    namespaceModel = new NamespaceModel(key, _configuration) { Name = namespaceName };
                    Namespaces.Add(key, namespaceModel);
                }
            }
            return namespaceModel;
        }

        private bool NameExists(XmlQualifiedName name)
        {
            var elements = Set.GlobalElements.Names.Cast<XmlQualifiedName>();
            var types = Set.GlobalTypes.Names.Cast<XmlQualifiedName>();
            return elements.Concat(types).Any(n => n.Namespace == name.Namespace && name.Name.Equals(n.Name, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<RestrictionModel> GetRestrictions(IEnumerable<XmlSchemaFacet> facets, XmlSchemaSimpleType type)
        {
            var min = facets.OfType<XmlSchemaMinLengthFacet>().Select(f => int.Parse(f.Value)).DefaultIfEmpty().Max();
            var max = facets.OfType<XmlSchemaMaxLengthFacet>().Select(f => int.Parse(f.Value)).DefaultIfEmpty().Min();

            if (DataAnnotationMode == XmlSchemaClassGenerator.DataAnnotationMode.All)
            {
                if (min > 0) yield return new MinLengthRestrictionModel(_configuration) { Value = min };
                if (max > 0) yield return new MaxLengthRestrictionModel(_configuration) { Value = max };
            }
            else if (min > 0 || max > 0)
                yield return new MinMaxLengthRestrictionModel(_configuration) { Min = min, Max = max };

            foreach (var facet in facets)
            {
                if (facet is XmlSchemaTotalDigitsFacet)
                    yield return new TotalDigitsRestrictionModel(_configuration) { Value = int.Parse(facet.Value) };
                if (facet is XmlSchemaFractionDigitsFacet)
                    yield return new FractionDigitsRestrictionModel(_configuration) { Value = int.Parse(facet.Value) };

                if (facet is XmlSchemaPatternFacet)
                    yield return new PatternRestrictionModel(_configuration) { Value = facet.Value };

                var valueType = type.Datatype.ValueType;

                if (facet is XmlSchemaMinInclusiveFacet)
                    yield return new MinInclusiveRestrictionModel(_configuration) { Value = facet.Value, Type = valueType };
                if (facet is XmlSchemaMinExclusiveFacet)
                    yield return new MinExclusiveRestrictionModel(_configuration) { Value = facet.Value, Type = valueType };
                if (facet is XmlSchemaMaxInclusiveFacet)
                    yield return new MaxInclusiveRestrictionModel(_configuration) { Value = facet.Value, Type = valueType };
                if (facet is XmlSchemaMaxExclusiveFacet)
                    yield return new MaxExclusiveRestrictionModel(_configuration) { Value = facet.Value, Type = valueType };
            }
        }

        public IEnumerable<Particle> GetElements(XmlSchemaGroupBase groupBase)
        {
            foreach (var item in groupBase.Items)
            {
                foreach (var element in GetElements(item))
                {
                    element.MaxOccurs = Math.Max(element.MaxOccurs, groupBase.MaxOccurs);
                    element.MinOccurs = Math.Min(element.MinOccurs, groupBase.MinOccurs);
                    yield return element;
                }
            }
        }

        public IEnumerable<Particle> GetElements(XmlSchemaObject item)
        {
            if (item == null) yield break;

            var element = item as XmlSchemaElement;
            if (element != null) yield return new Particle(element);

            var any = item as XmlSchemaAny;
            if (any != null) yield return new Particle(any);

            var groupRef = item as XmlSchemaGroupRef;
            if (groupRef != null) yield return new Particle(groupRef);

            var itemGroupBase = item as XmlSchemaGroupBase;
            if (itemGroupBase != null)
                foreach (var groupBaseElement in GetElements(itemGroupBase))
                    yield return groupBaseElement;
        }

        public List<DocumentationModel> GetDocumentation(XmlSchemaAnnotated annotated)
        {
            if (annotated.Annotation == null) return new List<DocumentationModel>();

            return annotated.Annotation.Items.OfType<XmlSchemaDocumentation>()
                .Where(d => d.Markup != null && d.Markup.Any())
                .Select(d => new DocumentationModel { Language = d.Language, Text = new XText(d.Markup.First().InnerText).ToString() })
                .Where(d => !string.IsNullOrEmpty(d.Text))
                .ToList();
        }
    }
}


using System.Diagnostics;
using System.Xml.Serialization;
using CAFE.DAL.Models.Resources;

namespace CAFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CAFE.DAL.Models;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Reflection;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CAFE.DAL.DbContexts.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CAFE.DAL.DbContexts.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            try
            {
                var permanentUserName = WebConfigurationManager.AppSettings["PermanentUserName"];
            var users = new List<DbUser>
            {
                 new DbUser() {
                    Id = Guid.Parse("2218929f-988c-e611-9c39-f0761cf9a82c"),
                    Email = "admin@cafe.com",
                    Name = "Administrator",
                    Surname = "First",
                    UserName = "adminFirst",
                    PostalAddress = "38 avenue de l'Opera, F-75002 Paris, France",
                    PhoneNumber = "+380951234567",
                    PasswordHash = "AL39j/DOuiN+wzEQya/wDhPIkg9ljb3wak1Zco3YaQMsEcRYvwxjN5GWsgiLnfMtvg==", //111111
                    SecurityStamp = "6ddca9e3-73d9-43b1-be6c-ed6c2fe6d4a2",
                    AcceptanceDate = new System.DateTime(2010, 10, 10, 10, 10, 10),
                    AccessFailedCount = 0,
                    LockoutEndDateUtc = null,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    IsActive = true,
                    IsAccepted = true,
                    EmailConfirmed = true
                },
                new DbUser()
                {
                    Id = Guid.Parse("B4ADCD73-F17F-4F8D-833A-CD546702654A"),
                    Email = "user@cafe.com",
                    Name = "User",
                    Surname = "First",
                    UserName = "userFirst",
                    PostalAddress = "Unit 42, Land of Bargains Shopping Paradise, 12 Highway 101, Boston, MA, USA",
                    PhoneNumber = "+130591234567",
                    PasswordHash = "AL39j/DOuiN+wzEQya/wDhPIkg9ljb3wak1Zco3YaQMsEcRYvwxjN5GWsgiLnfMtvg==", //111111
                    SecurityStamp = "6ddca9e3-73d9-43b1-be6c-ed6c2fe6d4a2",
                    AcceptanceDate = new System.DateTime(2011, 1, 1, 1, 1, 1),
                    AccessFailedCount = 0,
                    LockoutEndDateUtc = null,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    IsActive = true,
                    IsAccepted = true,
                    EmailConfirmed = true,
                    Roles = new List<DbRole> { }
                },
                new DbUser()
                {
                    Id = Guid.Parse("B10C1E4A-61A1-4B0D-A2C5-F3A2AF1483BF"),
                    Email = permanentUserName + "@cafe.com",
                    Name = permanentUserName,
                    Surname = "User",
                    UserName = permanentUserName,
                    PostalAddress = "When User decides to delete an account but keep the data, this data will be assigned to this account",
                    PhoneNumber = "",
                    PasswordHash = "AL39j/DOuiN+wzEQya/wDhPIkg9ljb3wak1Zco3YaQMsEcRYvwxjN5GWsgiLnfMtvg==", //111111
                    SecurityStamp = "6ddca9e3-73d9-43b1-be6c-ed6c2fe6d4a2",
                    AcceptanceDate = new System.DateTime(2016, 1, 1, 1, 1, 1),
                    AccessFailedCount = 0,
                    LockoutEndDateUtc = null,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    IsActive = true,
                    IsAccepted = true,
                    EmailConfirmed = true,
                    Roles = new List<DbRole> { }
                }
            };

                var roles = new[]
                {
                new DbRole()
                {
                    Id = Guid.Parse("2318929f-988c-e611-9c39-f0761cf9a82c"),
                    Name = "Administrator",
                    Discriminator = "Administrator's role",
                    IsGroup = false,
                    Users = new [] { users[0] }
                },
                new DbRole()
                {
                    Id = Guid.Parse("58307169-A9A7-4D48-B2C2-E86A466B911D"),
                    Name = "Curator",
                    Discriminator = "Curator's role",
                    IsGroup = false
                },
                new DbRole()
                {
                    Id = Guid.Parse("81C6630F-ADE9-45F5-A3CE-029D0FEF9FC8"),
                    Name = "User",
                    Discriminator = "User's role",
                    IsGroup = false,
                    Users = new [] { users[1], users[2] }
                }
            };

                foreach (var role in roles)
                    context.Roles.AddOrUpdate(x => x.Id, role);

                foreach (var user in users)
                    context.Users.AddOrUpdate(x => x.Id, user);

                /*
                var userFiles = new[]
                {
                    new DbUserFile
                    {
                       Id = Guid.Parse("af3b0a4f-fd2a-45dc-b4c2-444289636332"),
                       Name = "test-file.txt",
                       CreationDate = System.DateTime.Parse("2016-10-27 17:39:25.900"),
                       AcceptedGroups = new List<DbRole> { },
                       AcceptedUsers = new List<DbUser> { users[1] },
                       AccessMode = DbUserFile.DbFileAccessMode.Explicit,
                       Type = DbUserFile.DbFileType.Other,
                       Description = "This is text file",
                       Owner = users[0]
                    },
                    new DbUserFile
                    {
                       Id = Guid.Parse("fa3f9e9f-ced2-4653-8de1-a960945e4a79"),
                       Name = "test-file.mp3",
                       CreationDate = System.DateTime.Parse("2016-10-27 18:12:27.153"),
                       AcceptedGroups = new List<DbRole> { },
                       AcceptedUsers = new List<DbUser> { },
                       AccessMode = DbUserFile.DbFileAccessMode.Explicit,
                       Type = DbUserFile.DbFileType.Audio,
                       Description = "This is audio file",
                       Owner = users[0]
                    }
                };

                foreach (var userFile in userFiles)
                    context.UserFiles.AddOrUpdate(x => x.Id, userFiles);

                var accessibleResources = new[]
                {
                    new DbAccessibleResource
                    {
                       Id = 1,
                       Kind = 0,
                       Owner = users[0],
                       ResourceId = Guid.Parse("af3b0a4f-fd2a-45dc-b4c2-444289636332")
                    },
                    new DbAccessibleResource
                    {
                       Id = 2,
                       Kind = 0,
                       Owner = users[0],
                       ResourceId = Guid.Parse("fa3f9e9f-ced2-4653-8de1-a960945e4a79")
                    }
                };

                var accessRequests = new[]
                {
                    new DbAccessRequest
                    {
                        Id = 1,
                        CreationDate = System.DateTime.Parse("2016-10-31 23:15:26.810"),
                        RequestSubject = "Access request 1",
                        RequestMessage = "Gime me access, please",
                        RequestedResources = new List<DbAccessibleResource> { accessibleResources[0] }
                    },
                    new DbAccessRequest
                    {
                        Id = 2,
                        CreationDate = System.DateTime.Parse("2016-10-31 23:16:07.120"),
                        RequestSubject = "Access request 2",
                        RequestMessage = "I need to access to this mp3 file!",
                        RequestedResources = new List<DbAccessibleResource> { accessibleResources[1] },
                    }
                };


                var conversations = new[]
                {
                    new DbConversation
                    {
                        Id = 1,
                        HasRecieverUnreadMessages = false,
                        Receiver = users[0],
                        Request = accessRequests[0],
                        Requester = users[1],
                        Status = DbAccessRequestStatus.Open
                    },
                    new DbConversation
                    {
                        Id = 2,
                        HasRecieverUnreadMessages = true,
                        Receiver = users[0],
                        Request = accessRequests[1],
                        Requester = users[1],
                        Status = DbAccessRequestStatus.Accepted
                    }
                };

                var messages = new[]
                {
                    new DbMessage
                    {
                        Id = 1,
                        Conversation = conversations[0],
                        Text = "Gime me access, please.",
                        CreationDate = System.DateTime.Parse("2016-10-31 23:15:27.137"),
                        Receiver = users[0],
                        Sender = users[1]
                    },
                    new DbMessage
                    {
                        Id = 2,
                        Conversation = conversations[1],
                        Text = "I need to access to this mp3 file!",
                        CreationDate = System.DateTime.Parse("2016-10-31 23:16:07.137"),
                        Receiver = users[0],
                        Sender = users[1]
                    },
                    new DbMessage
                    {
                        Id = 3,
                        Conversation = conversations[0],
                        Text = "Ok, I will give you access. Enjoy it!!",
                        CreationDate = System.DateTime.Parse("2016-10-31 23:24:52.193"),
                        Receiver = users[1],
                        Sender = users[0]
                    }
                };


                foreach (var role in roles)
                    context.Roles.AddOrUpdate(x => x.Id, role);

                foreach (var user in users)
                    context.Users.AddOrUpdate(x => x.Id, user);

                foreach (var accessibleResource in accessibleResources)
                    context.AccessibleResources.AddOrUpdate(x => x.Id, accessibleResource);

                foreach (var userFile in userFiles)
                    context.UserFiles.AddOrUpdate(x => x.Id, userFiles);

                foreach (var accessRequest in accessRequests)
                    context.AccessRequests.AddOrUpdate(x => x.Id, accessRequest);

                foreach (var conversation in conversations)
                    context.Conversations.AddOrUpdate(x => x.Id, conversation);

                foreach (var message in messages)
                    context.Messages.AddOrUpdate(x => x.Id, message);
                */

                ////////Seed for vocabularies

                var directory = AppDomain.CurrentDomain.BaseDirectory;
                //For Denis => 
                //string schemaPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + "\\Tools\\ModelGenerator\\Compiled\\ease.xsd";
                var baseBath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName;
                string schemaPath = baseBath + "\\ease.xsd";
                XmlTextReader reader = new XmlTextReader(schemaPath);
                XmlSchema myschema = XmlSchema.Read(reader, ValidationCallback);
                var vocabularyValues = new List<DbVocabularyValue>();

                //First check that vocabulary values doesn't exists
                if (!context.VocabularyValues.Any())
                {
                    //Getting asm when stores vocabulary enums
                    var asm =
                        AppDomain.CurrentDomain.GetAssemblies()
                            .AsEnumerable()
                            .Where(w => w.FullName.Contains("CAFE.Core"))
                            .FirstOrDefault();

                    //Get enum types that is Enum, public and it's name ends with 'Vocabulary'
                    var enumTypes = asm.GetTypes()
                            .Where(t => t.IsEnum && t.IsPublic && t.Name.ToLower().EndsWith("vocabulary"));

                    var enumDics = new Dictionary<string, List<string>>();

                    //Enumerate through each enum types and getting name or alias(XmlEnumAttribute)
                    foreach (var enumType in enumTypes)
                    {
                        var enumNames = new List<string>();
                        var enemValues = Enum.GetNames(enumType).AsEnumerable();
                        foreach (var enemValue in enemValues)
                        {
                            var findedAlias = enemValue;
                            var memInfo = enumType.GetMember(enemValue);
                            var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute),
                                false);
                            if (attributes.Length > 0)
                            {
                                findedAlias = ((XmlEnumAttribute)attributes[0]).Name;
                            }

                            enumNames.Add(findedAlias);
                        }

                        enumDics.Add(enumType.Name, enumNames);
                    }

                    //Store finded vocabulary values into db with enum type
                    foreach (var enumDic in enumDics)
                    {
                        //Trace.WriteLine(enumDic.Key);

                        var simpleTypeItems = new XmlSchemaObjectCollection();
                        foreach (object item in myschema.Items)
                        {
                            if (item is XmlSchemaSimpleType)
                            {
                                var simpleType = item as XmlSchemaSimpleType;

                                //Trace.WriteLine(simpleType.Name);

                                if (simpleType.Name == enumDic.Key)
                                {
                                    var restrictionType = simpleType.Content as XmlSchemaSimpleTypeRestriction;
                                    if(restrictionType != null)
                                    {
                                        simpleTypeItems = restrictionType.Facets;
                                    }
                                    var compositeType = simpleType.Content as XmlSchemaSimpleTypeUnion;
                                    if(compositeType != null)
                                    {
                                        if (compositeType.MemberTypes.Any())
                                        {
                                            var firstFacetsType = compositeType.MemberTypes.First();
                                            if(firstFacetsType != null)
                                            {
                                                var foundType = FindTypeByName(firstFacetsType.Name, myschema.Items);
                                                if(foundType != null)
                                                {
                                                    var firstRestrictionType = foundType.Content as XmlSchemaSimpleTypeRestriction;
                                                    if(firstRestrictionType != null)
                                                    {
                                                        simpleTypeItems = firstRestrictionType.Facets;
                                                    }
                                                }
                                                
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        foreach (var enVal in enumDic.Value)
                        {
                            //Trace.WriteLine(enVal);

                            var documentation = new XmlSchemaDocumentation();
                            foreach (var item in simpleTypeItems)
                            {
                                if (!(item is XmlSchemaEnumerationFacet))
                                    continue;

                                var name = (item as XmlSchemaEnumerationFacet).Value;

                                if (name != enVal)
                                    continue;

                                var items = (item as XmlSchemaEnumerationFacet).Annotation?.Items;
                                if (null == items)
                                    continue;

                                var itemfound = false;
                                foreach (var facetItem in items)
                                {
                                    if (facetItem is XmlSchemaDocumentation)
                                    {
                                        documentation = facetItem as XmlSchemaDocumentation;
                                        itemfound = true;
                                        break;
                                    }
                                }
                                if (itemfound)
                                    break;
                            }

                            vocabularyValues.Add(new DbVocabularyValue()
                            {
                                Type = enumDic.Key,
                                Value = enVal,
                                Description = documentation?.Markup?[0].InnerText
                            });
                        }
                    }
                }

                if (!context.SchemaItemDescriptions.Any())
                {
                    var items = LoadXsdDescriptions(schemaPath);
                    WriteSchemaDataToDatabase(items, context);
                }

                //Adding descriptions for labels
                System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(schemaPath);
                var elements2 = doc.Descendants().Where(d => d.Name.LocalName == "documentation");

                foreach (var item in elements2)
                {
                    var name = item.Parent.Parent.FirstAttribute.Value;
                    if (null == vocabularyValues.FirstOrDefault(v => v.Type == name))
                        vocabularyValues.Add(new DbVocabularyValue()
                        {
                            Type = name,
                            Value = "-1",
                            Description = item.Value
                        });
                }

                context.VocabularyValues.AddRange(vocabularyValues);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public XmlSchemaSimpleType FindTypeByName(string name, XmlSchemaObjectCollection items)
        {
            foreach (object item in items)
            {
                if (item is XmlSchemaSimpleType)
                {
                    var simpleType = item as XmlSchemaSimpleType;

                    //Trace.WriteLine(simpleType.Name);

                    if (simpleType.Name == name)
                    {
                        return simpleType;
                    }
                }
            }

            return null;
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }


        static List<SchemeDescriptionElement> LoadXsdDescriptions(string xsdPath)
        {
            var result = new List<SchemeDescriptionElement>();

            try
            {
                var xDoc = XDocument.Load(xsdPath);
                var ns = XNamespace.Get(@"http://www.w3.org/2001/XMLSchema");

                var easeElement = xDoc.Element(ns + "schema").Element(ns + "element");
                var objectElement =
                    easeElement.Element(ns + "complexType").Element(ns + "sequence").Element(ns + "element");
                var contextsElement =
                    objectElement.Element(ns + "complexType").Element(ns + "sequence").Element(ns + "element");
                var contextElement =
                    contextsElement.Element(ns + "complexType").Element(ns + "sequence").Element(ns + "element");

                var contextSequense = contextElement.Element(ns + "complexType").Element(ns + "sequence");

                var parentKey = "Object";

                result.AddRange(GetElementsRecursively(contextSequense, parentKey));

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            return result;
        }

        static IEnumerable<SchemeDescriptionElement> GetElementsRecursively(XElement element, string parentKey)
        {
            var ns = XNamespace.Get(@"http://www.w3.org/2001/XMLSchema");
            List<SchemeDescriptionElement> result = new List<SchemeDescriptionElement>();

            if (element.HasElements)
            {
                foreach (var child in element.Elements())
                {
                    try
                    {
                        if (child.Attribute("name") == null) continue;

                        var key = $"{parentKey}.{child.Attribute("name").Value}";
                        var annotationElement = child.Element(ns + "annotation");
                        if (annotationElement == null) continue;

                        var descriptionElement = annotationElement.Element(ns + "documentation");
                        var description = "";
                        if (descriptionElement != null)
                            description = descriptionElement.Value.Trim().Replace("\t", "");

                        var resultItem = new SchemeDescriptionElement();
                        resultItem.Key = key;
                        resultItem.Description = description;

                        var parenComplexType = child.Element(ns + "complexType");
                        if (parenComplexType != null)
                        {
                            var parentSequences = parenComplexType.Element(ns + "sequence");
                            if (parentSequences != null)
                                resultItem.Children.AddRange(GetElementsRecursively(parentSequences, key));

                        }

                        result.Add(resultItem);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }

            return result;
        }

        private void WriteSchemaDataToDatabase(IEnumerable<SchemeDescriptionElement> items, DbContexts.ApplicationDbContext context)
        {
            foreach (var schemeDescriptionElement in items)
            {
                var dbItem = new DbSchemaItemDescription();
                dbItem.Path = schemeDescriptionElement.Key;
                dbItem.Description = schemeDescriptionElement.Description;

                context.SchemaItemDescriptions.Add(dbItem);

                WriteSchemaDataToDatabase(schemeDescriptionElement.Children, context);
            }    
        }


        class SchemeDescriptionElement
        {
            public string Key { get; set; }
            public string Description { get; set; }

            public List<SchemeDescriptionElement> Children { get; set; } = new List<SchemeDescriptionElement>();
        }
    }
}

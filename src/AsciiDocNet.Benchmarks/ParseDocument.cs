﻿using BenchmarkDotNet.Attributes;

namespace AsciiDocNet.Benchmarks
{
	[MarkdownExporterAttribute.GitHub]
	[JsonExporterAttribute.Full]
	[MemoryDiagnoser]
	[RyuJitX64Job]
	public class ParseDocument
	{
		private string _text = @":ref_current: https://www.elastic.co/guide/en/elasticsearch/reference/6.1

:github: https://github.com/elastic/elasticsearch-net

:nuget: https://www.nuget.org/packages

////
IMPORTANT NOTE
==============
This file has been generated from https://github.com/elastic/elasticsearch-net/tree/6.x/src/Tests/ClientConcepts/HighLevel/Mapping/FluentMapping.doc.cs. 
If you wish to submit a PR for any spelling mistakes, typos or grammatical errors for this file,
please modify the original csharp file found at the link and submit the PR with that change. Thanks!
////

[[fluent-mapping]]
=== Fluent mapping

Fluent mapping POCO properties to fields within an Elasticsearch type mapping
offers the most control over the process. With fluent mapping, each property of
the POCO is explicitly mapped to an Elasticsearch type field mapping.

To demonstrate, we'll define two POCOs

* `Company`, which has a name and a collection of Employees

* `Employee` which has various properties of different types and has itself a collection of `Employee` types.

[source,csharp]
----
public class Company
{
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }
}

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Salary { get; set; }
    public DateTime Birthday { get; set; }
    public bool IsManager { get; set; }
    public List<Employee> Employees { get; set; }
    public TimeSpan Hours { get; set; }
}
----

==== Manual mapping

To create a mapping for our Company type, we can use the fluent API
and map each property explicitly

[source,csharp]
----
var createIndexResponse = client.CreateIndex(""myindex"", c => c
    .Mappings(ms => ms
        .Map<Company>(m => m
            .Properties(ps => ps
                .Text(s => s
                    .Name(n => n.Name)
                )
                .Object<Employee>(o => o
                    .Name(n => n.Employees)
                    .Properties(eps => eps
                        .Text(s => s
                            .Name(e => e.FirstName)
                        )
                        .Text(s => s
                            .Name(e => e.LastName)
                        )
                        .Number(n => n
                            .Name(e => e.Salary)
                            .Type(NumberType.Integer)
                        )
                    )
                )
            )
        )
    )
);
----

Here, the Name property of the `Company` type has been mapped as a {ref_current}/text.html[text datatype] and
the `Employees` property mapped as an {ref_current}/object.html[object datatype]. Within this object mapping,
only the `FirstName`, `LastName` and `Salary` properties of the `Employee` type have been mapped.

The json mapping for this example looks like

[source,javascript]
----
{
  ""mappings"": {
    ""company"": {
      ""properties"": {
        ""name"": {
          ""type"": ""text""
        },
        ""employees"": {
          ""type"": ""object"",
          ""properties"": {
            ""firstName"": {
              ""type"": ""text""
            },
            ""lastName"": {
              ""type"": ""text""
            },
            ""salary"": {
              ""type"": ""integer""
            }
          }
        }
      }
    }
  }
}
----

Manual mapping in this way is powerful but can become verbose and unwieldy for
large POCOs. The majority of the time you simply want to map *all* the properties of a POCO in a single go
without having to specify the mapping for each property,
particularly when there is <<auto-map,inferred mapping>> from CLR types to Elasticsearch types.

This is where the fluent mapping in conjunction with auto mapping comes in.

[[auto-map-with-overrides]]
==== Auto mapping with fluent overrides

In most cases, you'll want to map more than just the vanilla datatypes and also provide
various options for your properties, such as the analyzer to use, whether to enable `doc_values`, etc.

In this case, it's possible to use `.AutoMap()` in conjunction with explicitly mapped properties.

Here we are using `.AutoMap()` to automatically infer the mapping of our `Company` type from the
CLR property types, but then we're overriding the `Employees` property to make it a
{ref_current}/nested.html[nested datatype], since by default `.AutoMap()` will infer the
`List<Employee>` property as an `object` datatype

[source,csharp]
----
var createIndexResponse = client.CreateIndex(""myindex"", c => c
    .Mappings(ms => ms
        .Map<Company>(m => m
            .AutoMap()
            .Properties(ps => ps
                .Nested<Employee>(n => n
                    .Name(nn => nn.Employees)
                )
            )
        )
    )
);
----

[source,javascript]
----
{
  ""mappings"": {
    ""company"": {
      ""properties"": {
        ""name"": {
          ""type"": ""text"",
          ""fields"": {
            ""keyword"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
            }
          }
        },
        ""employees"": {
          ""type"": ""nested""
        }
      }
    }
  }
}
----

`.AutoMap()` __**is idempotent**__   therefore calling it _before_ or _after_
manually mapped properties will still yield the same result. The next example
generates the same mapping as the previous

[source,csharp]
----
createIndexResponse = client.CreateIndex(""myindex"", c => c
    .Mappings(ms => ms
        .Map<Company>(m => m
            .Properties(ps => ps
                .Nested<Employee>(n => n
                    .Name(nn => nn.Employees)
                )
            )
            .AutoMap()
        )
    )
);
----

Just as we were able to override the inferred properties from auto mapping in the previous example,
fluent mapping also takes precedence over <<attribute-mapping, Attribute Mapping>>.
In this way, fluent, attribute and auto mapping can be combined. We'll demonstrate with an example.

Consider the following two POCOS

[source,csharp]
----
[ElasticsearchType(Name = ""company"")]
public class CompanyWithAttributes
{
    [Keyword(NullValue = ""null"", Similarity = ""BM25"")]
    public string Name { get; set; }

    [Text(Name = ""office_hours"")]
    public TimeSpan? HeadOfficeHours { get; set; }

    [Object(Store = false)]
    public List<Employee> Employees { get; set; }
}

[ElasticsearchType(Name = ""employee"")]
public class EmployeeWithAttributes
{
    [Text(Name = ""first_name"")]
    public string FirstName { get; set; }

    [Text(Name = ""last_name"")]
    public string LastName { get; set; }

    [Number(DocValues = false, IgnoreMalformed = true, Coerce = true)]
    public int Salary { get; set; }

    [Date(Format = ""MMddyyyy"")]
    public DateTime Birthday { get; set; }

    [Boolean(NullValue = false, Store = true)]
    public bool IsManager { get; set; }

    [Nested]
    [PropertyName(""empl"")]
    public List<Employee> Employees { get; set; }
}
----

Now when mapping, `AutoMap()` is called to infer the mapping from the POCO property types and
attributes, and inferred mappings are overridden with fluent mapping

[source,csharp]
----
var createIndexResponse = client.CreateIndex(""myindex"", c => c
    .Mappings(ms => ms
        .Map<CompanyWithAttributes>(m => m
            .AutoMap() <1>
            .Properties(ps => ps <2>
                .Nested<Employee>(n => n
                    .Name(nn => nn.Employees)
                )
            )
        )
        .Map<EmployeeWithAttributes>(m => m
            .AutoMap() <3>
            .Properties(ps => ps <4>
                .Text(s => s
                    .Name(e => e.FirstName)
                    .Fields(fs => fs
                        .Keyword(ss => ss
                            .Name(""firstNameRaw"")
                        )
                        .TokenCount(t => t
                            .Name(""length"")
                            .Analyzer(""standard"")
                        )
                    )
                )
                .Number(n => n
                    .Name(e => e.Salary)
                    .Type(NumberType.Double)
                    .IgnoreMalformed(false)
                )
                .Date(d => d
                    .Name(e => e.Birthday)
                    .Format(""MM-dd-yy"")
                )
            )
        )
    )
            );
----
<1> Automap company
<2> Override company inferred mappings
<3> Auto map employee
<4> Override employee inferred mappings

[source,javascript]
----
{
  ""mappings"": {
    ""company"": {
      ""properties"": {
        ""employees"": {
          ""type"": ""nested""
        },
        ""name"": {
          ""null_value"": ""null"",
          ""similarity"": ""BM25"",
          ""type"": ""keyword""
        },
        ""office_hours"": {
          ""type"": ""text""
        }
      }
    },
    ""employee"": {
      ""properties"": {
        ""birthday"": {
          ""format"": ""MM-dd-yy"",
          ""type"": ""date""
        },
        ""empl"": {
          ""properties"": {
            ""birthday"": {
              ""type"": ""date""
            },
            ""employees"": {
              ""properties"": {},
              ""type"": ""object""
            },
            ""firstName"": {
              ""fields"": {
                ""keyword"": {
                  ""type"": ""keyword"",
                  ""ignore_above"": 256
                }
              },
              ""type"": ""text""
            },
            ""hours"": {
              ""type"": ""long""
            },
            ""isManager"": {
              ""type"": ""boolean""
            },
            ""lastName"": {
              ""fields"": {
                ""keyword"": {
                  ""type"": ""keyword"",
                  ""ignore_above"": 256
                }
              },
              ""type"": ""text""
            },
            ""salary"": {
              ""type"": ""integer""
            }
          },
          ""type"": ""nested""
        },
        ""first_name"": {
          ""fields"": {
            ""firstNameRaw"": {
              ""type"": ""keyword""
            },
            ""length"": {
              ""analyzer"": ""standard"",
              ""type"": ""token_count""
            }
          },
          ""type"": ""text""
        },
        ""isManager"": {
          ""null_value"": false,
          ""store"": true,
          ""type"": ""boolean""
        },
        ""last_name"": {
          ""type"": ""text""
        },
        ""salary"": {
          ""ignore_malformed"": false,
          ""type"": ""double""
        }
      }
    }
  }
}
----

==== Auto mapping overrides down the object graph

You may have noticed in the <<auto-map-with-overrides, Automap with fluent overrides example>>
that the properties of the `Employees` property on `Company` were not mapped. This is because the automapping
was applied only at the root level of the `Company` mapping.

By calling `.AutoMap()` inside of the `.Nested<Employee>` mapping, it is possible to auto map the
`Employee` nested properties and again, override any inferred mapping from the automapping process,
through manual mapping

[source,csharp]
----
var createIndexResponse = client.CreateIndex(""myindex"", c => c
    .Mappings(m => m
        .Map<Company>(mm => mm
            .AutoMap() <1>
            .Properties(p => p <2>
                .Nested<Employee>(n => n
                    .Name(nn => nn.Employees)
                    .AutoMap() <3>
                    .Properties(pp => pp <4>
                        .Text(t => t
                            .Name(e => e.FirstName)
                        )
                        .Text(t => t
                            .Name(e => e.LastName)
                        )
                        .Nested<Employee>(nn => nn
                            .Name(e => e.Employees)
                        )
                    )
                )
            )
        )
    )
);
----
<1> Automap `Company`
<2> Override specific `Company` mappings
<3> Automap `Employees` property
<4> Override specific `Employee` properties

[source,javascript]
----
{
  ""mappings"": {
    ""company"": {
      ""properties"": {
        ""name"": {
          ""type"": ""text"",
          ""fields"": {
            ""keyword"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
            }
          }
        },
        ""employees"": {
          ""type"": ""nested"",
          ""properties"": {
            ""firstName"": {
              ""type"": ""text""
            },
            ""lastName"": {
              ""type"": ""text""
            },
            ""salary"": {
              ""type"": ""integer""
            },
            ""birthday"": {
              ""type"": ""date""
            },
            ""isManager"": {
              ""type"": ""boolean""
            },
            ""employees"": {
              ""type"": ""nested""
            },
            ""hours"": {
              ""type"": ""long""
            }
          }
        }
      }
    }
  }
}
----

";

		[Benchmark]
		public Document Parse() => Document.Parse(_text);
	}
}
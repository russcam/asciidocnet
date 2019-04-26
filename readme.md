# AsciiDocNet

This is a _**very alpha**_ implementation of an AsciiDoc processor for .NET. It's been written from scratch using .NET idioms, although it's been influenced by the [AsciiDoctor text processor](http://asciidoctor.org/) in [Ruby](https://github.com/asciidoctor/asciidoctor). 

## What is AsciiDoc?

From [AsciiDoctor](http://asciidoctor.org/docs/what-is-asciidoc/#what-is-asciidoc)

> AsciiDoc belongs to the family of lightweight markup languages, the most renowned of which is Markdown. AsciiDoc stands out from this group because it supports all the structural elements necessary for drafting articles, technical manuals, books, presentations and prose. In fact, it’s capable of meeting even the most advanced publishing requirements and technical semantics.

More information can be found on the [AsciiDoctor website](http://asciidoctor.org/) as well as the original [AsciiDoc guide](http://asciidoc.org/).

## Why write a processor for .NET?

Writing the processor came out of a need to modify existing AsciiDoc files to make modifications to them such as add Section titles, rearrange elements, etc.. and then save them again as AsciiDoc files for processing by an existing AsciiDoc toolchain. 

Since this modification happens within a .NET application, the intention was to be able to parse an Asciidoc file into an element tree representation that could be easily modified by visiting elements within the tree using the visitor pattern and then allow it to be output to different representations including AsciiDoc, HTML, docbook, PDF, etc.

### Is it production ready?

**_In short, no!_** There are many _TODO_s in the source and many scenarios that are not yet handled, for example, `Table` elements are not handled at all and neither are list item continuations. Additionally, since the original goal was to be able to output an AsciiDoc file from and AsciiDoc input, generating HTML or docbook outputs are either incomplete or not yet started. Over time, this functionality will be added but if you are itching for a feature now, [**We accept Pull Requests :)**](https://github.com/russcam/asciidocnet/pulls)

## License

AsciiDocNet is licensed under Apache 2. [See the license for more details](license.txt)

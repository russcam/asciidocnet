# language: en
Feature: Cross References
  In order to create links to other sections
  As a writer
  I want to be able to use a cross reference macro

  @ignore
  # TODO: Support tables and convert slim out to html
  Scenario: Create a cross reference from an AsciiDoc cell to a section
  Given the AsciiDoc source
    """
    |===
    a|See <<_install>>
    |===
    
    == Install
    
    Instructions go here.
    """
  When it is converted to html
  Then the result should match the HTML source
    """
    table.tableblock.frame-all.grid-all.spread
      colgroup
        col style='width: 100%;'
      tbody
        tr
          td.tableblock.halign-left.valign-top
            div
              .paragraph: p
                'See
                a href='#_install' Install
    .sect1
      h2#_install Install
      .sectionbody
        .paragraph: p Instructions go here.
    """
 

  @ignore
  # TODO: Convert slim output to HTML
  Scenario: Create a cross reference using the target section title
  Given the AsciiDoc source
    """
    == Section One
 
    content
 
    == Section Two
 
    refer to <<Section One>>
    """
  When it is converted to html
  Then the result should match the HTML source
    """
    .sect1
      h2#_section_one Section One
      .sectionbody: .paragraph: p content
    .sect1
      h2#_section_two Section Two
      .sectionbody: .paragraph: p
        'refer to
        a href='#_section_one' Section One
    """
 

  @ignore
  # TODO: Support reftext attribute and convert slim output to html
  Scenario: Create a cross reference using the target reftext
  Given the AsciiDoc source
    """
    [reftext="the first section"]
    == Section One
 
    content
 
    == Section Two
 
    refer to <<the first section>>
    """
  When it is converted to html
  Then the result should match the HTML source
    """
    .sect1
      h2#_section_one Section One
      .sectionbody: .paragraph: p content
    .sect1
      h2#_section_two Section Two
      .sectionbody: .paragraph: p
        'refer to
        a href='#_section_one' the first section
    """

  @ignore
  # TODO: wrap contents proceeding a section title in a section body
  Scenario: Create a cross reference using the formatted target title
  Given the AsciiDoc source
    """
    == Section *One*

    content

    == Section Two

    refer to <<Section *One*>>
    """
  When it is converted to html
  Then the result should match the HTML source
     """
     <div class="sect1">
       <h2 id="_section_strong_one_strong">
         Section <strong>One</strong>
	   </h2>
	 <div class="sectionbody">
	 <div class="paragraph">
	 <p>content</p>
	 </div>
	 </div>
     </div>
        <div class="sect1">
          <h2 id="_section_two">Section Two</h2>
	   <div class="sectionbody">
	   <div class="paragraph">
	   <p>refer to
            <a href="#_section_strong_one_strong">
              Section <strong>One</strong>
	     </a>
	   </p>
       </div>
	   </div>
     </div>
     """

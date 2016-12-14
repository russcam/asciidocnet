# language: en
Feature: Open Blocks
  In order to group content in a generic container
  As a writer
  I want to be able to wrap content in an open block

  Scenario: Render an open block that contains a paragraph to HTML
  Given the AsciiDoc source
    """
    --
    A paragraph in an open block.
    --
    """
  When it is converted to html
  Then the result should match the HTML source
    """
    <div class="openblock">
    <div class="content">
    <div class="paragraph">
    <p>A paragraph in an open block.</p>
    </div>
    </div>
    </div>
    """

  # TODO: support converting to docbook
  #Scenario: Render an open block that contains a paragraph to DocBook
  #Given the AsciiDoc source
  #  """
  #  --
  #  A paragraph in an open block.
  #  --
  #  """
  #When it is converted to docbook
  #Then the result should match the XML source
  #  """
  #  <simpara>A paragraph in an open block.</simpara>
  #  """

  Scenario: Render an open block that contains a list to HTML
  Given the AsciiDoc source
    """
    --
    * one
    * two
    * three
    --
    """
  When it is converted to html
  Then the result should match the HTML source
    """
    <div class="openblock">
    <div class="content">
	<ul>
	  <li><p>one</p></li>
	  <li><p>two</p></li>
	  <li><p>three</p></li>
	</ul>
	</div>
	</div>
    """

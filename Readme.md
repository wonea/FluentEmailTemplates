#Fluent Email Templates

##Quick Examples

###No Merge Data
Send an email template with no merge data.

```c#

    var email = new Email()
        .From("donald@example.com, "Donald Duck")
        .To("goofy@example.com", "Goofy Dawg")
        .WithSubject("Loving FluentEmailTemplates")
        .WithHtmlBody("<html><body>OMG, FluentEmailTemplates is perfect!</body></html>")
        .Send();

```

###With Merge Data
Send an email template with merge data.

```c#

    var mergeData = new MergeData()
        .Add("FirstName", "Goofy")
        .Add("LastName", "Dawg")
        .Add("Address1", "1 Goof Street")
        .Add("Address2", "The Kennel")
        .Add("City", "Nuke Town")
        .Add("PostCode", "007")
        .Add("Country", "New Zealand");

    var email = new Email()
        .From("donald@example.com, "Donald Duck")
        .To("goofy@example.com", "Goofy Dawg")
        .WithEmailTemplateFromFile(@"C:\My_Template.xml")
        .WithMergeData(mergeData)
        .Send();

```

##Example Template With Merge Fields
Merge fields use the format *|Field|*

```xml

    <emailTemplate>
        <subject>
            <!-- The subject can contain merge fields. -->
            <value>*|FirstName|*, a new email for you!</value>
        </subject>
        <htmlBody>
            <!-- Include the standard email header html file. Relative path is appended to the app setting "FluentEmailTemplatesFilePath". -->
            <htmlFile relativePath="EmailContent\StandardEmailHeader.html" />
        
            <!-- Html template with merge fields. -->
            <html>
                <table style="width:600px;margin-top:20px;margin-bottom:20px;">
                    <tr>
                        <td style="padding:10px 10px 10px 10px;">
                            Hi *|FirstName|*,
                            
                            <br /><br />
                            This is example email template 001 with some basic merge fields. Nothing too fancy.
                                
                            <br /><br />
                            <p style="font-size:14px;font-weight:bold;">Your address details:</p>
                            <br />*|Address1|*
                            <br />*|Address2|*
                            <br />*|City|* *|PostCode|*
                            <br />*|Country|*
                            
                            <br /><br />
                            <p style="font-size:14px;font-weight:bold;">Thank you for your enquiry.</p>
                            
                            <br /><br />
                            Daffy Duck.
                            <br />Email: <a href="mailto:daffy.duck@example.com">daffy.duck@example.com</a>
                            <br /><a href="https://example.com">https://example.com</a>
                        </td>
                    </tr>
                </table>
            </html>
        
            <!-- Include the standard email footer html file. Relative path is appended to the app setting "FluentEmailTemplatesFilePath". -->
            <htmlFile relativePath="EmailContent\StandardEmailFooter.html" />
        </htmlBody>
    </emailTemplate>

```

##Missing Merge Fields And Merge Data
If a field is in the template, but missing from the merge data the field will
remain in the output.

If there's data in the merge data dictionary, but no matching field in the template
the merge field and value will not be included in the output.

##Missing Values
Manage your merge data as you add it to the dictionary. e.g. if you don't have a first name:

```c#

    var mergeData = new MergeData()
        .Add("FirstName", customer.FirstName ?? "Valued Customer");

```

##Formatting Values
Format your merge data as you add it to the dictionary.

```c#

    var mergeData = new MergeData()
        .Add("OrderTotal", string.Format("{0:#,0.00}", 345.67M));
        .Add("Today", string.Format("{0:dd}-{0:MM}-{0:yyyy}", DateTime.Now));

```


#Email Template Parts
An email template is a file or string using a pre defined xml format.
The html body is made up of **parts**. e.g.

```xml

    <emailTemplate>
        <subject>
            <value>*|FirstName|*, a new email for you</value>
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

##Parts

###html
Html is used verbatim with merge fields.

###htmlFile
A path to an HTML file. Use either "filePath" or "relativePath".

####filePath
The attribute "filePath" is the full file path or UNC to the html file.

####relativePath
The attribute "relativePath" is the relative path to an html file.
The relative path is appended to the app setting "FluentEmailTemplatesFilePath".

###image
Rendered as an img with all attributes.

###span
Rendered as a span with all attributes.

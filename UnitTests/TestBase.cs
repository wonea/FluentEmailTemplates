using System;
using System.IO;
using NUnit.Framework;

namespace VisualProduct.FluentEmailTemplates.UnitTests
{
    /// <summary>
    /// Base test class with helper methods.
    /// </summary>
    public class TestBase
    {
        private static DirectoryInfo _projectDirectory;
        private static DirectoryInfo _solutionDirectory;

        /// <summary>
        /// Helper method to assert the email json is as expected - given a relative file path
        /// to the expected json output.
        /// </summary>
        /// <param name="email">The email message.</param>
        /// <param name="relativeExpectedJsonFilePath">The file path to the expected json.</param>
        protected static void AssertExpectedEmailJson(IEmail email, string relativeExpectedJsonFilePath)
        {
            var json = email.GetEmailMessageJson();
            var expectedJsonFilePath = GetProjectRelativeFilePath(relativeExpectedJsonFilePath);
            var expectedJson = File.ReadAllText(expectedJsonFilePath);
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        /// <summary>
        /// Helper method to assert the email message's subject and html body are as expected.
        /// </summary>
        /// <param name="email">The email message.</param>
        /// <param name="expectedSubject">The expected subject.</param>
        /// <param name="relativeExpectedHtmlBodyFilePath">The relative file path to the html body.</param>
        protected static void AssertExpectedEmailSubjectAndHtmlBody(IEmail email, string expectedSubject, string relativeExpectedHtmlBodyFilePath)
        {
            var emailMessage = email.GetEmailMessage();

            // Assert subject.
            Assert.That(emailMessage.Subject, Is.EqualTo(expectedSubject));

            // Assert html body.
            var expectedHtmlBodyFilePath = GetProjectRelativeFilePath(relativeExpectedHtmlBodyFilePath);
            var expectedHtmlBody = File.ReadAllText(expectedHtmlBodyFilePath);
            Assert.That(emailMessage.HtmlBody, Is.EqualTo(expectedHtmlBody));
        }

        /// <summary>
        /// Helper method to assert the email template's subject and html body are as expected.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="expectedSubject">The expected subject.</param>
        /// <param name="relativeExpectedHtmlBodyFilePath">The relative file path to the html body.</param>
        protected static void AssertExpectedEmailTemplateSubjectAndHtmlBody(IEmailTemplate emailTemplate, string expectedSubject, string relativeExpectedHtmlBodyFilePath)
        {
            // Assert subject.
            Assert.That(emailTemplate.Subject, Is.EqualTo(expectedSubject));

            // Assert html body.
            var expectedHtmlBodyFilePath = GetProjectRelativeFilePath(relativeExpectedHtmlBodyFilePath);
            var expectedHtmlBody = File.ReadAllText(expectedHtmlBodyFilePath);
            Assert.That(emailTemplate.HtmlBody, Is.EqualTo(expectedHtmlBody));
        }

        /// <summary>
        /// Helper method to assert the text is as is expected (int the file).
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="relativeFilePath">The relative file path to the file.</param>
        protected static void AssertExpectedFileContents(string value, string relativeFilePath)
        {
            var expected = ReadAllTextFromProjectRelativeFilePath(relativeFilePath);

            // Assert the value is as expected..
            Assert.That(value, Is.EqualTo(expected));
        }

        /// <summary>
        /// Gets the project directory.
        /// </summary>
        protected static DirectoryInfo GetProjectDirectory()
        {
            if (_projectDirectory != null)
            {
                return _projectDirectory;
            }

            var uriAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            uriAssemblyPath = uriAssemblyPath.Substring(0, uriAssemblyPath.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase) + 1);

            var uncAssemblyPath = uriAssemblyPath.Replace("file:///", string.Empty);
            uncAssemblyPath = uncAssemblyPath.Replace('/', '\\');

            var fileInfo = new FileInfo(uncAssemblyPath);

            if (fileInfo.Directory != null && fileInfo.Directory.Parent != null && fileInfo.Directory.Parent.Parent != null)
            {
                _projectDirectory = fileInfo.Directory.Parent.Parent;
                return _projectDirectory;
            }

            throw new IOException("Cannot find the project directory.");
        }

        /// <summary>
        /// Gets the full name of the executing assembly's project directory.
        /// </summary>
        protected static string GetProjectDirectoryFullName()
        {
            var dir = GetProjectDirectory();
            return dir.FullName;
        }

        /// <summary>
        /// Gets the full file path to a file in the executing assembly's file path, given a relative file path.
        /// </summary>
        /// <param name="relativeFilePath">The relative file path.</param>
        protected static string GetProjectRelativeFilePath(string relativeFilePath)
        {
            var filePath = Path.Combine(GetProjectDirectoryFullName(), relativeFilePath);
            return filePath;
        }

        /// <summary>
        /// Gets the solution directory.
        /// </summary>
        protected static DirectoryInfo GetSolutionDirectory()
        {
            if (_solutionDirectory != null)
            {
                return _solutionDirectory;
            }

            var projectDirectory = GetProjectDirectory();
            if (projectDirectory.Parent != null)
            {
                _solutionDirectory = projectDirectory.Parent.Parent;
                return _solutionDirectory;
            }

            throw new IOException("Cannot find the solution directory.");
        }

        /// <summary>
        /// Reads the text from a file, given a relative file path in the project.
        /// </summary>
        /// <param name="relativeFilePath">The relative file path.</param>
        protected static string ReadAllTextFromProjectRelativeFilePath(string relativeFilePath)
        {
            var filePath = Path.Combine(GetProjectDirectoryFullName(), relativeFilePath);
            var text = File.ReadAllText(filePath);
            return text;
        }
    }
}
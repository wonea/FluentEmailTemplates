# Unit Tests
Unit tests in this assembly cover 100% of the code in the FluentEmailTemplates class library.

## Mock Unit Tests
The mock unit tests can be run at any time. Everything is mocked out.
Email messages will not be sent to an SMTP server.

## Integration Tests
The integration tests are decorated with [Explicit] attribute as they
send actual emails to the email address configured in the [App.config](App.config) file.

## App.config
Most settings in the [App.config](App.config) will need to be modified for the integration tests to work.

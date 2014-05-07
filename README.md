JobServe Jobs API Client
===================

Portable .Net Client to access [the JobServe Jobs API](http://services.jobserve.com).

The library is neither produced by, officially endorsed by, or in any way supported by JobServe Ltd.

I created this because I worked on the API, and I wanted to help people consume it.

See [the Version History](VersionHistory.md) for detailed information on how this client has evolbved over time.

Take a look at the [ClientTests.cs](JobServe.API.Client.Tests/ClientTests.cs) unit test for a few examples of how to use the client.  You'll also find some other tests of the service-oriented types as well in the same project.


Notes
-

See the unit tests project for a couple of examples of how to use the WebRequestManager class to access the API directly.

Also, there is now a Client that provides more structured access to the API by hiding the underlying HTTP work.

I've also added a service abstraction layer, designed to run over the top of the client, to make accessing the service even easier.

Want your own access token?
-

The client code has a default API token that is used if no other token is provided.  Whilst it is perfectly acceptable
to use this token, you can also head on over to the [Request Access page on the JobServe API website](https://services.jobserve.com/Developers/register)
to get your own token.  You should definitely do this if you are developing your own application or website.

Finally
-

This is created initially to answer a support question from a user that registered for access and then asked for some example code.

I encourage you to fork the library at your leisure, or use it as-is, I will also accept pull requests for any urgent issues.

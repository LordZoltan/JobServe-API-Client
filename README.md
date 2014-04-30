JobServe API Client
===================

Portable .Net Client to access [the JobServe API](http://services.jobserve.com).

The library is neither produced by, officially endorsed by, or in any way supported by JobServe Ltd.

I created this because I work for JobServe, I worked on the API, and I wanted to help people consume it.

See [the Version History](VersionHistory.md) for detailed information on how this client has evolbved over time.

Notes
-

See the unit tests project for a couple of examples of how to use the WebRequestManager class to access the API directly.

Also, there is now a Client that provides more structured access to the API by hiding the underlying HTTP work.

Want your own access token?
-

The client code has a default API token that is used if no other token is provided.  Whilst it is perfectly acceptable
to use this token, you can also head on over to the [Request Access page on the JobServe API website](https://services.jobserve.com/Developers/register)
to get your own token.  You should definitely do this if you are developing your own application or website.

Finally
-

This is created initially to answer a support question from a user that registered for access and then asked for some example code.

I encourage you to fork the library at your leisure, or use it as-is, I will also accept pull requests for any urgent issues.
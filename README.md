JobServe API Client
===================

Unofficial (but since I work for JobServe, it's as good as official) .Net client to access [the JobServe API](http://services.jobserve.com).

Notes
-

See the unit tests project for a couple of examples of how to use the client.  In it's current form, 
you will need VS2012 to be able to compile the code straight off, as it uses the async/await keywords.

Want your own access token?
-

The client code has a default API token that is used if no other token is provided.  Whilst it is perfectly acceptable
to use this token, you can also head on over to the [Request Access page on the JobServe API website](https://services.jobserve.com/Developers/register)
to get your own token.  You should definitely do this if you are developing your own application or website.

This is a work in progress
-

This is currently a work in progress initially to answer a support question from a user that registered for access and then asked for some example code.

Over time I hope to expand the solution to cover other flavours of .Net, and as the basis for a nuget package.

In the meantime, I will also accept pull requests for any urgent issues.
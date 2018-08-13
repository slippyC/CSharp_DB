# CSharp_DB

Primary Purpose:<br>
C# example using "Chinook" SQLite DB to show various SQL queries.

Secondary:
1) JSON requests of remote API's and then deserialization into Objects
2) Image loading from remote source

Known Issues:<br>
1) Not all Cover Art, that is remotely queried, is easily queryable.  There are quite a few reasons for this, but mainly because of naming conventions used for Album titles.  There is definitely no perfect solution, only multiple half working solutions that will have to be weaved together.
2) Images that are pulled from remote location take time to load.  Will either implement a loading spinner or create new table with byte[] to hold image data.  I have attempted to pull a thumbnail version instead of full version of the cover art.  Depending on internet connection, making multiple requests to server(at least coverartarchive.org) takes about same amount of time.  The full image, is of course, better resolution as well.  At this point haven't decided if full or thumbnail image will be final implementation.
3) Currently Artist Information is not displayed below cover art.  This is a very easy fix, just having the time(in my spare time) to implement.
4) Artist Filtering function does not work.  Deciding whether to use multiple lists or just re-querying database for information. Probably will re-query, since it is more scalable for people who look at this and have larger databases(don't want to take up so much memory).

Reasons for choosing SQLite:<br>
1) Portability
2) Do not have to setup Server to serve up SQL queries

This readme wouldn't be complete without thanking the people at Musicbrainz.org and Coverartarchive.org.  Without their work, the currently half, working code wouldn't be possible at all.  Well at least the pulling of cover art from some source.

WARNINGS:<br>
This code DOES NOT contain complete error handling.  Feel free to use sections of this code anyway you may choose.  It is here for learning purposes.  While there is no intent or knowledge of malicious code, use at your own risk.

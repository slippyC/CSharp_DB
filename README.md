# CSharp_DB

Primary Purpose:<br>
C# example using "Chinook" SQLite DB to show various SQL queries.

Secondary:
1) JSON requests of remote API's and then deserialization into Objects
2) Image loading from remote source

Known Issues:<br>
Not all Cover Art, that is remotely queried, is easily queryable.  There are quite a few reasons for this, but mainly because of naming conventions used for Album titles.  There is definitely no perfect solution, only multiple half working solutions that will have to be weaved together.
<br><br>
Reasons for choosing SQLite:<br>
1) Portability
2) Do not have to setup Server to serve up SQL queries

This readme wouldn't be complete without thanking the people at Musicbrainz.org and Coverartarchive.org.  Without their work, the currently half, working code wouldn't be possible at all.  Well at least the pulling of cover art from some source.

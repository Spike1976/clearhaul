# Driver offline model

`DriverPacket` keeps a server token and a local token. Download copies the server token onto the device. `ReadOffline` returns the local token with no network call. If the server token changes, the local copy still reads the old token. `Sync` replaces the local token only when connectivity is true.

Acknowledgement requires the local token to match the server token. A hazmat quantity change replaces the server token and does not erase the old local copy.

There is no Android application, no on-device database, and no encrypted document store in this repository. Those remain unbuilt. This type is the contract a later driver app has to meet.

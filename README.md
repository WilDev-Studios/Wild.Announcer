# Wild.Announcer
Unturned plugin to add auto-announcing messages through the OpenMod API

## How to Install
Make sure you are in-game and run this command:
`/openmod install Wild.Announcer`

## Documentation
*Config.yaml*
```yaml
Interval: 300 # Seconds between each announcement - Must be an int value
Random-Enabled: true # If announcements should be random and not by order - Must be a boolean value
Prevent-Duplicates: true # If random duplicate announcements should be prevented - Must be a boolean value

Announcements:
  - URL: my.example.one.url # URL to retrieve image from - Must be a full URL, no quotation marks
    Message: "Announcement 1" # Message to be sent to the entire server - Must be a string value - Useable Parameters: Rich Text <>
  - URL: my.example.two.url # URL to retrieve image from - Must be a full URL, no quotation marks
    Message: "Announcement 2" # Message to be sent to the entire server - Must be a string value - Useable Parameters: Rich Text <>
```
*Parameters*
- Rich Text <>: To add color and text formatting support for in-game text - Must be configured with <>

## Planned Additions
- Option for random announcements instead of going by list order
- Option for disabling duplicate announcements from the random announcements option

## Contact Us
### [Discord](https://discord.gg/4Ggybyy87d)

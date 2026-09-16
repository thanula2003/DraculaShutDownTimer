# Dracula Shutdown Timer

Dracula Shutdown Timer is a lightweight Windows desktop application for scheduling shutdown, restart, sleep, and lock actions.

It supports both countdown-based scheduling and scheduling an action for a specific time.

## Features

* Schedule a shutdown after a specified duration
* Schedule a restart after a specified duration
* Schedule sleep after a specified duration
* Schedule a lock after a specified duration
* Schedule actions at an exact time
* Hours, minutes, and seconds support
* AM/PM support for scheduled times
* Real-time countdown
* Abort scheduled actions
* Clear all entered values
* Self-contained Windows executable

## Download

The latest Windows release is available on the [Releases](../../releases) page.

Download the ZIP file, extract it, and run:

```text
DraculaShutdownTimer.exe
```

No separate .NET Runtime installation is required.

## Requirements

* Windows 10 or later
* 64-bit Windows

## Usage

There are two ways to schedule an action.

### After a Certain Time

Enter the required hours, minutes, and seconds, then select the action you want to perform.

For example:

```text
Hours:   2
Minutes: 30
Seconds: 0
```

The application will execute the selected action after 2 hours and 30 minutes.

### At an Exact Time

Enter the hour, minute, second, and select AM or PM.

For example:

```text
05 : 30 : 00 PM
```

If the selected time has already passed today, the action will be scheduled for the following day.

Only one scheduling method can be used at a time.

## Build

This project was developed using C#, .NET 10, and Windows Forms.

To publish a self-contained Windows x64 executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The published application will be located in:

```text
bin\Release\net10.0-windows\win-x64\publish\
```

## Important

The application performs real Windows shutdown, restart, sleep, and lock operations.

Save your work before scheduling an action.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Developer

Thanula Rajapaksha

GitHub: [thanula2003](https://github.com/thanula2003)

<img width="857" height="716" alt="Screenshot 2026-09-16 092225" src="https://github.com/user-attachments/assets/8eb20df6-bab8-47f5-a04c-307c81c9f9d6" />
<img width="1064" height="601" alt="Screenshot 2026-09-16 092238" src="https://github.com/user-attachments/assets/ab03ec55-80fb-451a-8b19-fdd1ca6bfcc0" />
<img width="1058" height="601" alt="Screenshot 2026-09-16 092308" src="https://github.com/user-attachments/assets/32a4e190-0996-4e48-b1fc-59be1aba8cd6" />



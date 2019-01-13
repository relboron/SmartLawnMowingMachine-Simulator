# Simple Smart Lawn Mowing Machine Simulator

Here some notes about the solution.

## SLMM.Api

This is a simple .NET Core 2.1 class library that is the core of the entire solution.

## SLMM.WebApi (StartUp Project)

This is an ASP.NET Core 2.1 WebApi that exposes some services over http. It exposes some actions to move the Lawn Moving Machine and get the current position.

It uses:

 - Kestrel as (cross-platform) web server for ASP.NET Core.
 - A configuration file "*gardenconfig.json*" for reading the initial values.
 
 In the "*Statup.cs*" the config file is read and a singleton of the "*SLMM.Api.SmartLawnMowingMachine*" is created and then injected into the control using the .NET Core dependency injection container.

### Execution

- Run the project SLMM.WebApi profile and launch the project.

- You can reach it at http://localhost:5000 using tools like SoapUI or Postman:

   Get Position = http://localhost:5000/get,
   Move Forward = http://localhost:5000/move/moveonestepforward,
   Turn Clockwise = http://localhost:5000/moveclockwise,
   Turn Anticlockwise = http://localhost:5000/move/moveanticlockwise


## SLMM.Api.Test

This is the test project, unit tests are performed using *MSTest* and *Moq* frameworks.

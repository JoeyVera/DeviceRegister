# DeviceRegister

# DeviceRegister

Deployment - Dev Mode

The solution does not have an actual deployment procedure but a required step in order to run the different projects in Visual Studio.
Before its execution a MSSQL Server (or express) database needs to be created. The script for creating the database is provided in the folder
\DeviceRegister\DatabaseScript

Once the database has been created, the solution can be run in Visual Studio.

* Important note: Database connection strings are hardcoded. Since this is not an actual app but an exercise that required a lot of topics
I have not externalized that info into config files. I will list the places where these have been hardcoded at the end of this document in case they need to be changed.


1 - Description of the architecture

The architecture has been driven by the different requirements of the exercise. For the data layer, the database contains 3 tables, one for each type of device. Obviously
creating only one table with a device type field would have been easier, but I understand and assume that the point of the exercise it to gather and manage different
datasources using an ORM, therefore I decided to store the devices split in different repositories.

a) Back End - The backend is an ASP.NET Core API app that listens to HTTP requests and adds the POSTed devices to the MSSQL database. It should be started with the Visual
Studio IIS Express.

b) Front End 1 - As a frontend I have created a static HTML page that can be found in the folder \DeviceRegister\HTMLFrontEnd. The file registration.html can be open with
any recent browser directly. It is important - if asked - to allow blocked content in order to enable the javascript functionality.

c) Front End 2 - I have created a .net console application that accept commands to create devices and store them in the DB throughout the BackEnd. The command line
executable file can be found in the \DeviceRegister\AddDevice\bin\Debug\netcoreapp2.2\ folder with the following example syntaxis:

dotnet adddevice.dll -gw -s:'114-b12_24' -b:'General_Electrics' -i:'192.168.1.1' -p:'8080'

Full description can be read using:

dotnet adddevice.dll -?

d)Front End 3 - The requirements also suggested a Windows Desktop app Frontend, however I consider this unnecessary because I would not add any additional value to the exercise.

e) DeviceRandomGenerator. In order to implement an NServiceBus example I have created this Console App that creates a device every 10 seconds and send it to the BUS queue.

f) DeviceRegisterNServiceBus. This is another console application that read the NServiceBus application and store the device in the database. Initially I wanted the backend
to perform this function - and I actually have another version of the solution that works this way - but there is an issue adding this feature to the ASP.NET API: When the
server is started by the IIS Express, it does not wake up until the first HTTP request is received, therefore the queue stays unread until that moment. Because of this
I decided to create a specific app to perform this action.


2 - Execution of the different project

As commented above, it is required to create the database before using any of the different apps provided. There is no additional requirement. Needless to say for the Frontends
to operate it is necessary the backend to be up and running.

The DeviceRandomGenerator and DeviceRegisterNServiceBus can be started independently and will work following the NServiceBus decoupling feature.

If the solution is started with the "Start" button of VS or the "F5" key, the Backend, the DeviceRandomGenerator, and the DeviceRegisterNServiceBus will be started.

There is an additional project "Platform" provided by the NServiceBus resources that I found very useful to understand the product and monitorized the communication through
the BUS, It is not longer required by the other apps, but I have left it included for experimentation.


3 - Comments, notes and assumptions

A unit test project has been created for the Backend. Since the rest of the app are very small, I did not consider to add unit test to those. Also, many smaill improvements
occurred to me during the development - in example: to use a RegEx function to control the gateway IP has the right format - but all these minor things were left aside to
spend the rest of my available time to research the NServiceBus that I recon to be more important in the result of the exercise.

I try as much as possible to write clear code and SOLID principles, again always thinking on saving time for the rest of the tasks. There are not many comments because
the exercise does not contain much of complicate procedures.

I have recorded a Review here:

https://www.youtube.com/watch?v=NOR-uWpikBI&feature=youtu.be

Last thing, the list of hardcoded references, just in case you want to run the solution with different configuration:

- DeviceRegister.Startup.Configuration: var connection = @"Server=.;Database=Devices;Trusted_Connection=True;";

- DeviceRegister.Models.DevicesContext.OnConfiguring: optionsBuilder.UseSqlServer(@"Server=.;Database=Devices;Trusted_Connection=True;");

- AddDevice.Program: const string url = "http://localhost:26647/api/Devices/";

- HTML Front End, in the section:

$.ajax({
	url: 'http://localhost:26647/api/Devices',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(device),
        dataType: "json",
        success: function (result) {
        alert("The device has been saved.");
        },
        error: function (err) {
        	alert(err.responseText);
        }
});

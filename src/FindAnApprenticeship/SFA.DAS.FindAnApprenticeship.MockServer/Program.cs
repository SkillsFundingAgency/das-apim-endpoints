using SFA.DAS.FindAnApprenticeship.MockServer;


Console.WriteLine("Mock Server starting on http://localhost:5051");

MockApiServer.Start();

Console.WriteLine(("Press any key to stop the server"));
Console.ReadKey();
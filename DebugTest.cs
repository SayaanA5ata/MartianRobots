using MartianRobots.IO;
using MartianRobots.Models;

var line = "5 3";
Console.WriteLine($"Parsing line: '{line}'");
Console.WriteLine($"Line length: {line.Length}");
Console.WriteLine($"Is null or whitespace: {string.IsNullOrWhiteSpace(line)}");

try 
{
    var surface = MissionCommandParser.ParseMarsSurface(line);
    Console.WriteLine($"Success! Surface created: {surface.MaxX} x {surface.MaxY}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Exception type: {ex.GetType().Name}");
}
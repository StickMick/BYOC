using BYOC.Data.Objects;
using BYOC.Shared.DTOs;

namespace BYOC.ConsoleClient.Helpers;

public static class MapVisualizer
{
    public static void DrawToConsole(WorldDTO world)
    {
        for (int i = 0; i < world.Width; i++)
        {
            for (int j = 0; j < world.Height; j++)
            {
                Console.SetCursorPosition(i, j);
                
                if (world.Nodes.SingleOrDefault(n=>n.Position.X == i && n.Position.Y == j)?.IsWalkable ?? false)
                {
                    Console.Write(' ');
                }
                else
                {
                    Console.Write('X');
                }
            }
        }
    }

    public static void DrawPath(WorldDTO? world, IEnumerable<Position> path)
    {
        Console.CursorVisible = false;
        DrawToConsole(world);
        foreach (Position position in path)
        {
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write('.');
        }
        
        Console.SetCursorPosition(path.First().X, path.First().Y);
        Console.Write('S');
        Console.SetCursorPosition(path.Last().X, path.Last().Y);
        Console.Write('E');
    }
}
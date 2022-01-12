using BYOC.Data.Objects;

namespace BYOC.Data.Helpers;

public static class MapVisualizer
{
    public static void DrawToConsole(IWorld world)
    {
        for (int i = 0; i < world.Width; i++)
        {
            for (int j = 0; j < world.Height; j++)
            {
                Console.SetCursorPosition(i, j);
                
                if (world.Nodes[i, j]?.IsWalkable ?? false)
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

    public static void DrawPath(IWorld world, IEnumerable<Position> path)
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
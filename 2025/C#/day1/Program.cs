namespace Day1;

class Program
{
  static void Main(string[] args)
  {
    if (args.Length == 0)
    {
      Console.WriteLine($"No arguments provided. Syntax: {Environment.GetCommandLineArgs()[0]} <instructionfile>");
      return;
    }

    var safe = new Safe();

    try
    {
      StreamReader reader = new StreamReader(args[0]);
      String? line;
      while ((line = reader.ReadLine()) != null)
      {
        RotateInstruction instruction = RotateInstruction.Parse(line);
        safe.Rotate(instruction);
      } 
    }
    catch(Exception e) // Copied from docs
    {
      Console.WriteLine("Exception: " + e.Message);
      return;
    }

    uint password = safe.GetPassword();
    Console.WriteLine($"The password is: {password}");
  }
}

public class Safe {
  int currentNumber = 50;
  uint password = 0;

  public void Rotate(RotateInstruction instruction)
  {
    this.currentNumber += instruction.Direction switch {
      RotateDirection.Right => instruction.Distance,
      RotateDirection.Left => -instruction.Distance,
    };

    while (this.currentNumber < 0)
      this.currentNumber = this.currentNumber + 100;
    while (this.currentNumber > 99)
      this.currentNumber = this.currentNumber - 100;

    if (this.currentNumber == 0)
      this.password++;
  }

  public uint GetPassword()
  {
    return this.password;
  }
}

public record RotateInstruction(RotateDirection Direction, int Distance)
{
  public static RotateInstruction Parse(string input)
  {
    if (input.Length < 2)
      throw new FormatException($"Instruction {input} is invalid");

   RotateDirection direction = input[0] switch
    {
      'R' => RotateDirection.Right,
      'L' => RotateDirection.Left,
      _ => throw new FormatException($"Invalid Direction {input[0]} in instruction {input}"),
    };
    int distance = int.Parse(input.Substring(1));
    return new RotateInstruction(direction, distance);
  }
}

public enum RotateDirection
{
  Left,
  Right,
}

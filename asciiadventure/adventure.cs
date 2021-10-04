using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure {
  public class Game {
    private Random random = new Random();
    private static Boolean Eq(char c1, char c2){
      return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private static string Menu() {
      return "WASD to move\nIJKL to attack/protect\nEnter command: ";
    }
    
    private static void PrintScreen(Screen screen, string message, string menu) {
      Console.Clear();
      Console.WriteLine(screen);
      Console.WriteLine($"\n{message}");
      Console.WriteLine($"\n{menu}");
    }
    public void Run() {
      Console.ForegroundColor = ConsoleColor.Green;

      Screen screen = new Screen(10, 20);
      // add a couple of walls
      for (int i=0; i < 7; i++){ //wall1
        new Wall(2, 2 + i, screen);
      }
      new Wall(6,18, screen);
      for (int i=0; i < 3; i++){ //wall4
        new Wall(6, 9 + i, screen);
      }
      for (int i=0; i < 4; i++){ //wall2
        new Wall(5 + i, 4, screen);
      }
      for (int i=0; i < 4; i++){ //wall3
        new Wall(2 + i, 13, screen);
      }
      new Portal1(1,5, screen);
      new Portal2(5,18, screen);
      
      // add a player
      Player player = new Player(0, 0, screen, "Zelda");
      
      // add a treasure
      Treasure treasure = new Treasure(6, 5, screen);
      Treasure treasure2 = new Treasure(3, 16, screen);

      Shield shield = new Shield(random.Next(0,10), random.Next(0,20), screen);


      // add some mobs
      List<Mob> mobs = new List<Mob>();
      mobs.Add(new Mob(9, 19, screen));
      mobs.Add(new Mob(0, 19, screen));
      mobs.Add(new Mob(9, 0, screen));
      
      // initially print the game board
      PrintScreen(screen, "Welcome!", Menu());
      Boolean gameOver = false;
      Boolean Action = false;
      Boolean hasShield = false;
      int treasures = 0;
      
      while (!gameOver) {
          char input = Console.ReadKey(true).KeyChar;
          Action = false;
          String message = "";

          if (Eq(input, 'q')) {
            break;
          } else if (Eq(input, 'w')) {
            player.Move(-1, 0);
          } else if (Eq(input, 's')) {
            player.Move(1, 0);
          } else if (Eq(input, 'a')) {
            player.Move(0, -1);
          } else if (Eq(input, 'd')) {
            player.Move(0, 1);
          } else if (Eq(input, 'i')) {
            if(player.InventoryCheck(-1, 0) == 2){
              hasShield = true;
            }
            else if(player.InventoryCheck(-1, 0) == 1){
              treasures++;
            }
            message += player.Action(-1, 0) + "\n";
            Action = true;
          } else if (Eq(input, 'k')) {
            if(player.InventoryCheck(1, 0) == 2){
              hasShield = true;
            }
            else if(player.InventoryCheck(1, 0) == 1){
              treasures++;
            }
            message += player.Action(1, 0) + "\n";
            Action = true;
          } else if (Eq(input, 'j')) {
            if(player.InventoryCheck(0, -1) == 2){
              hasShield = true;
            }
            else if(player.InventoryCheck(0, -1) == 1){
              treasures++;
            }
            message += player.Action(0, -1) + "\n";
            Action = true;
          } else if (Eq(input, 'l')) {
            if(player.InventoryCheck(0, 1) == 2){
              hasShield = true;
            }
            else if(player.InventoryCheck(0, 1) == 1){
              treasures++;
            }
            message += player.Action(0, 1) + "\n";
            Action = true;
          } else if (Eq(input, 'v')) {
            // TODO: handle inventory
            message = "You have nothing\n";
          } else {
            message = $"Unknown command: {input}";
          }
          if(treasures == 2){
            message += "YOU WIN! YOU GOT ALL THE TREASURE BEFORE THE MOBS\n";
                Console.ForegroundColor = ConsoleColor.Blue;
            gameOver = true;
          }

          // OK, now move the mobs
          foreach (Mob mob in mobs){
            // TODO: Make mobs smarter, so they jump on the player, if it's possible to do so
            List<Tuple<int, int>> moves = screen.GetLegalMoves(mob.Row, mob.Col);
            if (moves.Count == 0){
                continue;
            }
            // NOTE: the version of the C# compiler on Repl.it cannot
            // handle destructuring of Tuples, so we have to explicitly
            // get the tuple, and then extract each of the 2 values.
            // I am not sure why this is the case.
            //var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];
            Tuple<int, int> t = moves[random.Next(moves.Count)];
            int deltaRow = t.Item1;
            int deltaCol = t.Item2;
            
            

            if (screen[mob.Row + deltaRow, mob.Col + deltaCol] is Player){
              if(Action){
                //mob.Delete();
                //mob.Token = " ";
                mob.Move(-2*deltaRow, -2*deltaCol);
                message += "Continue\n";
                break;
              }
              if(hasShield){
                //mob.Delete();
                //mob.Token = " ";
                mob.Move(-deltaRow, -deltaCol);
                message += "Continue\n";
                hasShield = false;
                break;
              }
                // the mob got the player!
                mob.Token = "*";
                message += "A MOB GOT YOU! GAME OVER\n";
                Console.ForegroundColor = ConsoleColor.Red;
                gameOver = true;
            }

            else if (screen[mob.Row + deltaRow, mob.Col + deltaCol] is Treasure){
                // the mob got the player!
                mob.Token = "$";
                message += "A MOB GOT THE TREASURE! GAME OVER\n";
                Console.ForegroundColor = ConsoleColor.Red;
                gameOver = true;
            }
            else{
            mob.Move(deltaRow, deltaCol);
            }
          }

          PrintScreen(screen, message, Menu());
      }
    }

    public static void Main(string[] args){
      Game game = new Game();
      game.Run();
    }
  }
}
using System;

namespace asciiadventure {
    class Player : MovingGameObject {
        public Player(int row, int col, Screen screen, string name) : base(row, col, "@", screen) {
            Name = name;
            screen.MC = this;
        }
        public string Name {
            get;
            protected set;
        }
        public override Boolean IsPassable(){
            return true;
        }
        public int InventoryCheck(int deltaRow, int deltaCol){
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol)){
                return 0;
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null){
                return 0;
            }
            if (other is Treasure){
                return 1;
            }
            if(other is Shield){
              return 2; 
            }
            else
            return 0;
        }
        public String Action(int deltaRow, int deltaCol){
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol)){
                return "nope";
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null){
              new Wall(newRow, newCol, Screen);
                return "#=Protected=#";
            }
            // TODO: Interact with the object
            if (other is Treasure){
                other.Delete();
                return "Yay, we got the treasure!";
            }
            if(other is Mob){
              return "You attacked a mob";
            }
            if(other is Shield){
              other.Delete();
              return "You picked up a shield";
              
            }
            if(other is Portal1){
             this.Move(4,13);
             return "You traveled through a portal OO";
            }

            if(other is Portal2){
             this.Move(-4,-13);
             return "You traveled through a portal OO";
            }
            
            
            return "ouch";
        }
    }
}
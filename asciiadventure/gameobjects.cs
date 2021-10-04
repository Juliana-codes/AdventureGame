using System;


namespace asciiadventure {
    public abstract class GameObject {
        
        public int Row {
            get;
            protected set;
        }
        public int Col {
            get;
            protected set;
        }

        public String Token {
            get;
            protected internal set;
        }

        public Screen Screen {
            get;
            protected set;
        }

        public GameObject(int row, int col, String token, Screen screen){
            Row = row;
            Col = col;
            Token = token;
            Screen = screen;
            Screen[row, col] = this;
        }
        public string ToToken() {
            return Token;
        }

        public virtual Boolean IsPassable() {
            return false;
        }

        public override String ToString() {
            return this.ToToken();
        }

        public void Delete() {
            Screen[Row, Col] = null;
            
        }
    }


    public abstract class MovingGameObject : GameObject {

        public MovingGameObject(int row, int col, String token, Screen screen) : base(row, col, token, screen) {}
        
        public string Move(int deltaRow, int deltaCol) {
            int newRow = deltaRow + Row;
            int newCol = deltaCol + Col;
            if (!Screen.IsInBounds(newRow, newCol)) {
                return "";
            }
            
            
            GameObject gameObject = Screen[newRow, newCol];
            if (gameObject != null && !gameObject.IsPassable()) {
                // TODO: How to handle other objects?
                // walls just stop you
                // objects can be picked up
                // people can be interactd with
                // also, when you move, some things may also move
                // maybe i,j,k,l can attack in different directions?
                // can have a "shout" command, so some objects require shouting
                return "TODO: Handle interaction";
            }
            
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Row = newRow;
            Col = newCol;
            //if(this is Player){
            //  return "";
            //}
            Screen[originalRow, originalCol] = null;
            Screen[Row, Col] = this;
            return "";
        }
    }

    class Wall : GameObject {
        public Wall(int row, int col, Screen screen) : base(row, col, "=", screen) {}
    }
    
    class Portal1 : GameObject {
        public Portal1(int row, int col, Screen screen) : base(row, col, "O", screen) {}
    }
    class Portal2 : GameObject {
        public Portal2(int row, int col, Screen screen) : base(row, col, "O", screen) {}
    }

    class Shield : GameObject {
       public Shield(int row, int col, Screen screen) : base(row, col, "S", screen) {}
      public override Boolean IsPassable() {
            return true;
        }
    }

    class Treasure : GameObject {
        public Treasure(int row, int col, Screen screen) : base(row, col, "T", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
    }
    }



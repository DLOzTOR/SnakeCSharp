using System;
using System.Threading;
using System.Collections.Generic;


namespace Snake
{
    class SnakeGame{
        private int width,height,speed,frameTime,maxScore,score;
        private const char snakeHeadChar = '0', snakeBodyChar = 'o', groundChar = ' ', borderChar = '#', appleChar='@';
        private char[,] field;
        private enum Move{UP,DOWN,LEFT,RIGHT};
        private Random rand = new Random();
        private bool appleExsist;
        ConsoleKey c = ConsoleKey.W;
        private enum State{Play,Win,Lose};
        private State gameState;
        private struct Cords{
            public int x,y;
            public void Set(int x,int y){
                this.x=x;
                this.y=y;
            }
            public void Change(int x,int y,int maxX,int maxY){
                this.x=overflowField(this.x+x,maxX);
                this.y=overflowField(this.y+y,maxY);
            }
            
            private int overflowField(int val, int max){
                if(val<0)val=max-1;
                if(val>max-1)val=0;
                return val;
            }
        }
        private Cords[] snake;
        private Cords apple;
        private Move move;
        Dictionary<ConsoleKey, Move> keyBinds = new Dictionary<ConsoleKey, Move>(){
            {ConsoleKey.W, Move.UP},
            {ConsoleKey.S, Move.DOWN},
            {ConsoleKey.A, Move.LEFT},
            {ConsoleKey.D, Move.RIGHT},
        };
        private void createField(){
            field = new char[width,height];
            for(int j = 0; j < height; j++){
                for(int i = 0; i < width; i++){
                    field[i,j]=groundChar;
                }
            }
        }
        private void createSnake(){
            snake = new Cords[maxScore+3];
            for(int i = 0; i <3; i++){
                snake[i].Set(width/2,height/2+i);
                field[snake[i].x,snake[i].y]=snakeBodyChar;
            }
            field[snake[0].x,snake[0].y]=snakeHeadChar;
        }
        private void input(){
            if (Console.KeyAvailable){c = Console.ReadKey(true).Key;
            keyBinds.TryGetValue(c,out move);}
        }
        private void addScore(){
            score+=1;
            if(score>=maxScore)gameState=State.Win;
        }
        private void moveSnake(){
            Cords temp = snake[0];
            switch(move){
                case(Move.UP):
                    temp.Change(0,-1,width,height);
                    break;
                case(Move.DOWN):
                    temp.Change(0,1,width,height);
                    break;
                case(Move.LEFT):
                    temp.Change(-1,0,width,height);
                    break;
                case(Move.RIGHT):
                    temp.Change(1,0,width,height);
                    break;
            }
            if(temp.x==apple.x&&temp.y==apple.y){addScore();appleExsist=false;}
            if(field[temp.x,temp.y]==snakeBodyChar){gameState=State.Lose;}
            field[snake[score+2].x,snake[score+2].y]=groundChar;         
            for(int i = score + 2; i > 0 ; i--){
                snake[i]=snake[i-1];
            }   
            snake[0]=temp;
        }
        private void renderSnake(){
            for(int i=1;i<score+2;i++){
                field[snake[i].x,snake[i].y]=snakeBodyChar;
            }
            field[snake[0].x,snake[0].y]=snakeHeadChar;
        }
        private void createApple(){
            if(!appleExsist){
                while(!appleExsist){
                    apple.Set(Convert.ToInt32(rand.NextInt64(width)),Convert.ToInt32(rand.NextInt64(height)));
                    for(int i =0; i < score +3 ;i++)if(snake[i].x!=apple.x && snake[i].y!=apple.y)appleExsist= true;
                }
                if(appleExsist)field[apple.x,apple.y]=appleChar;
            }
        }
        private void outputField(){
            for(int j = 0; j < height; j++){
                for(int i = 0; i < width; i++){
                    Console.SetCursorPosition(i,j);
                    Console.Write(field[i,j]);
                }
            }
        }


        private void Start(int width, int height,int speed){
            this.width = width >= 5 ? width : 5; 
            this.height = height>= 5 ? height : 5;
            this.speed = speed > 0 ? speed : 1;
            Console.SetWindowSize(this.width,this.height);
            maxScore = 7;
            gameState=State.Play;
            move=Move.UP;
            appleExsist =false;
            frameTime = 1000/speed;
            score=0;
            snake= new Cords[0];
            createField();
            createSnake();
            Thread.Sleep(1000);
        }
        private void Update() {
            while(gameState==State.Play){
                createApple();
                input();
                moveSnake();
                renderSnake();
                outputField();
                Thread.Sleep(frameTime);
            }
        }
        private void Final(){
            Console.Clear();
            Console.WriteLine(gameState);
            Console.WriteLine("Scr:{0}",score);
            Console.WriteLine("Restart");
            Console.WriteLine("y-yes");
            Console.WriteLine("Else no");
            bool restart= ConsoleKey.Y == Console.ReadKey(true).Key;
            if(restart)Run(width,height,speed);
        }
        public void Run(int width, int height,int speed){
            Start(width,height,speed);
            Update();
            Final();
        }
    }
}
/*      TODO
Start done
    SetSettings done
    crtField done 
    crtSnake done
Update doen
    input done
    crtApple done
    moveSnake done
        collision done
            eatApple done
                extendSnake done
                win done
                lose done
Final done
    restart done
*/

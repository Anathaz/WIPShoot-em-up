using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Core;
using System.Collections.Generic;
using System.IO;

namespace Shmup {

    // ===== Data Types =====

    public class SpriteSheet {
        
        private string imagefile;
        
        private List<Rectangle> rects = new List<Rectangle>();

        private SpriteSheet() {
        }

        public static SpriteSheet load(string configfile) {
            SpriteSheet ss = new SpriteSheet();
            StreamReader sr = new StreamReader(configfile);
            string line = sr.ReadLine();
            line = sr.ReadLine();
            string[] fields = line.Split('=');
            ss.imagefile = fields[1];
            while (!sr.EndOfStream) {
                line = sr.ReadLine();
                fields = line.Split(',');
                int x = int.Parse(fields[1]);
                int y = int.Parse(fields[2]);
                int w = int.Parse(fields[3]);
                int h = int.Parse(fields[4]);
                Rectangle r = new Rectangle(x,y,w,h);
                ss.rects.Add(r);
            }
            return ss;
        }

        public Rectangle getRectangle(int sprite) {
            return rects[sprite];
        }

        public string getImagefile() {
            return imagefile;
        }
    }

    public class Ship {

        // state

        private int x;
        private int y;
        private int dx;
        private int dy;
        private int sprite;
        private int direction;

        // constructors

        public Ship(int x, int y, int sprite, int direction, int dx, int dy) {
            this.x = x;
            this.y = y;
            this.sprite = sprite;
            this.direction = direction;
        }

        // behaviour 

        public void setX(int x) {
            this.x = x;
        }

        public int getX() {
            return x;
        }
        public void setdy(int dy)
        {
            this.dy = dy;
        }
        public void setdx(int dx)
        {
            this.dx = dx;
        }
        public void setY(int y) {
            this.y = y;
        }

        public int getY() {
            return y;
        }
        public void setSprite(int sprite)  {
            this.sprite = sprite;
        }

        public int getSprite() {
            return sprite;
        }

        public void setDirection(int direction) {
            this.direction = direction;
        }

        public int getDirection() {
            return direction;
        }
        public void move() {
            x += dx;
            y += dy;
        }
    }

    public class Asteroid {

        // state

        private int x;
        private int y;
        private int dx;
        private int dy;
        private int sprite;
        private int direction;
        private int rotation;

        // constructors

        public Asteroid(int x, int y, int sprite, int direction, int rotation, int dx, int dy) {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            this.sprite = sprite;
            this.direction = direction;
            this.rotation = rotation;
        }

        // behaviour 

        public void setX(int x) {
            this.x = x;
        }

        public int getX() {
            return x;
        }

        public void setY(int y) {
            this.y = y;
        }

        public int getY() {
            return y;
        }

        public void setSprite(int sprite) {
            this.sprite = sprite;
        }

        public int getSprite() {
            return sprite;
        }

        public void setDirection(int direction) {
            this.direction = direction;
        }

        public int getDirection() {
            return direction;
        }

        public void setRotation(int rotation) {
            this.rotation = rotation;
        }

        public int getRotation() {
            return rotation;
        }

        public void move() {
            x += dx;
            y += dy;
            
            if (rotation != 0) {
                direction = (direction + rotation) % 360;
            }
        }

    }

    // -- Laser --

    public class Laser {

        private int sprite;
        private Beam beam;

        public Laser(int sprite, double x, double y, double velocity, double direction) {
            this.sprite = sprite;
            this.beam = new Beam(x,y,velocity,direction);
        }

        public int getSprite() {
            return sprite;
        }

        public Beam getBeam() {
            return beam;
        }

    }

    public class Map    {

        private int height, width;
        private int[] tiles;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            tiles = new int[width*height];
            GenerateLevel();
        }

        public Map(string path)
        {
            LoadMap(path);
        }

        protected void GenerateLevel()
        {

        }

        private void LoadMap (string path)
        {

        }
    }
   
    class Tile: Map
    {
        private bool collision;
    }
    class Wall : Tile
    {
        private int sprite;
    }
    class Floor: Tile
    {
        private int sprite;
    }
           // -- Beam --

    public class Beam {

        private double mass;
        private Position position;
        private Motion motion;

        public Beam(double x, double y, double velocity, double direciton) {
            this.position = new Position(x,y);
            this.motion = new Motion(velocity,direciton);
        }

        public Position getPosition() {
            return position;
        }

        public Motion getMotion() {
            return motion;
        }

    }
    
    // -- Body --

    public class Body {

        private double mass;
        private Position position;
        private Motion motion;

        public Body(double mass, double x, double y, double velocity, double direciton) {
            this.mass = mass;
            this.position = new Position(x,y);
            this.motion = new Motion(velocity,direciton);
        }

        public double getMass() {
            return mass;
        }

        public void setMass(double mass) {
            this.mass = mass;
        }

        public Position getPosition() {
            return position;
        }

        public Motion getMotion() {
            return motion;
        }

    }

    // -- Position --

    public class Position {

        private double x;
        private double y;

        public Position(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public double getX() {
            return x;
        }

        public void setX(double x) {
            this.x = x;
        }

        public double getY() {
            return y;
        }

        public void setY(double y) {
            this.y = y;
        }

    }

    // -- Motion --

    public class Motion {

        private double velocity;
        private double direction;

        public Motion(double velocity, double direction) {
            this.velocity = velocity;
            this.direction = direction;
        }

        public double getVelocity() {
            return velocity;
        }

        public void setVelocity(double velocity) {
            this.velocity = velocity;
        }

        public double getDirection() {
            return direction;
        }

        public void setDirection(double direction) {
            this.direction = direction;
        }

    }

    // -- 2D Netwtonian Physics Model --

    public class Newtonian {

        private Newtonian() {
        }

        public static void move(Position position, Motion motion) {
            // Calcluate the new position given the motion.
            // Update the position.
            double rad = motion.getDirection() * Math.PI / 180.0;
            double dx = motion.getVelocity() * Math.Cos(rad);
            double dy = motion.getVelocity() * Math.Sin(rad) * -1.0;
            position.setX(position.getX() + dx);
            position.setY(position.getY() + dy);
        }

    }

    // -- Program, Entry Point --

    static class Program {

        // STATE
        // Keep the state of the elements of the game here (variables hold state).

        static Random rnd = new Random();
        static SpriteSheet ss;

        static Ship ship;
        static Ship aim;
        static Ship Weapon;
        static Laser laser;

        // This procedure is called (invoked) before the first time onTick is called.
        static void onInit() {
            Weapon = new Ship(0,0, 1, 0, 0, 0);
            ship = new Ship(FRAME_WIDTH/2, FRAME_HEIGHT/2, 0, 0, 0, 0);
            aim = new Ship(0, 0, 2, 0, 0, 0);
            
        }

        // This procedure is called (invoked) for each window refresh
        static void onTick(object sender, TickEventArgs args) {

            // STEP
            // Update the automagic elements and enforce the rules of the game here.
            HasCollision();
            ship.move();
            DirectionLocation();
            WeaponSet();
            if (ship.getX() < 0) {
                ship.setX(FRAME_WIDTH);
            } else if (ship.getX() > FRAME_WIDTH) {
                ship.setX(0);
            }

            if (ship.getY() < 0) {
                ship.setY(0);
            } else if (ship.getY() > FRAME_HEIGHT) {
                ship.setY(FRAME_HEIGHT );
            }
            if (laser != null) {
                Beam beam = laser.getBeam();
                Newtonian.move(beam.getPosition(),beam.getMotion());
                if (   (beam.getPosition().getX() < 0) 
                    || (beam.getPosition().getX() > FRAME_WIDTH)
                    || (beam.getPosition().getY() < 0)
                    || (beam.getPosition().getY() > FRAME_HEIGHT)) {
                        laser = null;
                } 
            }
            HasCollision();
            // DRAW
            // Draw the new view of the game based on the state of the elements here.
            drawBackground(WorldX = WorldX + MovementX, WorldY = WorldY + MovementY);

            if (laser != null) { 
                drawSprite(laser.getSprite(),(int)laser.getBeam().getPosition().getX(), (int)laser.getBeam().getPosition().getY(), (int)laser.getBeam().getMotion().getDirection()+90);
            }
            
            drawSprite(aim.getSprite(), aim.getX(), aim.getY(), aim.getDirection());
            drawSprite(Weapon.getSprite(), Weapon.getX(), Weapon.getY(), Weapon.getDirection());
            drawSprite(ship.getSprite(),ship.getX(),ship.getY(),ship.getDirection());
            // ANIMATE 
            // Step the animation frames ready for the next tick
            // ...

            // REFRESH
            // Tranfer the new view to the screen for the user to see.
            video.Update();

        }

        static void WeaponSet()
        {
            Weapon.setX(ship.getX()+10);
            Weapon.setY(ship.getY()+5);
        }
        // this procedure is called when the mouse is moved
        static void onMouseMove(object sender, SdlDotNet.Input.MouseMotionEventArgs args) {
            aim.setX(args.X);
            aim.setY(args.Y);
            DirectionLocation();
            
        }
        
        static void DirectionLocation()
        {

            double dx = (Weapon.getX() - aim.getX());
            double dy = -(Weapon.getY() - aim.getY());

            double angle = Math.Atan(dy / dx) * 180.0f / Math.PI + ((dy < 0 && dx >= 0) ? 180.0f : 0.0f);
            if (dy > 0 && dx >= 0)
            {
                angle = angle + 180.0f;
            }

            Weapon.setDirection((int)angle);
        }
        // this procedure is called when a mouse button is pressed or released
        static void onMouseButton(object sender, SdlDotNet.Input.MouseButtonEventArgs args) {
            if (args.ButtonPressed)
            {
                switch (args.Button)
                {
                    case SdlDotNet.Input.MouseButton.PrimaryButton:
                       if (laser == null) {
                           laser = new Laser(3, Weapon.getX(), Weapon.getY(), 12.0, Weapon.getDirection());
                        }
                        break;
                }
            }
        }

        // this procedure is called when a key is pressed or released
        static void onKeyboard(object sender, SdlDotNet.Input.KeyboardEventArgs args) {
            if (args.Down) { 

                switch (args.Key) {
                    case SdlDotNet.Input.Key.D:
                        MovementX = -3;
                        break;
                    case SdlDotNet.Input.Key.A:
                        MovementX = 3;
                        break;
                    case SdlDotNet.Input.Key.W:
                        MovementY = 3;
                        break;
                    case SdlDotNet.Input.Key.S:
                        MovementY = -3;
                        break;
                    case SdlDotNet.Input.Key.Space:
                        break;
                    case SdlDotNet.Input.Key.Escape:
                        Environment.Exit(0);
                        break;
                }
            }else{

                switch (args.Key) {
                    case SdlDotNet.Input.Key.D :
                    case SdlDotNet.Input.Key.A :
                        MovementX = 0;
                        break;
                    case SdlDotNet.Input.Key.W:
                    case SdlDotNet.Input.Key.S:
                        MovementY = 0;
                        break;
                
                }

            }
            
        }


        // --------------------------
        // ----- GAME Utilities -----  
        // --------------------------

        // draw the background image onto the frame
        static void drawBackground(int x, int y)
        {
            video.Blit(background);
            video.Blit(Map, new Point(x, y));
        }

        // draw the sprite image onto the frame
        // Sprite sprite - which sprite to draw
        // int x, int y - the co-ordinates of where to draw the sprite on the frame.
        static void drawSprite(int sprite, int x, int y, int direction) {
            Surface temp1 = sprites.CreateSurfaceFromClipRectangle(ss.getRectangle(sprite));
            Surface temp2 = temp1.CreateRotatedSurface(direction,false);
            Surface temp3 = temp2.Convert(video,false,false);
            temp3.SourceColorKey = temp3.GetPixel(new Point(0, 0));
            video.Blit(temp3, new Point(x - (temp3.Width / 2), y - (temp3.Height / 2)));
            temp3.Dispose();
            temp2.Dispose();
            temp1.Dispose();
        }

        static bool IsPointRect(int x, int y, int rx, int ry, int rw, int rh)
        {
            return (x >= rx) && (x <= rx + rw) && (y >= ry) && (y <= ry + rh);
        }

        static bool Intersect(int r1x, int r1y, int r1h, int r1w, int r2x, int r2y, int r2h, int r2w)
        {
            return !(r1x + r1w < r2x || r1x > r2x + r2w || r1y + r1h < r2y || r1y > r2y + r2h);
        }
        static void HasCollision()
        {
            if(Intersect(ship.getX(), ship.getY(), 26, 8 , 0, 0, 2880, 2262))
            {

            }
            else
            {
                MovementX = 0;
                MovementY = 0;
            }
        }
        // -- APPLICATION ENTRY POINT --

        static void Main() {
            //System.Windows.Forms.Cursor.Hide();
            ss = SpriteSheet.load("images/config.csv");
            // Create display surface.
            video = Video.SetVideoMode(FRAME_WIDTH, FRAME_HEIGHT, COLOUR_DEPTH, FRAME_RESIZABLE, USE_OPENGL, FRAME_FULLSCREEN, USE_HARDWARE);
            Video.WindowCaption = "Shmup";
            Video.WindowIcon(new Icon(@"images/shmup.ico"));
                
            // invoke application initialisation subroutine
            setup();

            // invoke secondary initialisation subroutine
            onInit();

            // register event handler subroutines
            Events.Tick += new EventHandler<TickEventArgs>(onTick);
            Events.Quit += new EventHandler<QuitEventArgs>(onQuit);
            Events.KeyboardDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(onKeyboard);
            Events.KeyboardUp += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(onKeyboard);
            Events.MouseButtonDown += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(onMouseButton);
            Events.MouseButtonUp += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(onMouseButton);
            Events.MouseMotion += new EventHandler<SdlDotNet.Input.MouseMotionEventArgs>(onMouseMove);

            // while not quit do process events
            Events.TargetFps = 60;
            Events.Run();
        }

        // This procedure is called after the video has been initialised but before any events have been processed.
        static void setup() {

            // Load Art
            sprites = new Surface(ss.getImagefile());

            Map = new Surface(@"images/floor.png").Convert(video, true, false);

            backgroundColour = sprites.GetPixel(new Point(200, 200));
            background = video.CreateCompatibleSurface();
            background.Transparent = false;
            background.Fill(backgroundColour);
        }

        // This procedure is called when the event loop receives an exit event (window close button is pressed)
        static void onQuit(object sender, QuitEventArgs args) {
            Events.QuitApplication();
        }

        // -- DATA --
        static int WorldX = 0;
        static int WorldY = 0;
        static int MovementX = 0;
        static int MovementY = 0;
        const int FRAME_WIDTH = 800;
        const int FRAME_HEIGHT = 600;
        const int COLOUR_DEPTH = 32;
        const bool FRAME_RESIZABLE = false;
        const bool FRAME_FULLSCREEN = false;
        const bool USE_OPENGL = false;
        const bool USE_HARDWARE = true;

        static Surface video;  // the window on the display

        static Surface Map;
        static Surface sprites;
        static Surface background;

        static Color backgroundColour;
    }
}

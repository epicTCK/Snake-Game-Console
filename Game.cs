using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.IO;

class Game {
	//TODO add more comments
	//declare variables
	static int speed = 150;
	static Char space = ' ';
	static ConsoleKey input;
	static int y = 6;
	static int x = 6;
	static int moves = 0;
	static bool trim = true;
	static bool beatHigh = false;
	static int foody = 0;
	static int foodx = 0;
	static bool food = false;
	static string direction;
	static int length = 1;
	static List < int > historyX = new List < int > ();
	static List < int > historyY = new List < int > ();
	static Char[] line1 = new Char[50];
	static Char[] line2 = new Char[50];
	static Char[] line3 = new Char[50];
	static Char[] line4 = new Char[50];
	static Char[] line5 = new Char[50];
	static Char[] line6 = new Char[50];
	static Char[] line7 = new Char[50];
	static Char[] line8 = new Char[50];
	static Char[] line9 = new Char[50];
	static Char[] line10 = new Char[50];
	//TODO somehow add 50 rows
	static Char[][] lines = {
		line1, line2, line3, line4, line5, line6, line7, line8, line9, line10
	};

	public static void Main() {
		Menu("main");

		for (int h = 0; h < 10; h++) {
			for (int k = 0; k < 50; k++) {
				lines[h][k] = space;
			}
		}
		lines[y][x] = '@';
		Thread t = new Thread(new ThreadStart(processInput));
		t.Start();
		Thread m = new Thread(new ThreadStart(LoopUpMove));
		m.Start();
		Thread f = new Thread(new ThreadStart(LoopDownMove));
		f.Start();
		Thread b = new Thread(new ThreadStart(LoopLeftMove));
		b.Start();
		Thread c = new Thread(new ThreadStart(LoopRightMove));
		c.Start();

		while (true) {
			input = Console.ReadKey().Key;
			Console.Clear();
			Display();
			if (input == ConsoleKey.Escape) {
				System.Environment.Exit(1);
			}
		}



	}
	// method to display
	public static void Display() {

		string line; // variable to hold the current line
		Console.Clear(); // clear the console to replace with new display

		if (x == foodx && y == foody) {
			Console.Beep();
			length++;
			food = false;

		}



		for (int _h = 0; _h < 10; _h++) {
			for (int _k = 0; _k < 50; _k++) {
				lines[_h][_k] = space;
			}
		}










		historyY.Insert(0, y);
		historyX.Insert(0, x);


		for (int df = 0; df < length; df++) {
			lines[historyY[df]][historyX[df]] = '█';
		}

		lines[y][x] = '@';
		lines[foody][foodx] = 'O';
		Console.WriteLine(length);
		for (int i = 0; i < 10; i++) { // loop lines
			line = lines[i][0].ToString();
			for (int j = 0; j < 50; j++) { // loop arrays within lines
				if (j != 0) {
					line += lines[i][j].ToString(); //set the variable line to 
				}
			}
			Console.WriteLine(line);
		}
	}
	//process the input
	private static void processInput() {
		Console.Clear();
		Display();


		while (true) {

			Random rnd = new Random();


			if (food == false) {
				foody = rnd.Next(0, 9);
				foodx = rnd.Next(0, 49);
				lines[foody][foodx] = 'O';
				food = true;
			}





			if (input == ConsoleKey.UpArrow && direction != "down") {
				direction = "up";
			}

			if (input == ConsoleKey.RightArrow && direction != "left") {
				direction = "right";
			}
			if (input == ConsoleKey.LeftArrow && direction != "right") {
				direction = "left";
			}
			if (input == ConsoleKey.DownArrow && direction != "up") {
				direction = "down";
			}





		}




	}
	private static void LoopUpMove() {

		while (true) {
			if (direction == "up") {


				y--;
				if (y < 0) {
					y = 9;
				}
				if (lines[y][x] == '█') {
					Menu("dead");
				}
				lines[y][x] = '█';
				Console.Clear();
				Display();


			}
			Thread.Sleep(speed);




		}
	}
	private static void LoopDownMove() {
		while (true) {
			if (direction == "down") {


				y++;
				if (y > 9) {
					y = 0;
				}
				if (lines[y][x] == '█') {
					Menu("dead");
				}
				lines[y][x] = '█';
				Console.Clear();
				Display();


			}
			Thread.Sleep(speed);
		}


	}
	private static void LoopLeftMove() {
		while (true) {
			if (direction == "left") {


				x--;
				if (x < 0) {
					x = 49;
				}
				if (lines[y][x] == '█') {
					Menu("dead");
				}
				lines[y][x] = '█';
				Console.Clear();
				Display();


			}
			Thread.Sleep(speed);
		}






	}
	private static void LoopRightMove() {
		while (true) {
			if (direction == "right") {



				x++;
				if (x > 49) {
					x = 0;
				}
				if (lines[y][x] == '█') {
					Menu("dead");
				}
				lines[y][x] = '█';
				Console.Clear();
				Display();


			}
			Thread.Sleep(speed);
		}
	}

	public static void Menu(string opt) {


		if (opt == "dead") {
			Console.Clear();
			Console.WriteLine("Game over!");
			Console.WriteLine("Score: " + length);

			int high = getHigh();
			if (high < length && getHigh() != 6667) {
				setHigh(length);
				beatHigh = true;
			}
			if (getHigh() == 6667) {
				Console.WriteLine("Error: File Not Found");
			} else {

				Console.WriteLine("High Score: " + getHigh());
				if (beatHigh) {
					Console.WriteLine("Congrats! You Beat Your High Score!!");
				}
			}
			Console.ReadLine();





			System.Environment.Exit(1);
		}


		if (opt == "main") {
			Console.Clear();
			Console.WriteLine("	███████╗███╗   ██╗ █████╗ ██╗  ██╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗");
			Console.WriteLine("	██╔════╝████╗  ██║██╔══██╗██║ ██╔╝██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝");
			Console.WriteLine("	███████╗██╔██╗ ██║███████║█████╔╝ █████╗      ██║  ███╗███████║██╔████╔██║█████╗  ");
			Console.WriteLine("	╚════██║██║╚██╗██║██╔══██║██╔═██╗ ██╔══╝      ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  ");
			Console.WriteLine("	███████║██║ ╚████║██║  ██║██║  ██╗███████╗    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗");
			Console.WriteLine("	╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝");
			Console.WriteLine("				   ╔══════════════════════════════╗");
			Console.WriteLine("				   ║           >Play Game         ║");
			Console.WriteLine("				   ║           High Score         ║");
			Console.WriteLine("				   ║  Made by Caleb Gentry 2015   ║");
			Console.WriteLine("				   ╚══════════════════════════════╝");
			ConsoleKey ChooseMain = Console.ReadKey().Key;
			if (ChooseMain == ConsoleKey.Enter) {
				return;
			}
			if (ChooseMain == ConsoleKey.DownArrow) {
				Menu("main2");
			} else {
				Menu("main");
			}

		}
		if (opt == "main2") {
			Console.Clear();
			Console.WriteLine("	███████╗███╗   ██╗ █████╗ ██╗  ██╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗");
			Console.WriteLine("	██╔════╝████╗  ██║██╔══██╗██║ ██╔╝██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝");
			Console.WriteLine("	███████╗██╔██╗ ██║███████║█████╔╝ █████╗      ██║  ███╗███████║██╔████╔██║█████╗  ");
			Console.WriteLine("	╚════██║██║╚██╗██║██╔══██║██╔═██╗ ██╔══╝      ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  ");
			Console.WriteLine("	███████║██║ ╚████║██║  ██║██║  ██╗███████╗    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗");
			Console.WriteLine("	╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝");
			Console.WriteLine("				   ╔══════════════════════════════╗");
			Console.WriteLine("				   ║            Play Game         ║");
			Console.WriteLine("				   ║          >High Score         ║");
			Console.WriteLine("				   ║  Made by Caleb Gentry 2015   ║");
			Console.WriteLine("				   ╚══════════════════════════════╝");
			ConsoleKey ChooseMain2 = Console.ReadKey().Key;
			if (ChooseMain2 == ConsoleKey.UpArrow) {
				Menu("main");
			}
			if (ChooseMain2 == ConsoleKey.Enter) {
				Console.Clear();
				if (getHigh() != 6667) {
					Console.WriteLine("High Score: " + getHigh());
					Console.WriteLine("Press enter to return");

				} else {
					Console.WriteLine("File Not Found");
					Console.WriteLine("Press enter to return");
				}
				Console.ReadLine();
				Menu("main");
			} else {
				Menu("main2");
			}

		}
	}
	static public int getHigh() {
		try {
			StreamReader HighScoreRead = new StreamReader("HighScore");
			int highScore = int.Parse(HighScoreRead.ReadLine());

			HighScoreRead.Close();
			return highScore;
		} catch (FileNotFoundException) {
			try {
				StreamReader HighScoreRead = new StreamReader("HighScore.txt");
				int highScore = int.Parse(HighScoreRead.ReadLine());

				HighScoreRead.Close();
				return highScore;
			} catch (FileNotFoundException) {

				return 6667;
			}
		}


	}
	static public void setHigh(int high) {
		string tHigh = high.ToString();
		System.IO.File.WriteAllText(@
		"HighScore", tHigh);



	}

}

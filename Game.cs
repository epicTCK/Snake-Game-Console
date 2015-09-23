using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using MyFingers.ToTypeThis;


//TODO: add power-ups

//Made by........
/***
 *                        $$\        $$$$$$$$\  $$$$$$\  $$\   $$\       
 *                        \__|       \__$$  __|$$  __$$\ $$ | $$  |      
 *     $$$$$$\   $$$$$$\  $$\  $$$$$$$\ $$ |   $$ /  \__|$$ |$$  /       
 *    $$  __$$\ $$  __$$\ $$ |$$  _____|$$ |   $$ |      $$$$$  /        
 *    $$$$$$$$ |$$ /  $$ |$$ |$$ /      $$ |   $$ |      $$  $$<         
 *    $$   ____|$$ |  $$ |$$ |$$ |      $$ |   $$ |  $$\ $$ |\$$\        
 *    \$$$$$$$\ $$$$$$$  |$$ |\$$$$$$$\ $$ |   \$$$$$$  |$$ | \$$\       
 *     \_______|$$  ____/ \__| \_______|\__|    \______/ \__|  \__|      
 *              $$ |                                                     
 *              $$ |                                                     
 *              \__|                                                     
 */



class Game {
	//TODO add more comments
	//declare all the  variables
	
	static int bonusScore = 0;// implemented in later versions...

	static int speed = 150; // higher number, slower speed150
	static Char space = ' '; // unneeded?
	static ConsoleKey input;

	// starting x and y positions
	static int y = 1;
	static int x = 1;

	// number of columns and rows. Not implemented yet
	static int columns = 50;
	static int rows = 30;

	// x and y of food. Randomized.
	static int foody = 0;
	static int foodx = 0;

	// if we are dead, do not update display. Rather, break all loops and display end screen
	static bool dead = false;


	static string direction;

	// starting length. Should it be higher?
	static int length = 1;


	//records the last few moves. Used for the body of the snake
	static List < int > historyX = new List < int > ();
	static List < int > historyY = new List < int > ();

	//Hard Mode
	static bool hardMode = false;

	// the most important variable: the array of arrays that holds the positions of the items.
	static Char[][] lines = new Char[rows][];


	public static void Main() {
		

		// get me a start menu menu
		Menu("main");

		// Initialize arrays
		for (int sg = 0; sg < rows; sg++) {
			lines[sg] = new Char[columns];
			for (int sm = 0; sm < columns; sm++) {
				lines[sg][sm] = ' ';
			}
		}

		// What do I look like?
		lines[y][x] = '@';

		// add threads.
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

		// input loop.
		while (true) {
			input = Console.ReadKey().Key;
			Console.Clear();
			Display();
			if (input == ConsoleKey.Escape) {

				Menu("dead");
			}
		}



	}

	// method to display
	public static void Display() {
		
		if (dead) {
			return;
		}

		Console.Clear(); // clear the console to replace with new display

		if (x == foodx && y == foody) { // Did I eat Food??
			Console.Beep();
			Random rnd = new Random();
			while(true){
			foody = rnd.Next(0, rows - 1);
			foodx = rnd.Next(0, columns - 1);
			if (x != foodx && y != foody){
				break;
			}
			}
			lines[foody][foodx] = 'O';
				
			

			if(speed > 80){
			if(hardMode){//increment speed when fed
				speed-=2;
			}
			else{
				speed -=1;
			}
			}
			length++;
			

		}

		// this loop makes the lines[][] array blank

		for (int _h = 0; _h < rows; _h++) {
			for (int _k = 0; _k < columns; _k++) {
				lines[_h][_k] = space;
			}
		}



		// add my previous position to history lists

		historyY.Insert(0, y);
		historyX.Insert(0, x);

		// add the body of the snake by using *length* previous locations
		for (int df = 0; df < length; df++) {
			lines[historyY[df]][historyX[df]] = '█';
		}

		// Hey look! I look like "at"!!!


		lines[y][x] = '@';

		// man the food looks boring. TODO: Change it to something else!!
		lines[foody][foodx] = 'O';

		// how big am I??
		Console.WriteLine("Score: " + length);



		// make top  border
		string top = "┌";
		if (hardMode) {
			top = "╔";
		}
		for (int p = 0; p < columns; p++) {
			if (hardMode == true) {
				top += "═";

			} else {
				top += "─";
			}

		}
		if (!hardMode) {
			top += "┐";
		} else {
			top += "╗";
		}

		Console.WriteLine(top);


		//bottom border 
		string bottom = "└";
		if (hardMode) {
			bottom = "╚";
		}
		for (int p = 0; p < columns; p++) {
			if (hardMode == true) {
				bottom += "═";

			} else {
				bottom += "─";
			}

		}
		if (!hardMode) {
			bottom += "┘";
		} else {
			bottom += "╝";
		}

		//and finally, the original purpose of the method, DISPLAY STUFF!!
		string line = "│"; // variable to hold the current line

		for (int i = 0; i < rows; i++) { // loop lines

			if (hardMode) {
				line = "║";
			} else {
				line = "│";
			}

			for (int j = 0; j < columns; j++) { // loop arrays within lines

				line += lines[i][j].ToString(); //set the variable line to 

			}
			if (hardMode) {
				line += "║";
			} else {
				line += "│";
			}
			Console.WriteLine(line); // the actual displaying is done here very boring.
		}
		Console.WriteLine(bottom);
		
	

	}

	//process the input. Add this to main method???
	//TODO add more comments here
	private static void processInput() {
		Console.Clear();
		Display();



		while (true) {

			

			// get the input and process it. 


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




	} // and the rest of these are self explanatory pretty much
	private static void LoopUpMove() {

		while (dead == false) {
			if (direction == "up") {


				y--;
				if (y < 0) {

					y = rows - 1; // am I at the edge??
					if (hardMode) {
						Menu("dead");

					}
				}
				if (lines[y][x] == '█') {
					Menu("dead"); // Am I dead??
				}
				lines[y][x] = '█'; // This 
				Console.Clear();
				Display();


			}
			Thread.Sleep(speed);




		}
	}
	private static void LoopDownMove() {
		while (dead == false) {
			if (direction == "down") {


				y++;
				if (y > rows - 1) {

					y = 0;
					if (hardMode) {
						Menu("dead");

					}
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
		while (dead == false) {
			if (direction == "left") {


				x--;
				if (x < 0) {

					x = columns - 1;
					if (hardMode) {
						Menu("dead");

					}
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
		while (dead == false) {
			if (direction == "right") {



				x++;
				if (x > columns - 1) {

					x = 0;
					if (hardMode) {
						Menu("dead");

					}
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




	// menu method. Would it be overkill/useless if I used a class
	// TODO add more comments in this method.
	public static void Menu(string opt) {
		Tuple<int, bool> highScoreTuple = getHigh();
			bool allClear = highScoreTuple.Item2;
			int highScore = highScoreTuple.Item1;
		int fullScore = length + bonusScore;
		// Game Over Menu. This is extremely buggy.
		if (opt == "dead") {
			
			Console.Beep();
			
			Console.ForegroundColor = ConsoleColor.Red;
			dead = true; // tell the rest of the program to stop.

			Console.Clear();

			Console.WriteLine("	╔═╗┌─┐┌┬┐┌─┐  ╔═╗┬  ┬┌─┐┬─┐");
			Console.WriteLine("	║ ╦├─┤│││├┤   ║ ║└┐┌┘├┤ ├┬┘");
			Console.WriteLine("	╚═╝┴ ┴┴ ┴└─┘  ╚═╝ └┘ └─┘┴└─");
			Console.WriteLine("	         Score: " + fullScore);
			
			if(!allClear){
				Console.WriteLine("error, high score file not found");
			}
			else if(highScore < fullScore){
				setHigh(length + bonusScore);
				Console.WriteLine("High Score: " + fullScore);
				Console.WriteLine("Great Job! You beat your high score!");
			}
			else{
				Console.WriteLine("High score: " + highScore);
			}
			Console.ReadLine();
			Console.ResetColor();
			Console.Clear();
			
			
			//TODO add 'play again?' prompt
			System.Environment.Exit(1); // GoooooodBYEEEEE
		}

		// and the main menu....
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
				Console.Clear();
				Console.Write("Hard mode? (y/n)");
				string mode = Console.ReadLine();
				if(mode.ToUpper() == "Y"){
					hardMode = true;
				}
				else{
					hardMode = false;
				}
				Console.Clear();
				bool exit = false;
			do{
				Console.WriteLine("Mapsize large, medium, or small?(L, M, or S)");
				string mapSize  = Console.ReadLine();
				switch(mapSize.ToUpper()){
				case "L":
					columns = 50;
					rows = 30;
					exit = true;
				break;
				case"M":
					exit = true;
					columns = 35;
					rows = 20;
				break;
				case"S":
					exit = true;
					columns = 20;
					rows = 10;
				break;
				
				}
				
				
			}while(!exit);
			return;
		}
			if (ChooseMain == ConsoleKey.DownArrow) {
				Menu("main2");
			} else {
				Menu("main");
			}

		}
		// and the main menu with the cursor down one...
		if (opt == "main2") {
			Console.Clear();
			//use newline next time :)
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
			// here comes a jumble of unintelligible code... Sorry!!
			if (ChooseMain2 == ConsoleKey.UpArrow) {
				Menu("main");
			}
			if (ChooseMain2 == ConsoleKey.Enter) {
				// display the high score. I want to make this a little better than just "high score: blab...."
				Console.Clear();
				if (allClear) {
					Console.WriteLine("High Score: " + highScore);
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
	// get the high score form HighScore.txt. 
	static public Tuple<int, bool> getHigh() {
		bool allClear = true;
		string highScore;
		try {

			StreamReader HighScoreRead = new StreamReader("HighScore");
			highScore = HighScoreRead.ReadLine();
			HighScoreRead.Close();
			
		} catch (FileNotFoundException) { //poop the program can't find it...
			try {
				StreamReader HighScoreRead = new StreamReader("HighScore.txt"); // Horray!!! Now it Works!!!
				highScore = HighScoreRead.ReadLine();
				HighScoreRead.Close();
			} catch (FileNotFoundException) { // well, lets not crash.
				allClear = false;
				highScore = "0";	
			}
		}
		int highScoreInt = int.Parse(highScore);

		
		Tuple<int, bool> returner = new Tuple<int, bool>(highScoreInt, allClear);
		return returner;


	}
	static public void setHigh(int high) {
		string tHigh = high.ToString(); //TODO: error handling??
		System.IO.File.WriteAllText("HighScore", tHigh);
	}

}

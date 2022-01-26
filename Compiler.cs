using System;
using System.IO;

namespace compile
{
	/// <summary>
	/// This Class responsible for checking the correctness of what written 
	/// either in the editor or in the file
	/// </summary>
	class Compiler
	{
		/// <summary>
		/// The place of error
		/// </summary>
		public static int error=0;
		/// <summary>
		/// The error message
		/// </summary>
		public static string msg="";

		/// <summary>
		/// Check if this token is number or not
		/// </summary>
		/// <param name="c">The token to be checked</param>
		/// <returns>The result true or false</returns>
		private static bool isNumber(string s)
		{
			try
			{
				float.Parse(s);
				return true;
			}
			catch(Exception exc)
			{
				exc.ToString();
				return false;
			}
		}


		/// <summary>
		/// Check if this token is integer or not
		/// </summary>
		/// <param name="c">The token to be checked</param>
		/// <returns>The result true or false</returns>
		private static bool isInt(string s)
		{
			try
			{
				Int32.Parse(s);
				return true;
			}
			catch(FormatException exc)
			{
				exc.ToString();
				return false;
			}
		}


		/// <summary>
		/// This function to remove the spaces, tabs, and unnecessary new lines
		/// between the token, knowing that we need only one new line between tokens
		/// </summary>
		/// <param name="s">The unformated string</param>
		/// <returns>The formated string</returns>
		public static string removeSpaces(string s)
		{
			string temp = "";
			for(int i=0; i<s.Length; i++)
			{				
				switch (s[i])
				{
					case ' ' : 
					case '\t': break;
					case '\n': 
					{
						if (s[i-2] != '\n')
							temp = temp+s[i]; 
						break;
					}
					case '\r':
					{
						if (s[i-2] != '\r')
							temp = temp+s[i];
						break;
					}
					default: 
					{
						temp = temp+s[i];
						break;
					}
				}
			}
			return temp;
		}


		/// <summary>
		/// This function parse the string into token
		/// Which mean that number, comma, carriage return, or newline will be as an 
		/// element in the new 2 dim. array
		/// </summary>
		/// <param name="s">The unparsed string</param>
		/// <returns>The parsed string</returns>
		private static string[] parse(string s)
		{
			// make string to hold the token
			string temp = "";

			// make two dim. array to hold the tokens
			string []str = new string [s.Length];

			// make counter to hold the index of the string in the 2 dim. array
			int count = 0;

			// to move on the given string
			for(int i=0; i<s.Length; i++)
			{
				/*
				 *  if it not found comma, carriage return, or newline the character will 
				 *  be concatenated with the temp
				 */
				
				if((s[i] != ',') && (s[i] != '\r'))
					temp = temp+s[i];
				
					// else
				else
				{
					/*
					 *  it found the comma, carriage return, or newline then it will add 
					 * the temp to the 2 dim array and increment the index and then add 
					 * the newline or the comma to the next place in the array*/

					if(temp != "")
					{
						str[count] = temp;
						count++;
						temp = "";
					}
					/*
					 *  add this char to the array as a token 
					 *  knowing that this char is not a number
					 */
					if(s[i] == '\r')
					{
						str[count] = s[i].ToString() + "\n";
						i++;
					}
					else
						str[count] = s[i].ToString();
					count++;
				}
			}
			// This part remove the unused part in the array
			
			int length=0;
			// get the length of the used part
			for(int i=0; i<str.Length; i++)
			{				
				if(str[i] != null)
					length++;
				else
					break;
			}

			// make array with the appreciate index
			string []returnString = new string[length];

			// copy the first array to the appreciate array
			for(int i=0; i<length; i++)
				returnString[i] = str[i];

			// return the appreciate array
			return returnString;
		}


		/// <summary>
		/// This function to analysis the syntax to find syntax error such that we can't 
		/// allow two sequential commas(,,) or the token is somthing else than number
		/// </summary>
		/// <param name="s">The string to be checked for syntax analysis</param>
		/// <returns>The place of error if exist otherwise it return ZERO</returns>
		private static int syntaxAnalysis(string []s)
		{	
			if(s.Length == 8)
			{
				error = 8+1;
				msg = "The trainning data is empty\n You must enter a trainning data";
				return error;
			}

			if(isNumber(s[0]) == false)
			{
				error = 0+1;
				msg="The first element must be a number";
				return 0+1;
			}
			for(int i=1; i<s.Length; i++)
			{
				switch(s[i])
				{
					case "\r\n" :break;
					case ","  :
					{
						if(isNumber(s[i-1]) == false)
						{
							error = i+1;
							msg = "A comma is not Preceded by a number";
							return i+1;
						}
						break;
					}					
					default:
					{
						if(isNumber(s[i]) == false)
						{
							error = i+1;
							msg = "Number is only allowed here";
							return i+1;
						}
						else
							break;
					}
				}
			}
			return 0;
		}

		/// <summary>
		/// This function analysis the given string for symantec errors such that we can't 
		/// allow the written number to exceed the summation of the input and output values
		/// It also check for the first line format to be like that(2,2,2,2)
		/// the numbers by order stand for Number of layers, input neurons, output neurons, 
		/// hidden neurons and afterwards there is newline
		/// </summary>
		/// <param name="s">The string to be checked for symantec analysis</param>
		/// <returns>The place of error if exist otherwise it return ZERO</returns>
		private static int SymantecAnalysis(string []s)
		{
			// check for the first line if there is \n or non integer in between 
			for(int i=0; i<7; i++)
				if ( s[i] != "," && isInt(s[i]) == false) 
				{
					error = i+1;
					msg = "The configuration input of the network must be in one line and must contain 4 integers only";
					return error;
				}

			// check for the end of the line to be \n
			if(s[7] != "\r\n")
			{
				error = 7+1;
				msg = "The line must finish here";
				return error;
			}
			
				// else
			else
			{
				// get input and the output 
				int input  = Int32.Parse(s[2]);
				int output = Int32.Parse(s[4]);
                
				// start stand for the beginning of the line
				int start = 8;
				
				// check for the rest of the string as we check for the first line above
				while(start < s.Length)
				{
					// compute the length of the line
					int length = (input+output)*2+start-1;
					
					// check for the line if there is \n in between
					for(int i=start; i<length; i++)
						if(s[i] == "\r\n")
						{
							error = i+1;
							msg = "The input and output data for a time must be in one line";
							return error;
						}
					
					// check for the end of the line to be \n
					if(s[length]!= "\r\n")
					{
						error = length+1;
						msg = "The line must end here";
						return error;
					}
					
					// go to the next line
					start = start+(input+output)*2;
				}
			}
			return 0;
		}


		/// <summary>
		/// To fill the correct data into the 2 dim. array
		/// </summary>
		/// <param name="s">The compiled string</param>
		/// <returns>The 2 dim. array</returns>
		private static double[,] run(string []s)
		{

			// get input and the output 
			int input  = Int32.Parse(s[2]);
			int output = Int32.Parse(s[4]);

			// want to know number of lines
			int height = (s.Length-8)/((input+output)*2);
            
			// reserve the array
			double [,]trainData = new double[height,input+output];

			
			/*
			 * fill the array  
			 * the index of i stand for the row in the two dimension array
			 * the index of j stand for the column in the two dimension array 
			 * the index of k stand for the number in the string given and it is 
			 * incrememnted by two because each two consequence number is seperated 
			 * by a comma, we take only the number
			 */
			for(int i=0,k = 8; i<height; i++)
				for(int j=0; j<input+output; j++,k=k+2)
					trainData[i,j] = double.Parse(s[k]);
			
			return trainData;
		}
		

		/// <summary>
		/// This function for compiling the given array
		/// </summary>
		/// <param name="s">the uncompiled string</param>
		public static double[,] compile(string s)
		{
			// remove spaces
			s = removeSpaces(s);

			// format string
			string []parsedString = parse(s);
			
			// search for syntax error
			int errorPostion = syntaxAnalysis(parsedString);
			
			// if there is no error
			if (errorPostion == 0)

				// search for symantec error
				errorPostion = SymantecAnalysis(parsedString);

			// if there is no error fill data in the trainning array
			if (errorPostion == 0)
				return run(parsedString);
			return null;
		}
		

		/// <summary>
		/// This function for compiling the given array
		/// </summary>
		/// <param name="s">the uncompiled file</param>		
		public static double[,]compile(FileInfo fInfo)
		{

            StreamReader s = new StreamReader(@fInfo.DirectoryName+"\\"+fInfo.Name);
			string str="",temp="";
			temp = s.ReadLine();
			while(temp != null)
			{
				str = str + temp + "\r\n";
				temp = s.ReadLine();
			}
			s.Close();
			return compile(str);
		}
	}
}
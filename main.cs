using System;
using System.Threading;
using System.IO;
using compile;

namespace console_neuralnetwork
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// return the array form the file
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static double[,] Array_from_file(string path)
		{
			StreamReader file =  new StreamReader(@path);
			FileInfo fInfo = new FileInfo(@path);
			
			double [,]data;
			string str = file.ReadLine();
			data = Compiler.compile(fInfo);
			return data;
		}



		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			
			double [,]d_patterns ;
			float  [,]f_patterns;
			float  []f_one_pattern;

			d_patterns = Class1.Array_from_file("training.txt");

			f_patterns = new float[3,38];
			f_one_pattern = new float[38];

			// this two loops to get array from file "pattern" and put it in float array
			for(int x=0 ; x < d_patterns.Length/38 ; x++)
				for(int y=0 ; y <= 37 ; y++)
					f_patterns[x,y] = (float)d_patterns[x,y];	

			
			Neural_Networks.Nnetwork network = new Neural_Networks.Nnetwork(35,5,3);
			network.FirstTimeSettings();
			// main loop now the training loop
			for(int counter=0 ; counter<1000 ; counter++)
			{
				for(int i=0 ; i<3 ; i++)
				{
					for(int x=0 ; x <= 37 ; x++)
					{
						f_one_pattern[x] = f_patterns[i,x];
						Console.Write(f_one_pattern[x].ToString() + " " );
					}
					network.BeforeTraining(f_one_pattern);
					network.Training_for_one_pattern();
					Console.Write(" "+counter.ToString());
					Console.WriteLine();
				}
			}

			network.BeforeTraining(1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,1);
            network.Test_Drive();                                                                                   



			#region try this later , this one for the error calculation
		/*	float avgerr_per_pattern;
			int total_cycles = 0;
			int total_pattern = 0;

			int error_last_cycle = 0;
			int patterns_per_cycle = 0;
			while(avgerr_per_pattern > 0.1)
			{
				error_last_cycle = 0;
				patterns_per_cycle = 0;
				// for loop here to see all the patterns in the vector
				network.BeforeTraining(1.1f,1.1f,1.1f,1.1f,1.1f,1.1f);
				total_pattern++;
				patterns_per_cycle++;
				network.Training_for_one_pattern();
				error_last_cycle += sqr(network.Calc_total_error_in_pattern());
				// end for
				network.reset_total_error();
				avgerr_per_pattern = sqrt(error_last_cycle) / patterns_per_cycle;
			    
			}*/
			#endregion
		}
	}
}

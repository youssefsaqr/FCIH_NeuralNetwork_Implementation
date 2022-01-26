using System;

namespace Neural_Networks
{
	/// <summary>
	/// Summary description for Nnetwork.
	/// </summary>
	public class Nnetwork
	{
		private int i;
		private int o;
		private int h;

		private float LH=0.15f; // learning parameter of the hidden
		private float LO=0.2f; // learning parameter of the output
		
		private float []inputsToNetwork;// array of float to take training data "inputs"
		private float []desiredOutputs;// array of float to take training data "desired outputs"

		private node []input; // array of input neurons
		private node []output;// array of output neurons
		private node []hidden;// array of hidden units neurons

		private float total_error;
		public float error_compared_to_tolerance=0;
		
		/// <summary>
		/// this function to calculate the sigmoid of a float
		/// </summary>
		/// <param name="x">float number</param>
		/// <returns></returns>
		private float sigmoid(float x)
		{
			return 1/(1+(float)Math.Exp(-x));
		}


		#region these functions must be called before callind training

		/// <summary>
		/// Constructor of the neural network
		/// </summary>
		/// <param name="inp">integer : number of input neurons</param>
		/// <param name="hide">integer : number of hidden units neurons</param>
		/// <param name="outp">integer : number of output neurons</param>
		public Nnetwork(int inp , int hide , int outp )
		{
			i = inp ; o = outp ; h = hide;
			input = new node[i];
			output = new node[o];
			hidden = new node[h];
			Random rand = new Random(unchecked((int)DateTime.Now.Ticks)); 

			
			for(int x=0 ; x<inp ; x++)
			{
				input[x] = new node();
				input[x].weights = new float[this.h];
				for(int j=0 ; j<hide ; j++)// the loop for initializing the weights
				{
					input[x].weights[j] = (float) rand.NextDouble();
				}

			}
			
			for(int y=0; y<hide ; y++)
			{
				hidden[y] = new node();
				hidden[y].weights = new float[this.o];
				for(int j=0 ; j<outp ; j++)// the loop for initializing the weights
					hidden[y].weights[j] = (float)rand.NextDouble();
			}
			
			for(int z=0 ; z<outp ; z++)
			{
				output[z] = new node();
			}
		}
		

		/// <summary>
		/// This function to set the threshold of the neruons in the hidden layer and the output layer
		/// </summary>
		
		public void FirstTimeSettings()
		{
			Random x = new Random(unchecked((int)DateTime.Now.Ticks)); 
			for(int i=0 ; i<h ; i++)
				hidden[i].Threshold = (float)x.NextDouble();
			for(int i=0 ; i<o ; i++)
				output[i].Threshold = (float)x.NextDouble();
		}
		

		/// <summary>
		/// This function passes training data to the network , inputs and desired ouptus
		/// </summary>
		/// <param name="list"></param>
		public void BeforeTraining(params float []list)
		{
			int counter=0;
			int j=0;
			int k=0;
			inputsToNetwork = new float[this.i];
			desiredOutputs = new float[this.o];

			while(counter < list.Length)
			{
				if( counter < this.i)
				{
					inputsToNetwork[j] = list [counter];
					j++;
				}
				else
				{
					desiredOutputs[k] = list[counter];
					k++;
				}
				counter++;
			}

		}

		
#endregion

		/// <summary>
		/// a function to calculate the activation of the hidden layer and the output layer
		/// </summary>
		private void Calc_Activation()
		{
			// a loop to set the activations of the hidden layer
			int ch=0;
			while(ch<this.h)
			{
				for(int ci=0 ; ci<this.i ; ci++)
					hidden[ch].Activation += inputsToNetwork[ci] * input[ci].weights[ch];

				ch++;
			}// end of the while

			// calcaulate the output of the hidden
			for(int x=0 ; x<this.h ; x++)
			{
				hidden[x].Activation += hidden[x].Threshold ;
				hidden[x].Activation = sigmoid(hidden[x].Activation );
			}

			// a loop to set the activations of the output layer
			int co=0;
			while(co<this.o)
			{
				for(int chi=0 ; chi<this.h ; chi++)
					output[co].Activation += hidden[chi].Activation  * hidden[chi].weights[co];

				co++;
			}// end of the while

			// calcaulate the output of the output layer
			for(int x=0 ; x<this.o ; x++)
			{
				output[x].Activation += output[x].Threshold ;
				output[x].Activation = sigmoid(output[x].Activation );
			}


		}

		
		/// <summary>
		/// print output of the network
		/// </summary>
		public void print_output()
		{
			// calcaulate the output of the output layer
			for(int x=0 ; x<this.o ; x++)
			{
				Console.Write(output[x].Activation.ToString()+ " ");
			}

		}


		/// <summary>
		/// a function to calculate the error of each output neuron
		/// </summary>
		private void Calc_error_output()
		{
			for(int x=0 ; x<this.o ; x++)
				output[x].error = output[x].Activation * (1 - output[x].Activation ) * (desiredOutputs[x] - output[x].Activation );
		}


		/// <summary>
		/// a function to calculate the error of each hidden neuron
		/// </summary>
		private void Calc_error_hidden()
		{
			int y=0;
			while(y<this.h)
			{
				for(int x=0 ; x<this.o ; x++)
				{
					hidden[y].error += hidden[y].weights[x] * output[x].error ; 
				}
				hidden[y].error *= hidden[y].Activation * (1 - hidden[y].Activation );
				y++;
			}

		}

		
		/// <summary>
		/// a function to calculate the new thresholds for each neuron
		/// </summary>
		private void Calc_new_Thresholds()
		{
			// computing the thresholds for next itration for hidden layer
			for(int x=0 ; x<this.h ; x++)
				hidden[x].Threshold +=  hidden[x].error * this.LH ;
			// computing the thresholds for next itration for output layer
			for(int y=0 ; y<this.o ; y++)
				output[y].Threshold += output[y].error * this.LO;
		}


		/// <summary>
		/// a function to calculate the new weights between hidden and output
		/// </summary>
		private void Calc_new_weights_in_hidden()
		{
			int x=0;
			float temp=0.0f;
			while(x < this.h)
			{
				temp = hidden[x].Activation * this.LO;
				for(int y=0 ; y<this.o ; y++)
				{
					hidden[x].weights[y] += temp * output[y].error;
				}
				x++;
			}
		}


		/// <summary>
		/// a function to calculate the new weights between input and hidden
		/// </summary>
		private void Calc_new_weights_in_input()
		{
			int x=0;
			float temp=0.0f;
			while(x < this.i)
			{
				temp = inputsToNetwork[x] * this.LH;
				for(int y=0 ; y<this.h ; y++)
				{
					input[x].weights[y] += temp * hidden[y].error;
				}
				x++;
			}

		}

		
		/// <summary>
		/// the function that returns the total error
		/// </summary>
		/// <returns></returns>
		public float Calc_total_error_in_pattern()
		{
			float temp=0.0f;
			for(int x=0 ; x<this.o ; x++)
				temp+=output[x].error;
			return this.total_error;
		}


		/// <summary>
		/// set total error = 0 after one cycle
		/// </summary>
		public void reset_total_error()
		{
			this.total_error = 0;
		}




		/// <summary>
		/// the function that trains the network
		/// </summary>
		/// 
		public void Training_for_one_pattern()
		{
			this.Calc_Activation();
			this.Calc_error_output();
			this.Calc_error_hidden();
			this.Calc_new_Thresholds();
			this.Calc_new_weights_in_hidden();
			this.Calc_new_weights_in_input();
		}


		/// <summary>
		/// test the network after training
		/// </summary>
		public void Test_Drive()
		{
			this.Calc_Activation();
			this.print_output();
		}
	}
}
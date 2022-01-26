using System;

namespace Neural_Networks
{
	/// <summary>
	/// This class descripes the neuron
	/// </summary>
	public class node
	{
		private float activation;
		private float threshold;
		public float []weights;
		public float error;

		public node()
		{
			this.activation = 0;
			this.error = 0;
		}
		
		public node(float act,float thr,int numOfweights)
		{
			this.activation = act;
			this.threshold = thr;
			this.weights = new float[numOfweights];
		}

		public float Activation
		{
			set	{activation = value ;}
			get {return activation;}
		}

		public float Threshold
		{
			set {threshold = value;}
			get {return threshold;}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour {
    public int population_size = 15 * 12 * 3;
    public double max_mutation_rate = 0.05;
    public double min_mutation_rate = 0.005;
    public double max_crossover_rate = 0.8;
    public double min_crossover_rate = 0.5;
    public int[,,] position = new int[16,13,4];
    public double[] height = new double [4]{ 0,0.145, 0.679, 1.1436 };
    public double V_of_shelf = ((0.6737639 - 0.1639082) * 3) * (0.362 * 2) * (1.677 * 5);
    public double[] weight = new double [4]{0, 1, 2, 3 };

    //crossover rate
    public double Mutation_rate(double A, double f_derivation, double f_avg, double f_max){
        if(f_derivation >= f_avg){
            double member = 0;
            double denominator = 0;

            denominator = max_crossover_rate - min_crossover_rate;

            double exp_element1 = 2 * (f_derivation - f_avg);
            double exp_element2 = f_max - f_avg;
            double exp = A * (exp_element1 / exp_element2);

            member = 1 + Mathf.Exp((float)exp);

            return (denominator / member + min_crossover_rate);
        }else{
            return max_crossover_rate;
        }
    }

    //mutation rate
    public double Crossover_rate(double A, double f, double f_avg, double f_max){
        if(f >= f_avg){
            double member = 0;
            double denominator = 0;

            denominator = max_mutation_rate - min_mutation_rate;

            double exp_element1 = 2 * (f - f_avg);
            double exp_element2 = f_max - f_avg;
            double exp = A * (exp_element1 / exp_element2);

            member = 1 + Mathf.Exp((float)exp);

            return (denominator / member + min_mutation_rate);
        }else{
            return max_mutation_rate;
        }
    }

    //Init array
    public void Init_situation(){
        for (int i = 1; i < 16; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                for (int k = 1; k < 4; k++)
                {
                    position[i, j, k] = 0;
                }
            }
        }
    }

    //weight_1:Efficiency proirity
    public double Distance(int a, int b, int c){
        double y = 0;
        if(b%2==1){
            b /= 2;
            y = (double)b * 3;
        }else{
            b /= 2 - 1;
            y = (double)b * 3 + 0.372;
        }

        double x = 0;
        if( a>=3){
            int n = a / 5;
            int m = a % 5 - 1;
            x = (m - 2) * 1.77 + n * 10.4;
        }else if (a==2){
            x = 1.77;
        }else{
            x = 2 * 1.77;
        }

        double z = height[c];

        return x+y+z;
    }

    //weight_2:Similar profucts
    public double Centre_distance(int x, int y, int z, int type){
        double centre_x = 0;
        double centre_y = 0;
        double centre_z = 0;
        int num = 0;

        for (int i = 1; i < 16; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                for (int k = 1; k < 4; k++)
                {
                    if(position[i, j, k] == type){
                        centre_x += i;
                        centre_y += j;
                        centre_z += k;
                        num++;
                    }
                }
            }
        }

        centre_x /= num;
        centre_y /= num;
        centre_z /= num;

        double decimals =0;
        decimals = centre_x - (int)centre_x;
        if(decimals>=0.5){
            centre_x = (int)centre_x + 1;
        }else{
            centre_x = (int)centre_x;
        }

        decimals = centre_y - (int)centre_y;
        if (decimals >= 0.5)
        {
            centre_y = (int)centre_y + 1;
        }else{
            centre_y = (int)centre_y;
        }

        decimals = centre_z - (int)centre_z;
        if (decimals >= 0.5)
        {
            centre_z = (int)centre_z + 1;
        }else{
            centre_z = (int)centre_z;
        }

        double centre_a = 0;
        double centre_b = 0;
        double centre_c = 0;

        double n = centre_x / 5;
        double m = centre_x % 5 - 1;
        centre_a = -16.36 + m * 1.677 + n * 10.4;
        if ((int)centre_y % 2 == 1){
            centre_y = (int)centre_y/2;
            centre_b = -3.187 + centre_y * 3;
        }else{
            centre_y = (int)(centre_y) / 2 - 1;
            centre_b = -3.187 + centre_y * 3 + 0.372;
        }

        centre_c = height[(int)centre_z];

        double a = 0;
        double b = 0;
        double c = 0;

        n = x / 5;
        m = x % 5 - 1;
        a = -16.36 + m * 1.677 + n * 10.4;

        if (y % 2 == 1)
        {
            y = y / 2;
            b = -3.187 + y * 3;
        }
        else
        {
            y = y / 2 - 1;
            b = -3.187 + y * 3 + 0.372;
        }

        c = height[z];


        double res = Mathf.Sqrt(Mathf.Pow((float)(a - centre_a), 2) + Mathf.Pow((float)(b - centre_b), 2) + Mathf.Pow((float)(c - centre_c), 2));

        return res;
    }

    //weight_3:Shelf stability
    public double Core_of_shelf(int x, int y, int z){
        double core_of_shelf = 0;

        double w_sum = 0;

        for (int i = 1; i < 16; i++){
            for (int j = 1; j < 13;j++){
                for (int k = 1; k < 4;k++){
                    if(position[i,j,k]!=0){
                        w_sum += weight[position[i, j, k]];
                    }
                }
            }
        }

        core_of_shelf = w_sum / V_of_shelf;

        return core_of_shelf;
    }

    //w_1 & w-2 & w_3 => Objective function
    public double Objective(int a, int b, int c){
        double w_1 = 0.6;
        double w_2 = 0.2;
        double w_3 = 0.2;

        double res = 0;

        //f = w_1 * f1 + w_2 * f2 + w_3 * f3

        return res;
    }

    //Fitness value function 1
    public double Fitness_value_1(){
        
        return 0;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

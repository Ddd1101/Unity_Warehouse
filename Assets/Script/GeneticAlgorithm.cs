using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour {
    public int population_size = 3 * 5 * 3 * 6;
    public double max_mutation_rate = 0.05;
    public double min_mutation_rate = 0.005;
    public double max_crossover_rate = 0.8;
    public double min_crossover_rate = 0.5;

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

    //

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

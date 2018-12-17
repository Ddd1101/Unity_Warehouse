using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public int population_size = 15 * 12 * 3;
    public double A;
    public double max_mutation_rate = 0.05;
    public double min_mutation_rate = 0.005;
    public double max_crossover_rate = 0.8;
    public double min_crossover_rate = 0.5;
    public double[] height = new double[4] { 0, 0.145, 0.679, 1.1436 };
    public double V_of_shelf = ((0.6737639 - 0.1639082) * 3) * (0.362 * 2) * (1.677 * 5) * 18;
    public double[] weight = new double[4] { 0, 1, 2, 3 };
    public double[] turnover = new double[4] { 0, 0.3, 0.5, 0.2 };
    public Random Random;
    private List<Individual> population;
    public Queue<Individual> offspring_list;
    public double f_max;
    public double f_avg;
    public double f_sum;
    public Individual parent1;
    public Individual parent2;
    public Best_result best_result;
    public int type;
    public int num;
    public int half_num;

    public GeneticAlgorithm(int type, int num)
    {
        Random = new Random();
        population = new List<Individual>();
        offspring_list = new Queue<Individual>();
        best_result = new Best_result();
        f_max = 0;
        f_avg = 0;
        f_sum = 0;
        A = 9.903;
        this.type = type;
        this.num = num;
        if (num % 2 == 1)
        {
            half_num = (num / 2) + 1;
        }
        else
        {
            half_num = num / 2;
        }
    }

    public void Print()
    {
        foreach (var item in population)
        {
            print(item.x + " " + item.y + " " + item.z + " " + item.value);
        }
        print("=================");
    }

    //Initialize population
    public void Initialize_population()
    {
        population.Clear();
        int ran_x = 0;
        int ran_y = 0;
        int ran_z = 0;
        bool flag = true;
        for (int h = 0; h < num; h++)
        {
            while (flag)
            {
                ran_x = Random.Range(1, 16);
                ran_y = Random.Range(1, 13);
                ran_z = Random.Range(1, 4);
                //ran_type = Random.Range(1, 4);
                if (Data.position[ran_x, ran_y, ran_z] == 0)
                {
                    Data.position[ran_x, ran_y, ran_z] = type;
                    //print(ran_x + " " + ran_y + " " + ran_z);
                    population.Add(new Individual(ran_x, ran_y, ran_z, type));
                    break;
                }
                else
                {
                    //print("X_ " + ran_x + " " + ran_y + " " + ran_z);
                }
            }
        }
        //print("=============");
    }

    //crossover rate
    public double Crossover_rate(double f_derivation, double f_avg, double f_max)
    {
        if (f_derivation >= f_avg)
        {
            double member = 0;
            double denominator = 0;

            denominator = max_crossover_rate - min_crossover_rate;

            double exp_element1 = 2 * (f_derivation - f_avg);
            double exp_element2 = f_max - f_avg;
            double exp = A * (exp_element1 / exp_element2);

            member = 1 + Mathf.Exp((float)exp);

            return ((denominator / member) + min_crossover_rate);
        }
        else
        {
            return max_crossover_rate;
        }
    }

    //mutation rate
    public double Mutation_rate(double f, double f_avg, double f_max)
    {
        if (f >= f_avg)
        {
            double member = 0;
            double denominator = 0;

            denominator = max_mutation_rate - min_mutation_rate;

            double exp_element1 = 2 * (f - f_avg);
            double exp_element2 = f_max - f_avg;
            double exp = A * (exp_element1 / exp_element2);

            member = 1 + Mathf.Exp((float)exp);

            return (denominator / member + min_mutation_rate);
        }
        else
        {
            return max_mutation_rate;
        }
    }

    //weight_1:Efficiency proirity
    public double Distance(int a, int b, int c)
    {
        double y = 0;
        if (b % 2 == 1)
        {
            b /= 2;
            y = (double)b * 3;
        }
        else
        {
            b /= 2 - 1;
            y = (double)b * 3 + 0.372;
        }

        double x = 0;
        if (a >= 3)
        {
            int n = a / 5;
            int m = a % 5 - 1;
            x = (m - 2) * 1.77 + n * 10.4;
        }
        else if (a == 2)
        {
            x = 1.77;
        }
        else
        {
            x = 2 * 1.77;
        }

        double z = height[c];

        return x + y + z;
    }

    //weight_2:Similar profucts
    public double Centre_distance(int x, int y, int z)
    {
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
                    if (Data.position[i, j, k] == type)
                    {
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

        double decimals = 0;
        decimals = centre_x - (int)centre_x;
        if (decimals >= 0.5)
        {
            centre_x = (int)centre_x + 1;
        }
        else
        {
            centre_x = (int)centre_x;
        }

        decimals = centre_y - (int)centre_y;
        if (decimals >= 0.5)
        {
            centre_y = (int)centre_y + 1;
        }
        else
        {
            centre_y = (int)centre_y;
        }

        decimals = centre_z - (int)centre_z;
        if (decimals >= 0.5)
        {
            centre_z = (int)centre_z + 1;
        }
        else
        {
            centre_z = (int)centre_z;
        }

        double centre_a = 0;
        double centre_b = 0;
        double centre_c = 0;

        double n = centre_x / 5;
        double m = centre_x % 5 - 1;
        centre_a = -16.36 + m * 1.677 + n * 10.4;
        if ((int)centre_y % 2 == 1)
        {
            centre_y = (int)centre_y / 2;
            centre_b = -3.187 + centre_y * 3;
        }
        else
        {
            centre_y = (int)(centre_y) / 2 - 1;
            centre_b = -3.187 + centre_y * 3 + 0.372;
        }

        //print(centre_z);
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
    public double Core_of_shelf(int x, int y, int z)
    {
        double[] w_sum = new double[4];

        for (int i = 1; i < 4; i++)
        {
            w_sum[i] = 0;
        }

        for (int k = 1; k < 4; k++)
        {
            for (int i = 1; i < 16; i++)
            {
                for (int j = 1; j < 13; j++)
                {
                    w_sum[k] += weight[Data.position[i, j, k]];
                }

            }
        }

        double res = 0;

        res = (w_sum[1] * 1 + w_sum[2] * 2 + w_sum[3] * 3) / (w_sum[1] + w_sum[2] + w_sum[3]);

        return res;
    }

    //Fitness value function 1
    public void Fitness()
    {
        foreach (var individual in population)
        {
            double fitness = 0;
            double w1_fitness = Distance(individual.x, individual.y, individual.z) / 64.4056;
            fitness += 0.6 * (1 - w1_fitness);
            double w2_fitness = Centre_distance(individual.x, individual.y, individual.z) / 31.5221;
            fitness += 0.2 * (1 - w2_fitness);
            double w3_fitness = Core_of_shelf(individual.x, individual.y, individual.z) / 3;
            fitness += 0.2 * (1 - w3_fitness);
            individual.value = fitness;
            //print(fitness);
            //print(individual.x + " " + individual.y + " " + individual.z);
        }
        //print("====================");
        //print("====================");
    }

    //calulate f_max & f_avg
    public void cal_fmax_favg()
    {
        //print("cal_fmax_favg");
        f_sum = 0;
        f_max = 0;
        foreach (var individual in population)
        {
            f_sum += individual.value;
            if (f_max < individual.value)
            {
                f_max = individual.value;
            }
        }
        f_avg = f_sum / (population.Count);
        print(f_max + " " + f_avg);
        print("====================");
        if (best_result.f_avg < f_avg)
        {
            best_result.update(f_avg, population);
        }
    }

    //Roulette Wheel Selection
    public void Selection()
    {
        double p_selection = 0;
        double boundary = 0;
        foreach (var individual in population)
        {
            p_selection = individual.value / f_sum;
            individual.selection = p_selection;
            individual.roulette_left = boundary;
            boundary += p_selection;
            individual.roulette_right = boundary;
            //print(p_selection + " " + individual.roulette_left + " " + individual.roulette_right);
        }
    }

    //Stochastic Tournament
    public Individual Pick_parents()
    {
        double num1 = Random.Range(0.0f, 1.0f);
        double num2 = Random.Range(0.0f, 1.0f);

        Individual tmp1 = new Individual();
        Individual tmp2 = new Individual();

        int it = 0;

        foreach (var individual in population)
        {
            if ((num1 >= individual.roulette_left) && (num1 < individual.roulette_right))
            {
                tmp1 = individual;
                it++;
            }

            if ((num2 >= individual.roulette_left) && (num2 < individual.roulette_right))
            {
                tmp2 = individual;
                it++;
            }
            if (it == 2)
            {
                break;
            }
        }
        return (tmp1.value > tmp2.value ? tmp1 : tmp2);
    }

    //generate offspring
    public void Offspring()
    {
        double p_c;
        double p_m;
        double f_derivation;
        Individual offspring = new Individual();
        double num1;
        double num2;
        int num3;
        bool flag = true;
        flag = true;
        while (flag)
        {
            parent1 = Pick_parents();
            parent2 = Pick_parents();

            f_derivation = parent1.value > parent2.value ? parent1.value : parent2.value;
            p_c = Crossover_rate(f_derivation, f_avg, f_max);
            p_m = Mutation_rate(parent1.value, f_avg, f_max);
            //print(p_c + " " + p_m);

            num1 = Random.Range(0.0f, 1.0f);
            num2 = Random.Range(0.0f, 1.0f);
            //decide which dimension to crossover
            num3 = Random.Range(1, 4);

            //crossover
            if (num1 < p_c)
            {
                if (num3 == 1)
                {
                    offspring.x = parent2.x;
                    offspring.y = parent1.y;
                    offspring.z = parent1.z;
                }
                else if (num3 == 2)
                {
                    offspring.x = parent1.x;
                    offspring.y = parent2.y;
                    offspring.z = parent1.z;
                }
                else
                {
                    offspring.x = parent1.x;
                    offspring.y = parent1.y;
                    offspring.z = parent2.z;
                }
            }
            else
            {
                offspring.x = parent1.x;
                offspring.y = parent1.y;
                offspring.z = parent1.z;
            }

            //decide which dimension to mutation
            num3 = Random.Range(1, 4);

            //mutation
            if (num2 < p_m)
            {
                if (num3 == 1)
                {
                    offspring.x = Random.Range(1, 16);
                }
                else if (num3 == 2)
                {
                    offspring.y = Random.Range(1, 13);
                }
                else
                {
                    offspring.z = Random.Range(1, 4);
                }
                //print(offspring.x + " " + offspring.y + " " + offspring.z);
            }
            offspring.type = 1;

            //print(offspring.x + " " + offspring.y + " " + offspring.z);
            if (Data.position[offspring.x, offspring.y, offspring.z] == 0)
            {
                //print(offspring.x + " " + offspring.y + " " + offspring.z);
                //print("==============================================");
                Data.position[offspring.x, offspring.y, offspring.z] = offspring.type;
                flag = false;
            }
        }

        offspring_list.Enqueue(offspring);
    }

    //exchange between parents and offspring
    public void Exchange()
    {
        population.Sort((x, y) => x.CompareTo(y));
        for (int i = population.Count - 1; i > num - half_num; i--)
        {
            Data.position[population[i].x, population[i].y, population[i].z] = 0;
            population.Remove(population[i]);
        }
        for (int i = 0; i < half_num; i++)
        {
            population.Add(offspring_list.Dequeue());
        }
    }

    private int sortlist(Individual a, Individual b)
    {
        if (a.value > b.value)
        {
            return 1;
        }
        else if (a.value < b.value)
        {
            return -1;
        }
        return 0;
    }

    public void Out_put()
    {
        foreach (var item in best_result.population)
        {
            Vector4 tmp = new Vector4(item.x, item.y, item.z, type);
            Data.position_put.Enqueue(tmp);
        }
    }
}

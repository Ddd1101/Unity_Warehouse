using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{
    public int population_size = 15 * 12 * 3;
    public float A;
    public float max_mutation_rate = 0.05f;
    public float min_mutation_rate = 0.005f;
    public float max_crossover_rate = 0.8f;
    public float min_crossover_rate = 0.5f;
    public float[] height = new float[4] { 0f, 0.145f, 0.679f, 1.1436f };
    public float V_of_shelf = ((0.6737639f - 0.1639082f) * 3f) * (0.362f * 2f) * (1.677f * 5f) * 18f;
    public float[] weight = new float[4] { 0f, 1f, 2f, 3f };
    public float[] turnover = new float[4] { 0f, 0.3f, 0.5f, 0.2f };
    public System.Random random;
    private List<Individual> population;
    public Queue<Individual> offspring_list;
    public float f_max;
    public float f_avg;
    public float f_sum;
    public Individual parent1;
    public Individual parent2;
    public Best_result best_result;
    public int type;
    public int num;
    public int half_num;
    public static int[,,] position = new int[16, 13, 4];


    public GeneticAlgorithm(int type, int num)
    {
        random = new System.Random();
        population = new List<Individual>();
        offspring_list = new Queue<Individual>();
        best_result = new Best_result();
        f_max = 0f;
        f_avg = 0f;
        f_sum = 0f;
        A = 9.903f;
        this.type = type;
        this.num = num;
        if (num % 2 == 1)
        {
            half_num = (int)(num / 2) + 1;
        }
        else
        {
            half_num = num / 2;
        }
        best_result.clear();

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    position[i, j, k] = Data.position[i, j, k];
                }
            }
        }
    }

    //Initialize population
    public void Initialize_population()
    {
        population.Clear();
        int ran_x = 0;
        int ran_y = 0;
        int ran_z = 0;
        bool flag = true;
        //Debug.Log("num = " + num);
        for (int h = 0; h < num; h++)
        {
            flag = true;
            while (flag)
            {
                ran_x = random.Next(1, 16);
                ran_y = random.Next(1, 13);
                ran_z = random.Next(1, 4);
                //ran_type = random.Next(1, 4);
                //Debug.Log(ran_x + " " + ran_y + " " + ran_z);
                if (position[ran_x, ran_y, ran_z] == 0)
                {
                    position[ran_x, ran_y, ran_z] = type;
                    //print(ran_x + " " + ran_y + " " + ran_z);
                    population.Add(new Individual(ran_x, ran_y, ran_z, type));
                    flag = false;
                }
            }
        }
        //Debug.Log("initial population  " + population.Count);
    }

    //crossover rate
    public float Crossover_rate(float f_derivation, float f_avg, float f_max)
    {
        if (f_derivation >= f_avg)
        {
            float member = 0;
            float denominator = 0;

            denominator = max_crossover_rate - min_crossover_rate;

            float exp_element1 = 2 * (f_derivation - f_avg);
            float exp_element2 = f_max - f_avg;
            float exp = A * (exp_element1 / exp_element2);

            member = 1f + (float)System.Math.Exp(exp);

            return ((denominator / member) + min_crossover_rate);
        }
        else
        {
            return max_crossover_rate;
        }
    }

    //mutation rate
    public float Mutation_rate(float f, float f_avg, float f_max)
    {
        if (f >= f_avg)
        {
            float member = 0;
            float denominator = 0;

            denominator = max_mutation_rate - min_mutation_rate;

            float exp_element1 = 2 * (f - f_avg);
            float exp_element2 = f_max - f_avg;
            float exp = A * (exp_element1 / exp_element2);

            member = 1f + (float)System.Math.Exp(exp);

            return (denominator / member + min_mutation_rate);
        }
        else
        {
            return max_mutation_rate;
        }
    }

    //weight_1:Efficiency proirity
    public float Distance(int a, int b, int c)
    {
        float y = 0;
        if (b % 2 == 1)
        {
            b = b / 2;
            y = (float)b * 3;
        }
        else
        {
            b /= 2 - 1;
            y = b * 3f + 0.372f;
        }

        float x = 0;
        if (a >= 3)
        {
            int n = a / 5;
            int m = a % 5 - 1;
            x = (m - 2) * 1.77f + n * 10.4f;
        }
        else if (a == 2)
        {
            x = 1.77f;
        }
        else
        {
            x = 2f * 1.77f;
        }

        float z = height[c];

        int a2 = 15 - a;
        float x2 = 0;
        if (a2 >= 3)
        {
            int n = a2 / 5;
            int m = a2 % 5 - 1;
            x2 = (m - 2) * 1.77f + n * 10.4f;
        }
        else if (a2 == 2)
        {
            x2 = 1.77f;
        }
        else
        {
            x2 = 2f * 1.77f;
        }

        return (x + y * 2 + x2);
    }

    //weight_2:Similar profucts
    public float Centre_distance(int x, int y, int z)
    {
        float centre_x = 0;
        float centre_y = 0;
        float centre_z = 0;
        int num_ = 0;

        for (int i = 1; i < 16; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                for (int k = 1; k < 4; k++)
                {
                    if (position[i, j, k] == type)
                    {
                        centre_x += i;
                        centre_y += j;
                        centre_z += k;
                        num_++;
                    }
                }
            }
        }
        if (num_ == 0)
        {
            return 0;
        }

        //Debug.Log("  num1 =  " + num_);
        centre_x /= num_;
        centre_y /= num_;
        centre_z /= num_;
        //Debug.Log(centre_z);

        float decimals = 0;
        decimals = centre_x - (int)(centre_x);
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

        //Debug.Log(centre_z);
        decimals = centre_z - (int)(centre_z);
        //Debug.Log(centre_z);
        if (decimals >= 0.5)
        {
            centre_z = (int)centre_z + 1;
            //Debug.Log(centre_z);
        }
        else
        {
            centre_z = (int)centre_z;
            //Debug.Log(centre_z);
        }

        float centre_a = 0;
        float centre_b = 0;
        float centre_c = 0;

        float n = centre_x / 5;
        float m = centre_x % 5 - 1;
        centre_a = -16.36f + m * 1.677f + n * 10.4f;
        if ((int)centre_y % 2 == 1)
        {
            centre_y = (int)centre_y / 2;
            centre_b = -3.187f + centre_y * 3;
        }
        else
        {
            centre_y = (int)(centre_y) / 2 - 1;
            centre_b = -3.187f + centre_y * 3 + 0.372f;
        }

        centre_c = height[(int)(centre_z)];
        //Debug.Log("++++++++++++++++++++++++++++_2");

        float a = 0;
        float b = 0;
        float c = 0;

        n = x / 5;
        m = x % 5 - 1;
        a = -16.36f + m * 1.677f + n * 10.4f;

        if (y % 2 == 1)
        {
            y = y / 2;
            b = -3.187f + y * 3;
        }
        else
        {
            y = y / 2 - 1;
            b = -3.187f + y * 3 + 0.372f;
        }

        c = height[z];


        float res = (float)System.Math.Sqrt(System.Math.Pow((a - centre_a), 2) + System.Math.Pow((float)(b - centre_b), 2) + System.Math.Pow((float)(c - centre_c), 2));

        return res;
    }

    //weight_3:Shelf stability
    public float Core_of_shelf(int x, int y, int z)
    {
        float[] w_sum = new float[4];

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
                    w_sum[k] += weight[position[i, j, k]];
                }

            }
        }

        float res = 0;

        res = (w_sum[1] * 1 + w_sum[2] * 2 + w_sum[3] * 3) / (w_sum[1] + w_sum[2] + w_sum[3]);

        return res;
    }

    //Fitness value function 1
    public void Fitness()
    {
        foreach (var individual in population)
        {
            float fitness = 0;
            float w1_fitness = Distance(individual.x, individual.y, individual.z) / (58.132f);
            fitness += 0.6f * (1 - w1_fitness);
            float w2_fitness = Centre_distance(individual.x, individual.y, individual.z) / 31.5221f;
            fitness += 0.2f * (1 - w2_fitness);
            float w3_fitness = Core_of_shelf(individual.x, individual.y, individual.z) / 3.0f;
            fitness += 0.2f * (1 - w3_fitness);
            individual.value = fitness;
        }
    }

    //calulate f_max & f_avg
    public void cal_fmax_favg()
    {
        f_sum = 0;
        f_max = 0;
        foreach (var individual in population)
        {
            f_sum = (float)(f_sum + individual.value);
            if (f_max < individual.value)
            {
                f_max = (float)individual.value;
            }
        }
        f_avg = f_sum / (population.Count);
        if (best_result.f_avg < f_avg)
        {
            best_result.update(f_avg, population);
        }
    }

    //Roulette Wheel Selection
    public void Selection()
    {
        float p_selection = 0;
        float boundary = 0;
        foreach (var individual in population)
        {
            p_selection = (float)(individual.value / f_sum);
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
        float num1 = (float)random.NextDouble();
        float num2 = (float)random.NextDouble();

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
        //Debug.Log(Time.itor + "9----------------------7");
        float p_c;
        float p_m;
        float f_derivation;
        Individual offspring = new Individual();
        float num1;
        float num2;
        int num3;
        bool flag = true;
        flag = true;
        //Debug.Log(Time.itor + "9----------------------6");
        while (flag)
        {
            parent1 = Pick_parents();
            parent2 = Pick_parents();

            f_derivation = (float)(parent1.value > parent2.value ? parent1.value : parent2.value);
            p_c = Crossover_rate(f_derivation, f_avg, f_max);
            p_m = (float)Mutation_rate((float)parent1.value, (float)f_avg, (float)f_max);
            //print(p_c + " " + p_m);

            num1 = (float)random.NextDouble();
            num2 = (float)random.NextDouble();
            //decide which dimension to crossover
            num3 = random.Next(1, 4);
            //Debug.Log(Time.itor + "9----------------------1");
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
            num3 = random.Next(1, 4);
            //Debug.Log(Time.itor + "9----------------------2");
            //mutation
            if (num2 < p_m)
            {
                if (num3 == 1)
                {
                    offspring.x = random.Next(1, 16);
                }
                else if (num3 == 2)
                {
                    offspring.y = random.Next(1, 13);
                }
                else
                {
                    offspring.z = random.Next(1, 4);
                }
            }
            //Debug.Log(Time.itor + "9----------------------3");
            if (position[offspring.x, offspring.y, offspring.z] == 0)
            {
                position[offspring.x, offspring.y, offspring.z] = type;
                flag = false;
            }
            //Debug.Log(type + "9----------------------4");
        }
        //Debug.Log(Time.itor + "9----------------------3");
        offspring_list.Enqueue(offspring);
        //Debug.Log(Time.itor + "9----------------------5");
    }

    //exchange between parents and offspring
    public void Exchange()
    {
        //Debug.Log("in exchange__1");
        population.Sort((x, y) => x.CompareTo(y));
        //Debug.Log(population.Count);
        for (int i = population.Count - 1; i >= num - half_num; i--)
        {
            position[population[i].x, population[i].y, population[i].z] = 0;
            population.Remove(population[i]);
        }
        for (int i = 0; i < half_num; i++)
        {
            population.Add(offspring_list.Dequeue());
        }
        foreach (var individual in offspring_list)
        {
            position[individual.x, individual.y, individual.z] = 0;
        }
        offspring_list.Clear();
        //Debug.Log("in exchange__2");
        //Debug.Log(population.Count);
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
        //Debug.Log("in out_put");
        foreach (var item in best_result.population)
        {
            Vector4 tmp = new Vector4(item.x, item.y, item.z, type);
            Data.position[item.x, item.y, item.z] = type;
            Data.position_put.Enqueue(tmp);
        }
        //Debug.Log(Data.position_put.Count);
    }
}

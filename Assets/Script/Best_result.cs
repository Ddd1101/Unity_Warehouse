using System.Collections;
using System.Collections.Generic;
using System;


public class Best_result
{
    public double f_avg { get; set; }
    public List<Individual> population;

    public Best_result()
    {
        f_avg = 0;
        population = new List<Individual>();
    }

    public void update(double f_avg, List<Individual> popul)
    {
        this.f_avg = f_avg;
        this.population.Clear();
        Individual tmp;
        foreach (var item in popul)
        {
            tmp = new Individual();
            tmp.x = item.x;
            tmp.y = item.y;
            tmp.z = item.z;
            tmp.value = item.value;
            population.Add(tmp);
        }
    }
}

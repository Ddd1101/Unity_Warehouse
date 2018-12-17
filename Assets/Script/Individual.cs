using System;

public class Individual
{
    public int x { get; set; }//基因
    public int y { get; set; }
    public int z { get; set; }
    public int age { get; set; }
    public double value { get; set; }
    public int type { get; set; }
    public double selection { get; set; }
    public double roulette_left { get; set; }
    public double roulette_right { get; set; }

    public Individual(int xx, int yy, int zz, int type)
    {
        this.x = xx;
        this.y = yy;
        this.z = zz;
        this.type = type;
        roulette_left = roulette_right = 0;
        selection = 0;
    }

    public Individual()
    {

    }

    public int CompareTo(Individual other)
    {
        if (null == other)
        {
            return 1;
        }
        return other.value.CompareTo(this.value);
    }
}

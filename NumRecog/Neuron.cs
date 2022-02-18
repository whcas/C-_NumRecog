namespace NumRecog
{
    public class Neuron
    {
        double weight;
        double weightUpdate;

        public Neuron(double weight)
        {
            this.weight = weight;
        }

        public double getWeight()
        {
            return weight;
        }
        public void update(double update)
        {
            weight += update;
        }
        public double getUpdate()
        {
            return weightUpdate;
        }
    }
}
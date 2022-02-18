namespace NumRecog
{
    public struct WeightAndBiasChanges
        {
            public double[,] weightChanges;
            public double[] biasChanges;

            internal static WeightAndBiasChanges[] getAverage(WeightAndBiasChanges[][] updates)
            {
                WeightAndBiasChanges[] total = new WeightAndBiasChanges[updates[0].Length];

                for (int i = 0; i < total.Length; i++)
                {
                    total[i].biasChanges = new double[updates[0][i].biasChanges.Length];
                    total[i].weightChanges = new double[updates[0][i].weightChanges.GetLength(0), updates[0][i].weightChanges.GetLength(1)];
                }

                for (int i = 0; i < updates.Length; i++)
                {
                    for (int j = 0; j < updates[i].Length; j++)
                    {
                        total[j] = add(total[j], updates[i][j]);
                    }
                }

                return average(total, updates.GetLength(0));

            }

            private static WeightAndBiasChanges[] average(WeightAndBiasChanges[] total, int len)
            {
                WeightAndBiasChanges[] averaged = total;

                for (int i = 0; i < averaged.Length; i++)
                {
                    for (int j = 0; j < averaged[i].biasChanges.Length; j++)
                    {
                        averaged[i].biasChanges[i] /= len;
                    }

                    for (int j = 0; j < averaged[i].weightChanges.GetLength(0); j++)
                    {
                        for (int k = 0; k < averaged[i].weightChanges.GetLength(1); k++)
                        {
                            averaged[i].weightChanges[j, k] /= len;
                        }
                    }
                }

                return averaged;
            }

            private static WeightAndBiasChanges add(WeightAndBiasChanges weightAndBiasChanges1, WeightAndBiasChanges weightAndBiasChanges2)
            {
                WeightAndBiasChanges added = new WeightAndBiasChanges();
                added.biasChanges = new double[weightAndBiasChanges1.biasChanges.Length];
                added.weightChanges = new double[weightAndBiasChanges1.weightChanges.GetLength(0), weightAndBiasChanges1.weightChanges.GetLength(1)];

                for (int i = 0; i < added.biasChanges.Length; i++)
                {
                    added.biasChanges[i] = weightAndBiasChanges1.biasChanges[i] + weightAndBiasChanges2.biasChanges[i];
                }

                for (int i = 0; i < added.weightChanges.GetLength(0); i++)
                {
                    for (int j = 0; j < added.weightChanges.GetLength(1); j++)
                    {
                        added.weightChanges[i, j] = weightAndBiasChanges1.weightChanges[i, j] + weightAndBiasChanges2.weightChanges[i, j];
                    }
                }

                return added;
            }
        }
}
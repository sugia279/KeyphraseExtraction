using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KeyphraseExtraction.KEUtilities
{
    public class NaiveBayesClassifier
    {
        private static NaiveBayesClassifier _instance;

        public static NaiveBayesClassifier Instance()
        {
            if (_instance == null)
            {
                _instance = new NaiveBayesClassifier();
            }
            return _instance;
        }
        
        public DataTable TrainClassifier(DataTable table)
        {
            //table
            DataTable GaussianDistribution = new DataTable("GaussianClassifierData");
            GaussianDistribution.Columns.Add(table.Columns[0].ColumnName);

            //columns
            for (int i = 1; i < table.Columns.Count; i++)
            {
                GaussianDistribution.Columns.Add(table.Columns[i].ColumnName + "Mean");
                GaussianDistribution.Columns.Add(table.Columns[i].ColumnName + "Variance");
            }

            //calc data
            var results = (from myRow in table.AsEnumerable()
                           group myRow by myRow.Field<string>(table.Columns[0].ColumnName) into g
                           select new { Name = g.Key, Count = g.Count() }).ToList();

            for (int j = 0; j < results.Count; j++)
            {
                DataRow row = GaussianDistribution.Rows.Add();
                row[0] = results[j].Name;

                int a = 1;
                for (int i = 1; i < table.Columns.Count; i++)
                {
                    row[a] = StatisticCalculationHelper.Mean(SelectRows(table, i, string.Format("{0} = '{1}'", table.Columns[0].ColumnName, results[j].Name)));
                    row[++a] = StatisticCalculationHelper.Variance(SelectRows(table, i, string.Format("{0} = '{1}'", table.Columns[0].ColumnName, results[j].Name)));
                    a++;
                }
            }

            return GaussianDistribution;
        }


        public string Classify(DataTable GaussianClassifierData,List<double> sample, Dictionary<string, double> score, double positiveTrainedTermsRatio)
        {
            for (int i = 0; i < GaussianClassifierData.Rows.Count; i++)
            {
                List<double> subScoreList = new List<double>();
                int a = 1, b = 1;
                for (int k = 1; k < GaussianClassifierData.Columns.Count; k = k + 2)
                {
                    double mean = Convert.ToDouble(GaussianClassifierData.Rows[i][a]);
                    double variance = Convert.ToDouble(GaussianClassifierData.Rows[i][++a]);
                    double result = StatisticCalculationHelper.NormalDist(sample[b - 1], mean, StatisticCalculationHelper.SquareRoot(variance));
                    subScoreList.Add(result);
                    a++; b++;
                }

                double finalScore = positiveTrainedTermsRatio;
                for (int z = 0; z < subScoreList.Count; z++)
                {
                    finalScore = finalScore * subScoreList[z];
                }

                score.Add((string)GaussianClassifierData.Rows[i][0], finalScore);
            }

            double maxOne = score.Max(c => c.Value);
            var name = (from c in score
                        where c.Value == maxOne
                        select c.Key).First();

            return name;
        }

        #region Helper Function

        public IEnumerable<double> SelectRows(DataTable table, int column, string filter)
        {
            List<double> _doubleList = new List<double>();
            DataRow[] rows = table.Select(filter);
            for (int i = 0; i < rows.Length; i++)
            {
                _doubleList.Add((double)rows[i][column]);
            }

            return _doubleList;
        }

        #endregion
    }
}

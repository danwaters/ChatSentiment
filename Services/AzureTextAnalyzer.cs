using System;
using System.Threading.Tasks;

namespace Sentiment.Services
{
    public class AzureTextAnalyzer : IAnalyzeText
    {
        public AzureTextAnalyzer()
        {
        }

        public async Task<float> AnalyzeSentiment(string input)
        {
            throw new NotImplementedException();
        }
    }
}

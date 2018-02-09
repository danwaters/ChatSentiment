using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sentiment.Services
{
    public class AzureTextAnalyzer : IAnalyzeText
    {
        public AzureTextAnalyzer()
        {
        }

        public async Task<double?> AnalyzeSentiment(string input)
        {
            var client = new TextAnalyticsAPI();
            client.AzureRegion = AzureRegions.Westus ;
            client.SubscriptionKey = "7003dfb3f5484db4829a98c046cef71b";
            var langInput = new MultiLanguageInput("en", "1", input);
            var batchInput = new MultiLanguageBatchInput(new List<MultiLanguageInput> { langInput });
            var sentiment = await client.SentimentAsync(batchInput);
            var result = sentiment.Documents.Single();
            return result.Score;
        }
    }
}

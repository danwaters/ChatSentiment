using System;
using System.Threading.Tasks;
namespace Sentiment.Services
{
    public interface IAnalyzeText
    {
        Task<double?> AnalyzeSentiment(string input); 
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Mynt.Core.Binance;
using Mynt.Core.Strategies;
using Mynt.Core.TradeManagers;

namespace Mynt.Functions
{
    public static class SellTimer
    {
        [FunctionName("SellTimer")]
        public static async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info("Starting processing...");

                // Call the Bittrex Trade manager with the strategy of our choosing.
                var manager = new GenericTradeManager(
                    new BinanceApi(),
                    new TheScalper(),
                    null, (a) => log.Info(a)
                );

                // Call the process method to start processing the current situation.
                await manager.UpdateRunningTrades();

                log.Info("Done...");
            }
            catch (Exception ex)
            {
                // If anything goes wrong log an error to Azure.
                log.Error(ex.Message + ex.StackTrace);
            }
        }
    }
}

namespace Slot_LoadTesting
{
    public class ResponseAnalyzer
    {
        private int _countOfErrorsSinceLastSuccess = 0;
        private bool _lastMessageWereSucceeded = true;
        private int _howManyMlsSleepWhenAnyErrorHappened;

        public ResponseAnalyzer(int HowManyMlsSleepWhenAnyErrorHappened)
        {
            _howManyMlsSleepWhenAnyErrorHappened = HowManyMlsSleepWhenAnyErrorHappened;
        }

        public void Analyze(string responseString, Logger logger)
        {
            if (responseString.Contains("error"))
            {
                _lastMessageWereSucceeded = false;
                var mlsForSleep = _howManyMlsSleepWhenAnyErrorHappened * (++_countOfErrorsSinceLastSuccess);
                logger.WriteLine("Error occured : sleeping for " + mlsForSleep);
                SmartThreadSleep.Sleep(mlsForSleep);
            }
            else
            {
                _lastMessageWereSucceeded = true;
                _countOfErrorsSinceLastSuccess = 0;
            }
        }
    }
}

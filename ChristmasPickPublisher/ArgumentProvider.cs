using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChristmasPickPublisher
{
    public interface IArgumentProvider
    {
        Task<string[]> GetArguments();
        void Cancel();
    }
    public class ArgumentProvider : IArgumentProvider
    {
        private string[] args;
        private readonly CancellationTokenSource tokenSource;
        
        public ArgumentProvider(string[] args, CancellationTokenSource tokenSource)
        {
            this.args = args;
            this.tokenSource = tokenSource;
        }

        public Task<string[]> GetArguments()
        {
            return Task.FromResult(args);
        }

        public void Cancel()
        {
            tokenSource.Cancel();
        }
    }
}

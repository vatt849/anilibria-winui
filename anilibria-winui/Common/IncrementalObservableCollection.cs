using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace anilibria.Common
{
    public class IncrementalObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public Func<Task<(IEnumerable<T> Results, int TotalResults)>> LoadCallback { get; set; }

        public bool HasMoreItems => Count < TotalResults;

        public int PageNumber { get; set; }

        public int TotalResults { get; set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
            => InternalLoadMoreItemsAsync().AsAsyncOperation();

        private async Task<LoadMoreItemsResult> InternalLoadMoreItemsAsync()
        {
            if (LoadCallback is null)
            {
                return new LoadMoreItemsResult();
            }
            var results = await LoadCallback();
            foreach (var item in results.Results)
            {
                Add(item);
            }

            TotalResults = results.TotalResults;

            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TotalResults)));

            return new LoadMoreItemsResult((uint)results.Results.Count());
        }
    }
}

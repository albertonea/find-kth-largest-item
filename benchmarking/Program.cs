using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<FindKthLargestElement>();

// long[] arr = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
// var f = new FindKthLargestElement();
// var x = f.FindKthLargest(arr, 0, arr.Length - 1);
// Console.WriteLine(x);
//
// f.QuickSort(arr, 0, arr.Length - 1);
// arr.ToList().ForEach(x => Console.WriteLine(x));

[SimpleJob(RuntimeMoniker.Net80)]
public class FindKthLargestElement
{
    private long[] data;

    [Params(10000, 1000000)] public int N;

    [GlobalSetup]
    public void Setup()
    {
        data = new long[N];
        var randNum = new Random(42);
        for (var i = 0; i < N; i++) data[i] = randNum.NextInt64();
    }

    private int Partition(long[] A, int p, int r)
    {
        var x = A[r];
        var i = p - 1;
        for (var j = p; j < r; j++)
            if (A[j] <= x)
            {
                i = i + 1;
                (A[i], A[j]) = (A[j], A[i]);
            }

        (A[i + 1], A[r]) = (A[r], A[i + 1]);
        return i + 1;
    }

    public void QuickSort(long[] A, int p, int r)
    {
        if (p < r)
        {
            var q = Partition(A, p, r);
            QuickSort(A, p, q - 1);
            QuickSort(A, q + 1, r);
        }
    }

    public long FindKthLargest(long[] A, int p, int r, int k)
    {
        var left = 0;
        var right = A.Length - 1;
        while (left <= right)
        {
            var newPivotIdx = Partition(A, left, right);
            if (newPivotIdx == k-1)
            {
                return A[newPivotIdx];
            } else if (newPivotIdx > k-1)
            {
                right = newPivotIdx - 1;
            }
            else
            {
                left = newPivotIdx + 1;
            }
        }

        return -1;
    }

    [Benchmark]
    public void FullQuicksort()
    {
        var copy = new long[data.Length];
        data.CopyTo(copy, 0);
        QuickSort(copy, 0, data.Length - 1);
    }

    [Benchmark]
    public long PartialQuicksort()
    {
        var copy = new long[data.Length];
        data.CopyTo(copy, 0);
        return FindKthLargest(copy, 0, data.Length - 1, N/100);
    }
}
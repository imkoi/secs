using Secs;
using FluentAssertions;
using NUnit.Framework;

public class QuickSortTest
{
    [Test]
    public unsafe void SortFromMinimumToMaximum()
    {
        var array = stackalloc int[] { 6, 7, 8, 9, 10, 1, 2, 3, 4, 5 };
        
        Registry.QuickSort(array, 0, 9, 1);
        
        array[0].Should().Be(1);
        array[1].Should().Be(2);
        array[2].Should().Be(3);
        array[3].Should().Be(4);
        array[4].Should().Be(5);
        array[5].Should().Be(6);
        array[6].Should().Be(7);
        array[7].Should().Be(8);
        array[8].Should().Be(9);
        array[9].Should().Be(10);
    }
    
    [Test]
    public unsafe void SortFromMaximumToMinimum()
    {
        var array = stackalloc int[] { 6, 7, 8, 9, 10, 1, 2, 3, 4, 5 };
        
        Registry.QuickSort(array, 0, 9, -1);
        
        array[0].Should().Be(10);
        array[1].Should().Be(9);
        array[2].Should().Be(8);
        array[3].Should().Be(7);
        array[4].Should().Be(6);
        array[5].Should().Be(5);
        array[6].Should().Be(4);
        array[7].Should().Be(3);
        array[8].Should().Be(2);
        array[9].Should().Be(1);
    }
    
    [Test]
    public unsafe void SortCorrectSpan()
    {
        var array = stackalloc int[] { 6, 7, 8, 9, 10, 1, 2, 3, 4, 5 };
        
        Registry.QuickSort(array, 0, 4, -1);
        
        array[0].Should().Be(10);
        array[1].Should().Be(9);
        array[2].Should().Be(8);
        array[3].Should().Be(7);
        array[4].Should().Be(6);
        array[5].Should().Be(1);
        array[6].Should().Be(2);
        array[7].Should().Be(3);
        array[8].Should().Be(4);
        array[9].Should().Be(5);
    }
}